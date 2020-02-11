using NUnit.Framework;
using NSWallet.Shared;

namespace NSWallet.UnitTests
{
	[TestFixture]
	public class SecurityStressFixture
	{
		const int PASSWORD_MAX_LEN = 100;
		const int SHORT_STRING = 100;
		const int LONG_STRING = 1000;
		const int HUGE_STRING = 60000;

		const int NORMAL_ITERATION = 1000;
		const int SHORT_ITERATION = 100;

		[Test]
		public void StressShortStrings()
		{
			

			for (int i = 0; i < NORMAL_ITERATION; i++)
			{
                var password = CommonUT.RandomString(CommonUT.RandomRange(1, PASSWORD_MAX_LEN));
                var plainText = CommonUT.RandomString(CommonUT.RandomRange(1, SHORT_STRING));
                var encrBytes = Security.EncryptStringAES(plainText, password, 0, password, out bool ok);
                Assert.IsTrue(ok, "Encryption returns 'false'");

				var decryptedString = Security.DecryptStringAES(encrBytes, password, 0, password, out ok);
				Assert.IsTrue(ok, "Decryption returns 'false'");

				Assert.IsNotNull(decryptedString, "decrypted string is null");
				Assert.IsNotEmpty(decryptedString, "Decrypted string is empty");
				Assert.AreEqual(plainText, decryptedString, "Decrypted string is not what was expected",
				                "Password: " + password, ", Iteration: " + i);
			}
		}

		[Test]
		public void StressLongStrings()
		{

			for (int i = 0; i < NORMAL_ITERATION; i++)
			{
                var password = CommonUT.RandomString(CommonUT.RandomRange(1, PASSWORD_MAX_LEN));
                var plainText = CommonUT.RandomString(CommonUT.RandomRange(1, LONG_STRING));
                var encrBytes = Security.EncryptStringAES(plainText, password, 0, password, out bool ok);
                Assert.IsTrue(ok, "Encryption returns 'false'");

				var decryptedString = Security.DecryptStringAES(encrBytes, password, 0, password, out ok);
				Assert.IsTrue(ok, "Decryption returns 'false'");

				Assert.IsNotNull(decryptedString, "decrypted string is null");
				Assert.IsNotEmpty(decryptedString, "Decrypted string is empty");
				Assert.AreEqual(plainText, decryptedString, "Decrypted string is not what was expected",
								"Password: " + password, ", Iteration: " + i);
			}
		}

		[Test]
		public void StressHugeStrings()
		{
			

			for (int i = 0; i < SHORT_ITERATION; i++)
			{
                var password = CommonUT.RandomString(CommonUT.RandomRange(1, PASSWORD_MAX_LEN));
                var plainText = CommonUT.RandomString(CommonUT.RandomRange(1, HUGE_STRING));
                var encrBytes = Security.EncryptStringAES(plainText, password, 0, password, out bool ok);
                Assert.IsTrue(ok, "Encryption returns 'false'");

				var decryptedString = Security.DecryptStringAES(encrBytes, password, 0, password, out ok);
				Assert.IsTrue(ok, "Decryption returns 'false'");

				Assert.IsNotNull(decryptedString, "decrypted string is null");
				Assert.IsNotEmpty(decryptedString, "Decrypted string is empty");
				Assert.AreEqual(plainText, decryptedString, "Decrypted string is not what was expected",
								"Password: " + password, ", Iteration: " + i);
			}
		}
	}
}
