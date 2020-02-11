using System;
using System.Security.Cryptography;
using System.Text;

namespace NSWallet.Shared
{
    public static partial class Security
    {
        public static string EncryptRSA(string input, string publicKeyXml)
        {
            var inputBLOB = Encoding.UTF8.GetBytes(input);
            using(var rsa = new RSACryptoServiceProvider(4096))
            {
                try
                {
                    rsa.FromXmlString(publicKeyXml);
                    var encryptedData = rsa.Encrypt(inputBLOB, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    rsa.PersistKeyInCsp = false;
                    return base64Encrypted;
                }
                catch(Exception ex)
                {
					log(ex.Message, nameof(EncryptRSA));
					return null;
                }
            }
        }
    }
}