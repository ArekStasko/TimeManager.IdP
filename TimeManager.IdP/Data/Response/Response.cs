namespace TimeManager.IdP.Data.Response
{
    public class Response<T> : IResponse<T>
    {
        public T Data { get; }
        public ApiException Exception { get; }

        public Response(T data) => Data = data;
        public Response(Exception ex) => Exception = new ApiException(500, ex.Message);
        
    }
}
