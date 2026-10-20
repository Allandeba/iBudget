namespace iBudget.Interfaces;

public interface IPdfRenderer
{
    Task<byte[]> RenderAsync(string url, CancellationToken cancellationToken = default);
}
