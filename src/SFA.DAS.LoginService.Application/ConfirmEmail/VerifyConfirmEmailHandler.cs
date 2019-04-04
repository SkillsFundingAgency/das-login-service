using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class VerifyConfirmEmailHandler : IRequestHandler<VerifyConfirmEmailRequest, VerifyConfirmEmailResponse>
    {
        private readonly LoginContext _loginContext;
        private readonly IUserService _userService;

        public VerifyConfirmEmailHandler(LoginContext loginContext, IUserService userService)
        {
            _loginContext = loginContext;
            _userService = userService;
        }

        public async Task<VerifyConfirmEmailResponse> Handle(VerifyConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var confirmEmailRequest = await _loginContext.ConfirmEmailRequests.SingleOrDefaultAsync(r =>
                r.IdentityToken == request.IdentityToken, cancellationToken);

            if (confirmEmailRequest != null)
            {
                if (confirmEmailRequest.ValidUntil <= SystemTime.UtcNow())
                {
                    _loginContext.UserLogs.Add(new UserLog()
                    {
                        Id = GuidGenerator.NewGuid(),
                        Action = "Verify confirm email link",
                        Email = confirmEmailRequest.Email,
                        ExtraData = confirmEmailRequest.IdentityToken,
                        DateTime = SystemTime.UtcNow(),
                        Result = "Email cannot be verified by expired token"
                    });
                    await _loginContext.SaveChangesAsync(cancellationToken);

                    return new VerifyConfirmEmailResponse
                    {
                        Email = confirmEmailRequest.Email,
                        VerifyConfirmedEmailResult = VerifyConfirmedEmailResult.TokenExpired
                    };
                }
                else if (confirmEmailRequest.IsComplete == true)
                {
                    _loginContext.UserLogs.Add(new UserLog()
                    {
                        Id = GuidGenerator.NewGuid(),
                        Action = "Verify confirm email link",
                        Email = confirmEmailRequest.Email,
                        ExtraData = confirmEmailRequest.IdentityToken,
                        DateTime = SystemTime.UtcNow(),
                        Result = "Email already successfully confirmed by token"
                    });
                    await _loginContext.SaveChangesAsync(cancellationToken);

                    return new VerifyConfirmEmailResponse
                    {
                        Email = confirmEmailRequest.Email,
                        VerifyConfirmedEmailResult = VerifyConfirmedEmailResult.TokenPreviouslyVerified
                    };
                }
                else
                {
                    var loginUser = await _userService.FindByEmail(confirmEmailRequest.Email);
                    if (loginUser != null)
                    {
                        // verify that the token is valid for the user and when valid update the user to email confirmed
                        if (await _userService.VerifyConfirmEmailToken(loginUser, confirmEmailRequest.IdentityToken) == IdentityResult.Success)
                        {
                            // the token verifed the user; the request is complete and cannot be used again
                            confirmEmailRequest.IsComplete = true;

                            _loginContext.UserLogs.Add(new UserLog()
                            {
                                Id = GuidGenerator.NewGuid(),
                                Action = "Verify confirm email link",
                                Email = confirmEmailRequest.Email,
                                ExtraData = confirmEmailRequest.IdentityToken,
                                DateTime = SystemTime.UtcNow(),
                                Result = "Email confirmed by token"
                            });
                            await _loginContext.SaveChangesAsync(cancellationToken);

                            return new VerifyConfirmEmailResponse
                            {
                                Email = confirmEmailRequest.Email,
                                VerifyConfirmedEmailResult = VerifyConfirmedEmailResult.TokenVerified
                            };
                        }
                        else
                        {
                            _loginContext.UserLogs.Add(new UserLog()
                            {
                                Id = GuidGenerator.NewGuid(),
                                Action = "Verify confirm email link",
                                Email = confirmEmailRequest.Email,
                                ExtraData = confirmEmailRequest.IdentityToken,
                                DateTime = SystemTime.UtcNow(),
                                Result = "Email cannot be confirmed by token"
                            });
                            await _loginContext.SaveChangesAsync(cancellationToken);

                            return new VerifyConfirmEmailResponse
                            {
                                Email = confirmEmailRequest.Email,
                                VerifyConfirmedEmailResult = VerifyConfirmedEmailResult.TokenInvalid
                            };
                        }
                    }
                }
            }

            _loginContext.UserLogs.Add(new UserLog()
            {
                Id = GuidGenerator.NewGuid(),
                Action = "Verify confirm email link",
                Email = string.Empty,
                ExtraData = request.IdentityToken,
                DateTime = SystemTime.UtcNow(),
                Result = "Token does not exists"
            });
            await _loginContext.SaveChangesAsync(cancellationToken);

            return new VerifyConfirmEmailResponse
            {
                VerifyConfirmedEmailResult = VerifyConfirmedEmailResult.TokenInvalid
            };

        }
    }
}