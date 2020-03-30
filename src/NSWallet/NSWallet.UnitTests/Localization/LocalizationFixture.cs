using NUnit.Framework;
using NSWallet.Shared;

namespace NSWallet.UnitTests
{
    [TestFixture]
    public partial class LocalizationFixture
    {
        const string _namespace = "NSWallet";

        [Test]
        public void SetLanguage()
        {
            TR.InitTR(_namespace);
            TR.SetLanguage(Lang.LANG_CODE_RU);
            Assert.AreEqual(Lang.LANG_CODE_RU, TR.Language);
        }

        [Test]
        public void SetNonExistingLanguage()
        {
            TR.InitTR(_namespace);
            TR.SetLanguage("ww"); // Trying to set non existing language, has to be switched to English
            Assert.AreEqual(Lang.LANG_CODE_EN, TR.Language);
        }

        [Test]
        public void CheckIfAvailableAreSupported()
        {
            TR.InitTR(_namespace);
            var langs = Lang.availableLangs();
            foreach (Lang lng in langs)
            {
                Assert.IsTrue(Lang.checkIfSupported(lng.LangCode));
            }
        }

        [Test]
        public void CheckNames()
        {
            TR.InitTR(_namespace);
            var langs = Lang.availableLangs();
            foreach (Lang lng in langs)
            {
                Assert.IsTrue(Lang.checkIfSupported(lng.LangCode));
            }
        }

        [Test]
        public void CheckTranslations()
        {
            TR.InitTR(_namespace);
            var enDict = TR.GetEnDictionary();
            Assert.Multiple(() =>
            {
                foreach (var lang in Lang.availableLangs())
                {
                    if (lang.LangCode == "en" || lang.LangCode == "--")
                        continue;
                    TR.SetLanguage(lang.LangCode);

                    foreach (var translation in enDict)
                    {
                        Assert.IsNotEmpty(TR.Tr(translation.Key));
                        Assert.IsNotNull(TR.Tr(translation.Key));

                        if (IsTranslationException(lang.LangCode, translation.Key)) continue;

                        Assert.AreNotEqual(translation.Value, TR.Tr(translation.Key), "Lang: " + lang.LangCode + ", key: " + translation.Key);
                    }
                }
            });

        }

    }
}
