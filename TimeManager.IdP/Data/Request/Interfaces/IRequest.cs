namespace TimeManager.IdP.Data
{
    public interface IRequest<T>
    {
        public T Data { get; set; }
    }
}
