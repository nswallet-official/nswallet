using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NSWallet.Shared
{
	// FIXME: Remove this method as soon as possible
	public  static partial class Security
	{
		public static string DecryptStringAES_IOS(byte[] encryptedData, string password, int reEncryptionCount, string hash, out bool ok)
		{
			ok = false;

			var key = prepareKey(password, hash, reEncryptionCount);
			byte[] decryptedData;
			key[0] = 0;

			try {
				using (MemoryStream ms = new MemoryStream()) {
					using (AesManaged myAes = new AesManaged()) {
						myAes.Mode = CipherMode.CBC;
						myAes.Padding = PaddingMode.PKCS7;
						myAes.Key = key;
						myAes.IV = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


						using (var cs = new CryptoStream(ms, myAes.CreateDecryptor(), CryptoStreamMode.Write)) {
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
				if (md5 == CalcMD5(fullStr)) {
					ok = true;
					return fullStr;
				}
			} catch (Exception ex) {
				log(ex.Message, nameof(DecryptStringAES));
				ok = false;
			}

			return "";
		}
	}
}
