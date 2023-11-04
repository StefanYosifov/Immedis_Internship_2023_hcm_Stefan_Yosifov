namespace HCM.Common.Requests
{
    using Microsoft.AspNetCore.Http;

    using RestSharp;

    public class RestRequestBuilder
    {
        private readonly string? jwtToken;
        private readonly RestRequest request;

        private readonly string resource;
        private string contentType;
        private Method method;

        public RestRequestBuilder(string resource, string? token)
        {
            this.resource = resource;
            jwtToken = token;
            method = Method.Get;
            contentType = "application/json";
            request = new RestRequest(this.resource);
        }

        public RestRequestBuilder SetMethod(Method method)
        {
            this.method = method;
            request.Method = method;
            return this;
        }

        public RestRequestBuilder AddQueryParameter(object obj)
        {
            if (obj != null)
            {
                var type = obj.GetType();
                foreach (var property in type.GetProperties())
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(obj, null)?.ToString();

                    request.AddQueryParameter(propertyName, propertyValue);
                }
            }

            return this;
        }

        public RestRequestBuilder AddParameter(params string[] queryParams)
        {
            foreach (var param in queryParams)
            {
                request.AddParameter(nameof(param), param);
            }

            return this;
        }

        public RestRequestBuilder AddQuery(params string[] queryParams)
        {
            foreach (var param in queryParams)
            {
                request.AddQueryParameter(nameof(param), param);
            }

            return this;
        }

        public RestRequestBuilder AddHeader(string name, string value)
        {
            request.AddHeader("Accept", "application/json");
            request.AddHeader(name, value);
            return this;
        }

        public RestRequestBuilder AddAuthentication()
        {
            if (jwtToken.StartsWith("Bearer"))
            {
                request.AddHeader("Authorization", jwtToken);
            }
            else
            {
                request.AddHeader("Authorization", $"Bearer {jwtToken}");
            }

            return this;
        }

        public RestRequestBuilder AddBody(object body)
        {
            request.AddBody(body);
            return this;
        }

        public RestRequestBuilder AddFile(IFormFile file)
        {
            byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            contentType = file.ContentType;
            request.AddFile(file.Name, fileBytes, Path.GetFileName(file.Name), contentType);
            return this;
        }

        public RestRequest Build()
        {
            if (method == Method.Post || method == Method.Put)
            {
                request.AddHeader("Content-Type", contentType);
            }

            return request;
        }
    }
}