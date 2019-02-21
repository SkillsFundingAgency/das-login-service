namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IHashingService
    {
        string GetHash(string plainText);
    }
}