namespace HCM.Common.Helpers
{
    using RestSharp;

    public class RestRequestAPI
    {
        private RestRequest request;

        public RestRequestAPI(string url, Method method)
        {
            request = new RestRequest(url, method);
        }
    }
}