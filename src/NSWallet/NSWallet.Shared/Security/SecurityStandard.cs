using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using NSWallet.Shared.Helpers.Logs.AppLog;

namespace NSWallet.Shared
{
    public static partial class Security
    {
        const int KEY_LENGTH = 32; // AES-256

        static byte[] prepareKey(string password, string hash, int reEncryptionCount)
        {
            while (password.Length < KEY_LENGTH)
            {
                password += password;
            }

            var key = password.Substring(0, KEY_LENGTH);

            if (string.IsNullOrEmpty(hash))
            {
                key = GetReEncryptedPassword(key, reEncryptionCount);
            }
            else if (reEncryptionCount > 0)
            {
                key = hash;
            }

            var keyFull = Encoding.UTF8.GetBytes(key);
            byte[] keyFinal = new byte[KEY_LENGTH];
            Array.Copy(keyFull, keyFinal, KEY_LENGTH);

            return keyFinal;
        }

        public static byte[] EncryptStringAES(string plainText, string password, int reEncryptionCount, string hash, out bool ok)
        {
            ok = false;
            byte[] bytes;

            try
            {
                var key = prepareKey(password, hash, reEncryptionCount);

                var md5str = CalcMD5(plainText);
                var fullStr = md5str + plainText;
                var data = Encoding.UTF8.GetBytes(fullStr);

                using (MemoryStream ms = new MemoryStream())
                {
                    AesManaged myAes = new AesManaged();
                    myAes.Mode = CipherMode.CBC;
                    myAes.Padding = PaddingMode.PKCS7;
                    myAes.Key = key;
                    myAes.IV = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    using (var cs = new CryptoStream(ms, myAes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.Close();
                    }
                    bytes = ms.ToArray();
                }
			} catch(Exception ex) {
				log(ex.Message, nameof(EncryptStringAES));
                return null;
            }


			ok = true;
            return bytes;
        }

        public static string DecryptStringAES(byte[] encryptedData, string password, int reEncryptionCount, string hash, out bool ok)
        {
            ok = false;

            var key = prepareKey(password, hash, reEncryptionCount);
            byte[] decryptedData;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (AesManaged myAes = new AesManaged())
                    {
                        myAes.Mode = CipherMode.CBC;
                        myAes.Padding = PaddingMode.PKCS7;
                        myAes.Key = key;
                        myAes.IV = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


                        using (var cs = new CryptoStream(ms, myAes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(encryptedData, 0, encryptedData.Length);
                            cs.Close();
                        }
                        decryptedData = ms.ToArray();
                    }
                }

                /*
                ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
                ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
                var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, encryptedData);
                */
                var resultText = Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);

                if (resultText.Length < KEY_LENGTH)
                    return "";

                var md5 = resultText.Substring(0, KEY_LENGTH);
                var fullStr = resultText.Substring(KEY_LENGTH);
                if (md5 == CalcMD5(fullStr))
                {
                    ok = true;
                    return fullStr;
                }
            }
            catch(Exception ex)
            {
				log(ex.Message, nameof(DecryptStringAES));
                ok = false;
            }

			// FIXME: remove as soon as possible
			// This is workaround created to avoid decryption bug in old iOS app when first byte in non ASCII
			// chars was replaced with /x0
			string fullStr2 = DecryptStringAES_IOS(encryptedData, password, reEncryptionCount, hash, out bool ok2);
			if (ok2) {
				ok = ok2;
				return fullStr2;
			}
			// End of workaround


			return "";
        }

        public static string GetReEncryptedPassword(string password, int reEncryptionCount)
        {
            var retPass = password;
            for (int i = 0; i < reEncryptionCount; i++)
            {
                retPass = CalcMD5(retPass);
            }

            return retPass;
        }

        public static string CalcMD5(string password)
        {
            MD5 md5 = MD5.Create();
            //var hashProvider = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Md5);
            var inputBytes = Encoding.UTF8.GetBytes(password);
            var hash = md5.ComputeHash(inputBytes);
            //var hash = hashProvider.HashData(inputBytes);

            var sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        const string lowerLetters = "qwertyuiopasdfghjklzxcvbnm";
        const string upperLetters = "QWERTYUIOPASDFGHJKLZXCVBNM";
        const string digitsSymbols = "1234567890";
        const string specialSymbols = "!@#$%^&*()_+-={}[];:|,.<>?~";
        const string allSymbols = lowerLetters + upperLetters + digitsSymbols + specialSymbols;

        public static string GeneratePassword(bool lowerCase, bool upperCase, bool digits, bool special, int len)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            string finalSymbols = "";
            if (lowerCase)
                finalSymbols += lowerLetters + lowerLetters+ lowerLetters;

            if (upperCase)
                finalSymbols += upperLetters + upperLetters+ upperLetters;

            if (digits)
                finalSymbols += digitsSymbols + digitsSymbols;

            if (special)
                finalSymbols += specialSymbols;

            var generatedPassword = "";
            for (int i = 0; i < len; i++)
            {
                generatedPassword += finalSymbols.Substring(rand.Next(0, finalSymbols.Length - 1), 1);
            }

            return generatedPassword;

        }

        public static string GenerateCleverPassword(string passwordPattern)
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            var generatedPassword = "";
            for (int i = 0; i < passwordPattern.Length; i++)
            {
                var symb = passwordPattern.Substring(i, 1);
                if (lowerLetters.Contains(symb))
                {
                    generatedPassword += lowerLetters.Substring(rand.Next(0, lowerLetters.Length - 1), 1);
                    continue;
                }
                if (upperLetters.Contains(symb))
                {
                    generatedPassword += upperLetters.Substring(rand.Next(0, upperLetters.Length - 1), 1);
                    continue;
                }
                if (digitsSymbols.Contains(symb))
                {
                    generatedPassword += digitsSymbols.Substring(rand.Next(0, digitsSymbols.Length - 1), 1);
                    continue;
                }
                if (specialSymbols.Contains(symb))
                {
                    generatedPassword += specialSymbols.Substring(rand.Next(0, specialSymbols.Length - 1), 1);
                    continue;
                }
                generatedPassword += allSymbols.Substring(rand.Next(0, allSymbols.Length - 1), 1);
            }
            return generatedPassword;
        }

		static void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(Security) + " (SecurityStandard)");
		}
    }
}