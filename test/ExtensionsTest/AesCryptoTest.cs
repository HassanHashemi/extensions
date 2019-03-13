using System.Text;
using Tebyan.Extensions.Cryphtography;
using Xunit;

namespace ExtensionsTest
{
    public class AesCryptoTest
    {
        [Fact]
        public void Should_Entrypt_Decrypt()
        {
            var person = "HassanHashemi";
            var salt = Encoding.UTF8.GetBytes("HassanHashemi@yahoo.com");
            var aes = new AesCrypto(salt, "pass");
            var encrypted = aes.Encrypt(person);
            var result = aes.Decrypt(encrypted);

            Assert.True(person == result);
        }
    }
}
