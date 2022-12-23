using LanguageExt.Common;
using TimeManager.IdP.Data;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Processors.UserProcessor
{
    public interface IUser_Register
    {
        public Task<Result<TokenDTO>> Execute(UserDTO data);
    }
}
