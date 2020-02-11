using NUnit.Framework;
using NSWallet.Shared;
using NSWallet.UnitTests;
using System;
using System.Linq;

namespace NSWallet.UnitTests
{
	[TestFixture]
	public class SecurityFixture
	{
		// TODO: Create additional test with ReEncryption > 0

		[Test]
		public void TestCaseMD5()
		{
			const string TextASCII2Hash = "Bsegge647TY.%22";
			const string MD5ASCIIExpectedHash = "7ee62f73f9c08a15f0472fa6c4b63361";
			const string TextUTF82Hash = "Проверка UTF8";
			const string MD5UTF8ExpectedHash = "c063c2eb08c2c0005e25e94d351ac44f";

			// Test data from current app
			const string TestItem = "Test Item";
			const string md5str = "e1c47101f7939099b633e61b3514c623";

			string hash01 = Security.CalcMD5(TextASCII2Hash);
			Assert.AreEqual(MD5ASCIIExpectedHash, hash01);

			string hash02 = Security.CalcMD5(TextUTF82Hash);
			Assert.AreEqual(MD5UTF8ExpectedHash, hash02);

			string hash03 = Security.CalcMD5(TestItem);
			Assert.AreEqual(md5str, hash03);
		}

		[Test]
		public void TestCaseEncrypt()
		{
			const string password = "Sun001!";
			const string plainText = "Test Item";
			var expectedBytes = new byte[] { 
				0x03, 0xde, 0xd5, 0x8a, 
				0x00, 0xcf, 0x22, 0x15,
				0x76, 0x6b, 0x57, 0x5d,
				0xbe, 0xdb, 0xf2, 0xd2,
				0x0f, 0x84, 0xec, 0x9b,
				0x68, 0x41, 0x59, 0xb3, 
				0x05, 0x6f, 0x75, 0x45,
				0xe7, 0x1b, 0xe4, 0x9d,
				0x1d, 0xef, 0xa5, 0xb2, 
				0x9d, 0xcd, 0x4a, 0x06, 
				0xa1, 0x18, 0xa8, 0xa6,
				0x91, 0x29, 0x13, 0x00
			};

			bool ok = false;
			var encrBytes = Security.EncryptStringAES(plainText, password, 0, password, out ok);

			Assert.IsTrue(ok, "Encryption returns 'false'");

			Assert.IsNotNull(encrBytes,"Returned byte array is empty");

			Assert.AreEqual(encrBytes, expectedBytes, "Encrypted byte array is wrong");
		}

		[Test]
		public void TestCaseDecrypt()
		{
			const string password = "Sun001!";
			const string expectedString = "nhh86c4uQKXu8rTjp4sL8rr2fxRMmnhWWhan8LiaVb4ZhTdF4RTlX4xcHYjwsfDu";
			var encryptedBytes = new byte[] {
				0x53,0xda,0x92,0xa5,
				0xf5,0x48,0x90,0xc5,
				0xd5,0xb4,0x13,0xbb,
				0xad,0x51,0xc5,0xf6,
				0xfb,0xf2,0xa3,0x0a,
				0x27,0x98,0x7c,0xed,
				0xad,0x9e,0xea,0xed,
				0x08,0xa7,0xd8,0xa1,
				0x42,0x0c,0xe1,0xe4,
				0xf4,0xf5,0x16,0x03,
				0x96,0x55,0x80,0xc6,
				0x88,0x39,0x16,0x85,
				0x2e,0xce,0x48,0xf9,
				0x8d,0x5e,0x6e,0xb4,
				0x53,0x36,0x51,0x2e,
				0x86,0x1f,0xff,0xb6,
				0x89,0xf2,0xbb,0x5b,
				0x49,0x2c,0x7d,0x92,
				0xae,0x23,0xac,0xbb,
				0x3f,0xd2,0x91,0x21,
				0x98,0x20,0x18,0x45,
				0xad,0xa0,0x9a,0x14,
				0x18,0x99,0xec,0x2d,
				0xed,0x29,0xb9,0xd0,
				0x2f,0xed,0xe9,0xe4,
				0xd2,0xf3,0x3d,0x87,
				0x89,0xf5,0xd3,0xda,
				0xb7,0x6d,0xda,0x55
			};
			bool ok = false;
			var decryptedString = Security.DecryptStringAES(encryptedBytes, password, 0, password, out ok);
			Assert.IsTrue(ok, "Decryption returns 'false'");
			Assert.IsNotNull(decryptedString, "decrypted string is null");
			Assert.IsNotEmpty(decryptedString, "Decrypted string is empty");
			Assert.AreEqual(expectedString, decryptedString, "Decrypted string is not what was expected");
		}

		[Test]
		public void TestCaseFullEncryptionCycle()
		{
            var password = CommonUT.RandomString(CommonUT.RandomRange(3,35));
            var plainText = CommonUT.RandomString(CommonUT.RandomRange(1, 555));

			bool ok;
			var encrBytes = Security.EncryptStringAES(plainText, password, 0, password, out ok);
			Assert.IsTrue(ok, "Encryption returns 'false'");

			var decryptedString = Security.DecryptStringAES(encrBytes, password, 0, password, out ok);
			Assert.IsTrue(ok, "Decryption returns 'false'");

			Assert.IsNotNull(decryptedString, "decrypted string is null");
			Assert.IsNotEmpty(decryptedString, "Decrypted string is empty");
			Assert.AreEqual(plainText, decryptedString, "Decrypted string is not what was expected", "Password: " + password);

		}


	}
}
