using LanguageExt.Common;

namespace TimeManager.IdP.Processors.TokenProcessor
{
    public interface IToken_Verify
    {
        public Task<Result<bool>> Execute(string token);
    }
}
