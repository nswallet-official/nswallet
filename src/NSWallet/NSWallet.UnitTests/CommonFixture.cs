using NUnit.Framework;
using NSWallet.Shared;
using System;

namespace NSWallet.UnitTests
{
	[TestFixture]
	public class CommonFixture
	{
		[Test]
		public void CheckDateToDBConversion()
		{
			var dt = new DateTime(2016, 12, 15, 17, 23, 54);
            var covertedValue = Shared.Common.ConvertDateTimeDB(dt);
			Assert.AreEqual("2016-12-15 17:23:54", covertedValue);
		}

		[Test]
		public void CheckDBDateTimeConversion()
		{
			var dbValue = "2016-12-15 17:23:54";
			var dtExpected = new DateTime(2016, 12, 15, 17, 23, 54);
			var covertedValue = Shared.Common.ConvertDBDateTime(dbValue);
			Assert.AreEqual(dtExpected, covertedValue);
		}
	}
}
