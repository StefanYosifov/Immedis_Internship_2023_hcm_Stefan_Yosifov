namespace HCM.Common.Requests
{
    using RestSharp;

    public class RestRequestBuilder
    {

        private readonly string resource;
        private Method method;
        private readonly string contentType;
        private object body;
        private readonly RestRequest request;

        public RestRequestBuilder(string resource)
        {
            this.resource = resource;
            this.method = Method.Get;
            this.contentType = "application/json";
            this.request = new RestRequest(this.resource);
        }

        public RestRequestBuilder SetMethod(Method method)
        {
            this.method = method;
            this.request.Method = method;
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

        public RestRequestBuilder AddHeader(string name, string value)
        {
            request.AddHeader("Accept", "application/json");
            request.AddHeader(name, value);
            return this;
        }

        public RestRequestBuilder AddJsonBody(object body)
        {
            this.body = body;
            return this;
        }

        public RestRequest Build()
        {
            if (method == Method.Post || method == Method.Put)
            {
                request.AddHeader("Content-Type", contentType);
                request.AddBody(body);
            }

            return request;
        }
    }
}
