namespace HCM.Common.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ControllerActionResponseFilter : Attribute, IAsyncResultFilter
    {
        private string apiRoute = "";

        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}