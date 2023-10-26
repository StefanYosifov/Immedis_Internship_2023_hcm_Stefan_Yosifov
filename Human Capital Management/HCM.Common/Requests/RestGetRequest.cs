namespace HCM.Common.Requests
{
    using RestSharp;

    public class RestGetRequest 
    {

        private readonly RestClient client;
        private readonly RestRequest request;
        public RestGetRequest(string baseUrl, RestClient client)
        {
            this.client = client;
            this.request = new RestRequest(baseUrl);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authentication", $"Bearer");
        }
        public async Task<RestResponse<TResponse>> SendRequest<TResponse>( params string[] queryParams)
        {
            if (queryParams.Length > 0)
            {
                foreach (var param in queryParams)
                {
                    request.AddQueryParameter(nameof(param), param);
                }
            }
            return await client.ExecuteGetAsync<TResponse>(request);
        }
    }
}
