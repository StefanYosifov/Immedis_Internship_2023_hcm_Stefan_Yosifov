namespace HCM.Common.Requests
{
    using RestSharp;

    public class RestFactoryRequest
    {
        private readonly RestClient restClient;
        private readonly string url;

        public RestFactoryRequest(string url, RestClient restClient)
        {
            this.url = url;
            this.restClient = restClient;
        }

        public TRequest CreateRequest<TRequest>() where TRequest : IRestRequest
        {
            if (typeof(TRequest) == typeof(RestGetRequest))
            {
                return (TRequest)(new RestGetRequest(url, restClient) as IRestRequest);
            }
            //else if (typeof(TRequest) == typeof(PostRequest))
            //{
            //    return new PostRequest(_baseUrl) as TRequest;
            //}
            //else if (typeof(TRequest) == typeof(PutRequest))
            //{
            //    return new PutRequest(_baseUrl) as TRequest;
            //}
            //else if (typeof(TRequest) == typeof(DeleteRequest))
            //{
            //    return new DeleteRequest(_baseUrl) as TRequest;
            //}

            throw new NotSupportedException("Unsupported request type");
        }
    }
}