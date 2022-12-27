using TimeManager.IdP.Processors.TokenProcessor;
using TimeManager.IdP.Processors.UserProcessor;
using Autofac;
using TimeManager.IdP.Authentication;
using TimeManager.IdP.Data;

namespace TimeManager.IdP.services
{
    public class Processors : IProcessors
    {
        public Processors(DataContext context, ILogger<AuthController> authLogger, ILogger<TokenController> tokenLogger) => _container = ContainerFactory.CreateProcessorsContainer(context, authLogger, tokenLogger);

        private IContainer _container { get; }

        public IToken_Generate token_Generate { get => _container.Resolve<IToken_Generate>(); }
        public IToken_Verify token_Verify { get => _container.Resolve<IToken_Verify>(); }

        public IUser_Login user_Login { get => _container.Resolve<IUser_Login>(); }
        public IUser_Register user_Register { get => _container.Resolve<IUser_Register>(); }
    }
}
