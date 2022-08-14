using TimeManager.IdP.Authentication;

namespace TimeManager.IdP.Data
{
    public class Request<T> : IRequest<T>
    {
        public T Data { get; set; }
        public Token Token { get; set; }
    }
}