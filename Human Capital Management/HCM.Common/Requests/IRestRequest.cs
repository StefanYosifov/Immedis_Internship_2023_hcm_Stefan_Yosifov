namespace HCM.Common.Requests
{
    using RestSharp;

    public interface IRestRequest
    {
        public Task<RestResponse<TResponse>> SendRequest<TRequest, TResponse>(TRequest model,
            params string[] queryParams);
    }
}