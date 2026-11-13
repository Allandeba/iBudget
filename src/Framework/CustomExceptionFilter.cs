using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shared;

namespace iBudget.Framework;

public class CustomExceptionFilter : IExceptionFilter
{
    private readonly IModelMetadataProvider _modelMetadataProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomExceptionFilter> _logger;

    public CustomExceptionFilter(IModelMetadataProvider modelMetadataProvider,
        IConfiguration configuration, ILogger<CustomExceptionFilter> logger)
    {
        _modelMetadataProvider = modelMetadataProvider;
        _configuration = configuration;
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        ViewResult result = new() { ViewName = Constants.CustomErrorView };
        result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);

        if (context.Exception is DbUpdateException dbUpdateException)
        {
            if (IsUniqueConstraintViolationException(dbUpdateException))
                result.ViewData.Add(Constants.ThrowDBException, Messages.UniqueDBMessage);
            else if (IsForeignKeyViolationException(dbUpdateException))
                result.ViewData.Add(Constants.ThrowDBException, Messages.ForeignKeyDBMessage);
            else
                result.ViewData.Add(Constants.ThrowDBException, Messages.GenericDBExceptionMessage);
        }
        else
        {
            if (_configuration.GetValue<bool>("Deployment:PublicDemo"))
            {
                _logger.LogError(context.Exception, "Falha não tratada na demonstração pública.");
                result.StatusCode = StatusCodes.Status500InternalServerError;
                result.ViewData.Add(Constants.ThrowDBException, "Ocorreu uma falha inesperada. Tente novamente.");
            }
            else
            {
                result.ViewData.Add(Constants.ThrowException, context.Exception);
            }
        }

        context.ExceptionHandled = false;
        context.Result = result;
    }

    private bool IsUniqueConstraintViolationException(DbUpdateException dbUpdateException)
    {
        return dbUpdateException.InnerException is PostgresException postgresException
               && postgresException.SqlState == "23505"; // Unique Key Exception PostgreSQL
    }

    private bool IsForeignKeyViolationException(DbUpdateException dbUpdateException)
    {
        return dbUpdateException.InnerException is PostgresException postgresException
               && postgresException.SqlState == "23503"; // Foreign Key Exception PostgreSQL
    }
}