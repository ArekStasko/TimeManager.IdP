using TimeManager.IdP.Processors.TokenProcessor;
using TimeManager.IdP.Processors.UserProcessor;

namespace TimeManager.IdP.services
{
    public interface IProcessors
    {
        public IToken_Generate token_Generate { get; }
        public IToken_Verify token_Verify { get; }

        public IUser_Login user_Login { get; }
        public IUser_Register user_Register { get; }

    }
}
