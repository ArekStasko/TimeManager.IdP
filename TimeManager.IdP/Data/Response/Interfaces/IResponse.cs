namespace TimeManager.IdP.Data.Response
{
    public interface IResponse<T>
    {
        public T Data { get; }
        public ApiException Exception { get; }
    }
}
