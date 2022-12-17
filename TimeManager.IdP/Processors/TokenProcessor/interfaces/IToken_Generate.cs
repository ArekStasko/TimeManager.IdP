using TimeManager.IdP.Data;

namespace TimeManager.IdP.Processors.TokenProcessor
{
    public interface IToken_Generate
    {
        public string Execute(User user);
    }
}
