using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using TimeManager.IdP.Data;
using TimeManager.IdP.Data.Token;
using TimeManager.IdP.Processors.TokenProcessor;
using Moq;
using TimeManager.IdP.Tests.Data;
using TimeManager.IdP.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace TimeManager.IdP.Tests
{
    [TestClass]
    public class TokenProcessorTests
    {
        private Mock<DbSet<TokenKey>> TestTokenKey { get {

                var data = new List<TokenKey>()
                {
                    new TokenKey() { Id = 1, Key = "f4de095c29e11c281264cfdf3c6404e78c69fa28084cefd1e0af78ee54eeb0a8" }
                }.AsQueryable();

                var mockSet = new Mock<DbSet<TokenKey>>();

                mockSet.As<IQueryable<TokenKey>>().Setup(t => t.Provider).Returns(data.Provider);
                mockSet.As<IQueryable<TokenKey>>().Setup(t => t.Expression).Returns(data.Expression);
                mockSet.As<IQueryable<TokenKey>>().Setup(t => t.ElementType).Returns(data.ElementType);
                mockSet.As<IQueryable<TokenKey>>().Setup(t => t.GetEnumerator()).Returns(data.GetEnumerator());

                return mockSet;
            } }


        [TestMethod]
        public void TokenGenerate_Should_GenerateToken()
        {
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(tkk => tkk.TokenKey).Returns(TestTokenKey.Object);

            var user = new User()
            {
                Id = 1,
                UserName = "Username",
                PasswordHash = Encoding.ASCII.GetBytes("PasswordHash"),
                PasswordSalt = Encoding.ASCII.GetBytes("PasswordSalt"),
                Token = "token"
            };

            var service = new Token_Generate(mockContext.Object, new MockLogger<AuthController>());
            string token = service.Execute(user);
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void TokenVerify_Should_VerifyToken()
        {
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(tkk => tkk.TokenKey).Returns(TestTokenKey.Object);

            string exampleFalseToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VybmFtZSI6InN0cmluZyIsImV4cCI6MTY3MjE3NDY0M30.KXxZWbrT_oBnowUYgOrKaY_2l3dYLSJWLL_EfkPdk5Y";


            var service = new Token_Verify(mockContext.Object, new MockLogger<TokenController>());
            var result = service.Execute(exampleFalseToken);
            Assert.IsNotNull(result);
            _ = result.Result.Match<bool>(succ =>
            {
                Assert.IsFalse(succ);
                return true;
            }, exception =>
            {
                Assert.IsTrue(exception.GetType().Equals(typeof(SecurityTokenExpiredException)));
                return false;
            });
        }
    }
}