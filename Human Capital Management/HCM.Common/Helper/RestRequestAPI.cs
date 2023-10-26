namespace HCM.Common.Helper
{
    using RestSharp;

    public class RestRequestAPI
    {

        public RestRequestAPI(string url, RestSharp.Method method)
        {
            request = new RestRequest(url, method);
        }

        private RestRequest request;

        

    }
}
