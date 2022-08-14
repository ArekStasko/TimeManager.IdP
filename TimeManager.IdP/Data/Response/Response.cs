namespace TimeManager.IdP.Data.Response
{
    public class Response<T> : IResponse<T>
    {
        public T Data { get; set; }
        public ApiException Exception { get; set; }

        public Response(T data) => Data = data;
        public Response(Exception ex, string title) => Exception = new ApiException(ex, title);
    }
}
