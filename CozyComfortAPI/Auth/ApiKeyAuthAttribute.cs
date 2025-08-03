using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CozyComfortAPI.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEY_HEADER_NAME = "X-API-KEY";
        private readonly string _requiredRole;

        public ApiKeyAuthAttribute(string requiredRole = null)
        {
            _requiredRole = requiredRole;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEY_HEADER_NAME, out var extractedApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var manufacturerApiKey = configuration.GetValue<string>("ApiKeys:CozyComfortManufacturerKey"); 
            var distributorKey = configuration.GetValue<string>("ApiKeys:CozyComfortDistributorKey");

            bool isManufacturerKey = extractedApiKey == manufacturerApiKey;
            bool isDistributorKey = extractedApiKey == distributorKey;

            if (!isManufacturerKey && !isDistributorKey)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!string.IsNullOrEmpty(_requiredRole))
            {
                if (_requiredRole == "Manufacturer")
                {
                    if (!isManufacturerKey)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
                else if (_requiredRole == "Distributor")
                {
                    if (!isDistributorKey && !isManufacturerKey)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }

            await next();
        }
    }
}