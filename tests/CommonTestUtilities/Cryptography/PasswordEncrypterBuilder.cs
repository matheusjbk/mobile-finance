using MobileFinance.Domain.Security.Cryptography;
using MobileFinance.Infra.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;
public class PasswordEncrypterBuilder
{
    public static IPasswordEncrypter Build() => new BCryptNet();
}
