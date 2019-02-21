using SFA.DAS.LoginService.Application.Interfaces;

namespace SFA.DAS.LoginService.Application.Services
{
    public class CodeGenerationService : ICodeGenerationService
    {
        public string GenerateCode()
        {
            var pwdGen = new PasswordGenerator(new PasswordGeneratorSettings(false, true, true, false, 8, 1, false));

            return pwdGen.Next();
        }
    }
}