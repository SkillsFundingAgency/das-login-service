using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword.ResetPassword
{
    [TestFixture]
    public class ResetPasswordTestBase
    {
        protected LoginContext LoginContext;
        protected Guid RequestId;
        protected Guid ClientId;
        protected IUserService UserService;
        protected ResetPasswordHandler Handler;
        
        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            LoginContext = new LoginContext(dbContextOptions);

            RequestId = Guid.NewGuid();
            ClientId = Guid.NewGuid();
           
            
            UserService = Substitute.For<IUserService>();
            Handler = new ResetPasswordHandler(UserService, LoginContext);
        }
    }
    
    
    [TestFixture]
    public class When_ResetPassword_called_with_invalid_password : ResetPasswordTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest {Id = RequestId, Email = "email@address.com", ClientId = ClientId, IdentityToken = "T0k3n", IsComplete = false, ValidUntil = SystemTime.UtcNow().AddHours(1)});
            LoginContext.Clients.Add(new Client() {Id = ClientId, ServiceDetails = new ServiceDetails() {PostPasswordResetReturnUrl = "http://returnurl"}});
            await LoginContext.SaveChangesAsync();
            UserService.ResetPassword("email@address.com", "Password", "T0k3n").Returns(new UserResponse(){Result = IdentityResult.Failed()});
        }

        [Test]
        public async Task Then_return_issuccessful_is_false()
        {
            var result = await Handler.Handle(new ResetUserPasswordRequest {ClientId = ClientId, RequestId = RequestId, Password = "Password"}, CancellationToken.None);
            result.IsSuccessful.Should().BeFalse();
        }
    }
    
    [TestFixture]
    public class When_ResetPassword_called_for_valid_request : ResetPasswordTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest {Id = RequestId, Email = "email@address.com", ClientId = ClientId, IdentityToken = "T0k3n", IsComplete = false, ValidUntil = SystemTime.UtcNow().AddHours(1)});
            LoginContext.Clients.Add(new Client() {Id = ClientId, ServiceDetails = new ServiceDetails() {PostPasswordResetReturnUrl = "http://returnurl"}});
            await LoginContext.SaveChangesAsync(); 
            UserService.ResetPassword("email@address.com", "Pa55word", "T0k3n").Returns(new UserResponse(){Result = IdentityResult.Success});
        }
        
        [Test]
        public async Task Then_User_has_password_updated()
        {
            await Handler.Handle(new ResetUserPasswordRequest {ClientId = ClientId, RequestId = RequestId, Password = "Pa55word"}, CancellationToken.None);

            await UserService.Received().ResetPassword("email@address.com", "Pa55word", "T0k3n");
        }

        [Test]
        public async Task Then_return_contains_correct_values()
        {
            var result = await Handler.Handle(new ResetUserPasswordRequest {ClientId = ClientId, RequestId = RequestId, Password = "Pa55word"}, CancellationToken.None);
            result.IsSuccessful.Should().BeTrue();
            result.ClientId.Should().Be(ClientId);
            result.ReturnUrl.Should().Be("http://returnurl");
        }
    }
}