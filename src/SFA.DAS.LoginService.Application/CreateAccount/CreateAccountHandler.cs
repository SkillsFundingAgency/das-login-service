using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountRequest, CreateAccountResponse>
    {
        private readonly LoginContext _loginContext;
        private readonly IUserService _userService;

        public CreateAccountHandler(LoginContext loginContext, IUserService userService)
        {
            _loginContext = loginContext;
            _userService = userService;
        }

        public async Task<CreateAccountResponse> Handle(CreateAccountRequest request, CancellationToken cancellationToken)
        {
            if(await _userService.FindByUsername(request.Username) != null)
            {
                _loginContext.UserLogs.Add(new UserLog()
                {
                    Id = GuidGenerator.NewGuid(),
                    Action = "Create account",
                    Email = request.Username,
                    ExtraData = null,
                    DateTime = SystemTime.UtcNow(),
                    Result = "Email is already used by existing account"
                });
                await _loginContext.SaveChangesAsync(cancellationToken);

                return new CreateAccountResponse()
                {
                    CreateAccountResult = CreateAccountResult.UsernameAlreadyTaken
                };
            }

            var identityResult = await _userService.ValidatePassword(request.Password);
            if (identityResult != IdentityResult.Success)
            {
                var identityError = identityResult.Errors.Any() ? identityResult.Errors.First().Description : "Password does not meet validity rules";

                _loginContext.UserLogs.Add(new UserLog()
                {
                    Id = GuidGenerator.NewGuid(),
                    Action = "Create account",
                    Email = request.Username,
                    ExtraData = null,
                    DateTime = SystemTime.UtcNow(),
                    Result = identityError
                });
                await _loginContext.SaveChangesAsync(cancellationToken);

                return new CreateAccountResponse()
                {
                    CreateAccountResult = CreateAccountResult.PasswordComplexityInvalid,
                    Message = identityError
                };
            }

            var newUserResponse = await _userService.CreateUser(new LoginUser()
            {
                UserName = request.Username,
                Email = request.Username,
                EmailConfirmed = false,
                GivenName = request.GivenName,
                FamilyName = request.FamilyName
            }, request.Password);

            if (newUserResponse.Result != IdentityResult.Success)
            {
                var identityError = newUserResponse.Result.Errors.Any() ? newUserResponse.Result.Errors.First().Description : "The account could not be created at this time";
                return new CreateAccountResponse()
                {
                    CreateAccountResult = CreateAccountResult.UnableToCreateAccount,
                    Message = identityError
                };
            }

            _loginContext.UserLogs.Add(new UserLog()
            {
                Id = GuidGenerator.NewGuid(),
                Action = "Create account",
                Email = newUserResponse.User.Email,
                Result = "User account created",
                DateTime = SystemTime.UtcNow()
            });

            await _loginContext.SaveChangesAsync(cancellationToken);

            return new CreateAccountResponse()
            {
                CreateAccountResult = CreateAccountResult.Successful,
            };
        }
    }
}