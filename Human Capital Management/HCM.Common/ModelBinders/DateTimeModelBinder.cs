namespace HCM.Common.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            return Task.FromResult(valueProviderResult);
        }
    }
}
