namespace HCM.Common.Requests
{
    using RestSharp;

    public class RestPostRequest
    {
        private readonly RestClient client;
        private readonly RestRequest request;
        public RestPostRequest(string baseUrl, RestClient client, string token)
        {
            this.client = client;
            this.request = new RestRequest(baseUrl);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authentication", $"Bearer {token}");
        }
        public async Task<RestResponse<TResponse>> SendRequest<TResponse>(string resource, params string[] queryParams)
        {
            if (queryParams.Length > 0)
            {
                foreach (var param in queryParams)
                {
                    request.AddQueryParameter(nameof(param), param);
                }
            }

            return await client.ExecutePostAsync<TResponse>(request);
        }

    }
}
