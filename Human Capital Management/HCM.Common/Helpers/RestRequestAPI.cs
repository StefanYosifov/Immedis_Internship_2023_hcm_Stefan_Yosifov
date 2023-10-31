namespace HCM.Common.Helpers
{
    using RestSharp;

    public class RestRequestAPI
    {

        public RestRequestAPI(string url, Method method)
        {
            request = new RestRequest(url, method);
        }

        private RestRequest request;

        

    }
}
