using System.Diagnostics;
using iBudget.Interfaces;
using iBudget.Models;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace iBudget.Framework;

public sealed class PlaywrightPdfRenderer : IPdfRenderer, IAsyncDisposable
{
    private readonly SemaphoreSlim _browserLock = new(1, 1);
    private readonly SemaphoreSlim _renderSlots;
    private readonly PdfOptions _options;
    private readonly ILogger<PlaywrightPdfRenderer> _logger;
    private IPlaywright _playwright;
    private IBrowser _browser;
    private bool _disposed;

    public PlaywrightPdfRenderer(
        IOptions<PdfOptions> options,
        ILogger<PlaywrightPdfRenderer> logger
    )
    {
        _options = options.Value;
        _logger = logger;
        _renderSlots = new SemaphoreSlim(_options.MaxConcurrentPages, _options.MaxConcurrentPages);
    }

    public async Task<byte[]> RenderAsync(
        string url,
        CancellationToken cancellationToken = default
    )
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (
            !Uri.TryCreate(url, UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        )
            throw new ArgumentException("A URL do PDF deve ser HTTP ou HTTPS absoluta.", nameof(url));

        await _renderSlots.WaitAsync(cancellationToken);
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var browser = await GetBrowserAsync(cancellationToken);
            await using var context = await browser
                .NewContextAsync(new BrowserNewContextOptions { JavaScriptEnabled = false })
                .WaitAsync(cancellationToken);
            var page = await context.NewPageAsync().WaitAsync(cancellationToken);
            page.SetDefaultNavigationTimeout(_options.NavigationTimeoutSeconds * 1000);

            var response = await page.GotoAsync(
                uri.ToString(),
                new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.Load,
                    Timeout = _options.NavigationTimeoutSeconds * 1000
                }
            ).WaitAsync(cancellationToken);

            if (response == null || !response.Ok)
                throw new InvalidOperationException(
                    $"Não foi possível carregar o HTML do PDF. Status: {response?.Status}."
                );

            var pdf = await page.PdfAsync(
                new PagePdfOptions
                {
                    Format = "A4",
                    PrintBackground = true,
                    PreferCSSPageSize = false
                }
            ).WaitAsync(cancellationToken);

            _logger.LogInformation(
                "PDF gerado para {Path} em {ElapsedMilliseconds} ms com {Size} bytes.",
                uri.AbsolutePath,
                stopwatch.ElapsedMilliseconds,
                pdf.Length
            );

            return pdf;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                "Geração de PDF cancelada para {Path} após {ElapsedMilliseconds} ms.",
                uri.AbsolutePath,
                stopwatch.ElapsedMilliseconds
            );
            throw;
        }
        catch (PlaywrightException exception) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                "Geração de PDF cancelada para {Path} após {ElapsedMilliseconds} ms.",
                uri.AbsolutePath,
                stopwatch.ElapsedMilliseconds
            );
            throw new OperationCanceledException(
                "A geração de PDF foi cancelada.",
                exception,
                cancellationToken
            );
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Falha ao gerar PDF para {Path} após {ElapsedMilliseconds} ms.",
                uri.AbsolutePath,
                stopwatch.ElapsedMilliseconds
            );
            throw;
        }
        finally
        {
            _renderSlots.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;
        await _browserLock.WaitAsync();

        try
        {
            if (_browser != null)
                await _browser.CloseAsync();

            _playwright?.Dispose();
            _browser = null;
            _playwright = null;
        }
        finally
        {
            _browserLock.Release();
            _browserLock.Dispose();
            _renderSlots.Dispose();
        }
    }

    private async Task<IBrowser> GetBrowserAsync(CancellationToken cancellationToken)
    {
        if (_browser?.IsConnected == true)
            return _browser;

        await _browserLock.WaitAsync(cancellationToken);

        try
        {
            if (_browser?.IsConnected == true)
                return _browser;

            if (_browser != null)
                await _browser.CloseAsync();

            _browser = null;
            _playwright?.Dispose();
            _playwright = null;

            var playwright = await Playwright.CreateAsync();

            try
            {
                using var cancellationRegistration = cancellationToken.Register(playwright.Dispose);
                var browser = await playwright.Chromium.LaunchAsync(
                    new BrowserTypeLaunchOptions
                    {
                        Headless = true,
                        Args =
                        [
                            "--no-sandbox",
                            "--disable-setuid-sandbox",
                            "--disable-dev-shm-usage"
                        ]
                    }
                );

                cancellationRegistration.Dispose();
                cancellationToken.ThrowIfCancellationRequested();
                _playwright = playwright;
                _browser = browser;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                playwright.Dispose();
                throw;
            }
            catch (PlaywrightException exception) when (cancellationToken.IsCancellationRequested)
            {
                playwright.Dispose();
                throw new OperationCanceledException(
                    "A inicialização do navegador para geração de PDF foi cancelada.",
                    exception,
                    cancellationToken
                );
            }
            catch
            {
                playwright.Dispose();
                throw;
            }

            return _browser;
        }
        finally
        {
            _browserLock.Release();
        }
    }
}
