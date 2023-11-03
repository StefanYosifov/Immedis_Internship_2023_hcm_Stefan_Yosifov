namespace HCM.Common.Filters
{
    using System.Text;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using RestSharp;

    public class ControllerActionResponseFilter : Attribute, IAsyncResultFilter
    {
        private string apiRoute = "";

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            next();

            var result = context.Result as ObjectResult;
            var response = result.Value as RestResponse;

            context.HttpContext.Response.ContentLength = response.ContentLength;
            await context.HttpContext.Response.WriteAsync(response.Content, Encoding.UTF8);
        }
    }
}