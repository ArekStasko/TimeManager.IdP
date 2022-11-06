using TimeManager.IdP.Data;
using TimeManager.IdP.Data.Response;
using TimeManager.IdP.Data.Token;

namespace TimeManager.IdP.Processors.UserProcessor
{
    public interface IUser_Login
    {
        public Response<TokenDTO> Login(UserDTO data);
    }
}
