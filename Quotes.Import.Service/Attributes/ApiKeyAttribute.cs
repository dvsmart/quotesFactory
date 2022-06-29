using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Quotes.Import.Service.Attributes
{
    /// <summary>
    /// Api key attribute
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Method | AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        public const string ApiKeyName = "X-Api-Key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<ServiceAuthenticationConfig>();
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ApiKeyAttribute>>();
            var apiKey = appSettings.ApiKey;

            if (apiKey is null)
            {
                logger.LogCritical("Missing X-API-Key in the api");
                context.Result = new StatusCodeResult((int)HttpStatusCode.InternalServerError);
                return;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
            {
                logger.LogError("Missing X-API-Key header");
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            if (!apiKey.Equals(extractedApiKey))
            {
                logger.LogError("X-API-Key header is invalid");
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }

            await next();
        }
    }
}
