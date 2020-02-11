using NUnit.Framework;
using NSWallet.Shared;
using System;

namespace NSWallet.UnitTests
{
	[TestFixture]
	public class CommonFixture
	{
		[Test]

		public void CheckRemovingTags()
		{
			string str2test = "something here[iosonly]<ul><li> Payment will be charged[/iosonly] bla-blah test";
			string result = "something here<ul><li> Payment will be charged bla-blah test";
			string processedText = Common.RemoveTags(str2test, "[iosonly]", "[/iosonly]");
			Assert.AreEqual(result, processedText);
		}

		[Test]
		public void CheckRemovingTextBetweenTags()
		{
			string str2test = "something here[iosonly]<ul><li> Payment will be charged[/iosonly] bla-blah test";
			string result = "something here bla-blah test";
			string processedText = Common.RemoveTextBetweenTags(str2test, "[iosonly]", "[/iosonly]");
			Assert.AreEqual(result, processedText);
		}

		[Test]
		public void CheckDateToDBConversion()
		{
			var dt = new DateTime(2016, 12, 15, 17, 23, 54);
            var covertedValue = Common.ConvertDateTimeDB(dt);
			Assert.AreEqual("2016-12-15 17:23:54", covertedValue);
		}

		[Test]
		public void CheckDBDateTimeConversion()
		{
			var dbValue = "2016-12-15 17:23:54";
			var dtExpected = new DateTime(2016, 12, 15, 17, 23, 54);
			var covertedValue = Common.ConvertDBDateTime(dbValue);
			Assert.AreEqual(dtExpected, covertedValue);
		}
	}
}
