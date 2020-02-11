using NUnit.Framework;
using NSWallet.Shared;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace NSWallet.UnitTests
{
    [TestFixture]
    public class ResourcesFixture
    {
        const string defaultLang = "en";
		readonly string[] files2check = { 
			"notpremium.html", 
			"old_premium.html", 
			"help_import_backup.html",
			"privacy_policy.html",
			"terms_of_use.html",
			"feedback_description.html"
		};

        [Test]
        public void CheckNSWalletIcon()
        {
            NSWRes.Init(GetType().Namespace);
            var stream = NSWRes.GetImage("Icons.app_icon_1024.png");
            Assert.IsNotNull(stream);
        }

        [Test]
        public void CheckLocalizationHTMLFiles()
        {
            var langs = Lang.availableLangs();

            Assert.NotNull(langs, "List of languages is not detected");
            var assembly = typeof(Lang).GetTypeInfo().Assembly;
            Assert.NotNull(assembly, "Assembly of Lang class is not detected!!");
            var curNamespace = GetType().Namespace;
            Assert.NotNull(curNamespace, "Current namespace is not detected!!");
            var locFilePath = curNamespace + GConsts.EMBEDDED_LANG_FILES_PATH;

            Assert.Multiple(() =>
            {
                foreach (var lang in langs)
                {
                    if (lang.LangCode == Lang.LANG_CODE_SYSTEM) continue;

                    foreach (var filename in files2check)
                    {
                        var locFile = locFilePath + lang.LangCode + "." + filename;
                        var stream = assembly.GetManifestResourceStream(locFile);
                        Assert.NotNull(stream, "Localization file is not found:" + locFile);
                    }
                }

            });
        }

        [Test]
        public void CheckLocalizationFiles()
        {
            var langs = Lang.availableLangs();

            Assert.NotNull(langs, "List of languages is not detected");

            var assembly = typeof(Lang).GetTypeInfo().Assembly;
            Assert.NotNull(assembly, "Assembly of Lang class is not detected!!");
            var curNamespace = GetType().Namespace;
            Assert.NotNull(curNamespace, "Current namespace is not detected!!");
            var locFilePath = curNamespace + GConsts.EMBEDDED_LANG_PATH;

            Assert.Multiple(() =>
            {
                foreach (var lang in langs)
                {
                    if (lang.LangCode == Lang.LANG_CODE_SYSTEM) continue;
                    var locFile = locFilePath + lang.LangCode + ".json";
                    var stream = assembly.GetManifestResourceStream(locFile);
                    Assert.NotNull(stream, "Localization file is not found: " + locFile);
                    string JSONfile = "";
                    try
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            JSONfile = reader.ReadToEnd();
                        }

                        JsonConvert.DeserializeObject<Dictionary<string, string>>(JSONfile);
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail("JSON lang file was not read or deserializaed, file name: " + locFile + ", error: " + ex.Message);
                    }
                }

            });

        }
    }
}
