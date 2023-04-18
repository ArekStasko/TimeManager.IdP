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
using TimeManager.IdP.Processors.UserProcessor;

namespace TimeManager.IdP.Tests
{
    [TestClass]
    public class UserProcessorTests
    {
        private Mock<DbSet<TokenKey>> TestTokenKey
        {
            get
            {

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
            }
        }
        private Mock<DbSet<User>> UserDbSet
        {
            get
            {

                var data = new List<User>()
                {
                    new User()
                    {
                        Id = 1,
                        UserName = "Username",
                        PasswordHash = Encoding.ASCII.GetBytes("PasswordHash"),
                        PasswordSalt = Encoding.ASCII.GetBytes(""),
                        Token = "token"
                    }
                }.AsQueryable();

                var mockSet = new Mock<DbSet<User>>();

                mockSet.As<IQueryable<User>>().Setup(t => t.Provider).Returns(data.Provider);
                mockSet.As<IQueryable<User>>().Setup(t => t.Expression).Returns(data.Expression);
                mockSet.As<IQueryable<User>>().Setup(t => t.ElementType).Returns(data.ElementType);
                mockSet.As<IQueryable<User>>().Setup(t => t.GetEnumerator()).Returns(data.GetEnumerator());

                return mockSet;
            }
        }

        [TestMethod]
        public void UserRegister_Should_RegisterUser()
        {
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(ctx => ctx.TokenKey).Returns(TestTokenKey.Object);
            mockContext.Setup(ctx => ctx.Users).Returns(UserDbSet.Object);

            var userDTO = new UserDTO()
            {
                Password = "PasswordHash",
                UserName = "Username2"
            };

            var service = new User_Register(mockContext.Object, new MockLogger<AuthController>());
            var result = service.Execute(userDTO);

            Assert.IsNotNull(result);
            _ = result.Result.Match<bool>(succ =>
            {
                mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
                return true;
            }, exception =>
            {
                Assert.Fail(exception.Message);
                return false;
            });
        }

        [TestMethod]
        public void UserLogin_Should_LoginUser()
        {
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(ctx => ctx.TokenKey).Returns(TestTokenKey.Object);
            mockContext.Setup(ctx => ctx.Users).Returns(UserDbSet.Object);

            var userDTO = new UserDTO()
            {
                Password = "PasswordHash",
                UserName = "Username2"
            };

            var service = new User_Login(mockContext.Object, new MockLogger<AuthController>());
            var result = service.Execute(userDTO);
            
            Assert.IsNotNull(result);
            _ = result.Result.Match<bool>(succ =>
            {
                mockContext.Verify(ctx => ctx.SaveChanges(), Times.Once);
                return true;
            }, exception =>
            {
                Assert.Fail(exception.Message);
                return false;
            });
        }

    }
}