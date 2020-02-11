using System;
using NUnit.Framework;
using NSWallet.Shared;
using System.IO;
using System.IO.Compression;

namespace NSWallet.UnitTests
{
	[TestFixture]
	public class BackupFixture
	{
		[Test]
		public void CheckDateGetter()
		{
			var defaultDate = default(DateTime);

			var goodBackup = "nswb-20171203-113108-auto.zip";
			var goodResult = Common.GetDateFromBackupFileName(goodBackup);
			var goodExpected = new DateTime(2017, 12, 3, 11, 31, 8);

			var badBackup_1 = "some text";
			var badResult_1 = Common.GetDateFromBackupFileName(badBackup_1);

			string badBackup_2 = null;
			var badResult_2 = Common.GetDateFromBackupFileName(badBackup_2);

			var badBackup_3 = "path/a/b/nswb-20171203-113108-auto.zip";
			var badResult_3 = Common.GetDateFromBackupFileName(badBackup_3);

			Assert.AreEqual(goodExpected, goodResult);
			Assert.AreEqual(defaultDate, badResult_1);
			Assert.AreEqual(defaultDate, badResult_2);
			Assert.AreEqual(defaultDate, badResult_3);
		}

		[Test]
		public void CheckFutureVersionOfDb()
		{
			var tempFileName = TestResources.WriteResFileToTempFolder("Backups.nswallet_from_future.dat");

			bool isDbOk = BL.CheckDBVersion(tempFileName);

			Assert.IsFalse(isDbOk, "Backup with DB version higher than current was accepted by app, huge mistake");

			File.Delete(tempFileName); // Clean Up
		}

		[Test]
		public void CheckOldVersionOfDb()
		{
			var tempFileName = TestResources.WriteResFileToTempFolder("Backups.nswallet_old.dat");

			bool isDbOk = BL.CheckDBVersion(tempFileName);

			Assert.IsTrue(isDbOk, "Lower backup version should fit, but app declined it!");

			File.Delete(tempFileName); // Clean Up
		}

		[Test]
		public void CheckPointDBVersion() {
			var tempFileName = TestResources.WriteResFileToTempFolder("Backups.nswallet_from_future.dat");

			var dbVersion = NSWalletDBCheck.GetDBVersion(tempFileName);

			Assert.AreEqual("999", dbVersion, "Version DB is not retrieved correctly");

			File.Delete(tempFileName); // Clean Up
		}

	}
}