using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LoginService.Data
{
    public enum CreateAccountResult
    {
        PasswordComplexityInvalid,
        UsernameAlreadyTaken,
        Successful,
        UnableToCreateAccount
    }
}
