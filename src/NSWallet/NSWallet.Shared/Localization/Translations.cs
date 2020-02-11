using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using NSWallet.Shared.Helpers.Logs.AppLog;

namespace NSWallet.Shared
{
    // ---------------------------------------------------
    // Localization usage
    // In the very beginning of the app always call
    // ----
    // TR.InitTR(GetType().Namespace);
    // ----
    // GetType().Namespace  will get the namaspace of current executable
    // English will be set as initial language 
    // ---------------------------------------------------
    // If you want to change the language, use
    // TR.SetLanguage(two_letters_language_code_here);

    public partial class Lang
    {
        public const string LANG_CODE_SYSTEM = "--";
        public const string LANG_NAME_SYSTEM = "System";

        public string LangCode;
        public string LanguageEnglish;
        public string LanguageLocal;
        public string LanguageCombined;
        static List<Lang> _listLangs;

        static Lang()
        {
            // We can read this list from some dynamic sources in the future
            updateLangs();
        }

        public Lang(string code, string english, string local = null)
        {

            LangCode = code;
            LanguageEnglish = english;
            LanguageLocal = local;
            if (string.IsNullOrEmpty(local))
            {
                LanguageCombined = LanguageEnglish;
            }
            else
            {
                if (string.Compare(LanguageLocal, LanguageEnglish) != 0)
                    LanguageCombined = LanguageLocal + " (" + LanguageEnglish + ")";
                else
                    LanguageCombined = LanguageEnglish;
            }
            if (LanguageLocal == null)
            {
                LanguageLocal = LanguageEnglish;
            }
        }

        static void updateLangs()
        {
            _listLangs = new List<Lang>();
            _listLangs.Add(new Lang(LANG_CODE_SYSTEM, LANG_NAME_SYSTEM)); //, LANG_NAME_CODE));
            _listLangs.Add(new Lang(LANG_CODE_EN, LANG_NAME_EN));

			SupportedLangs();
        }



        public static List<Lang> availableLangs()
        {
            updateLangs();
            return _listLangs;
        }

        public static bool checkIfSupported(string codeLang)
        {
            foreach (Lang lang in _listLangs)
            {
                if (codeLang == lang.LangCode)
                {
                    return true;
                }
            }
            return false;
        }

        public static Lang getLangByCode(string langCode)
        {
            updateLangs();
            Lang currentLang = null;
            foreach (Lang lang in _listLangs)
            {
                if (langCode == lang.LangCode)
                {
                    currentLang = lang;
                }
            }
            return currentLang;
        }


    }

    public class LocalizationException : Exception
    {
        public LocalizationException(string message) : base(message)
        {

        }
    }

    // Translation class, short name TR given to decrease amount of the code connected with Localization
    public  static partial class TR
    {
        static Dictionary<string, string> dictionary;
        static Dictionary<string, string> dictionaryEN;

        static string language;
        static string RunNameSpace;

        public static string Language
        {
            get
            {
                return language;
            }
        }

        public static string LanguageHumanReadable
        {
            get
            {
                return Lang.getLangByCode(language).LanguageLocal;
            }
        }

        public static void InitTR(string rNameSpace)
        {
            RunNameSpace = rNameSpace;
            dictionary = new Dictionary<string, string>();
            dictionaryEN = new Dictionary<string, string>();

            LoadEnglish(); // Always load English as base, values will be taken from here if not found for other langs
            SetLanguage(Lang.LANG_CODE_EN);

        }

        public static Dictionary<string, string> GetEnDictionary()
        {
            if (string.IsNullOrEmpty(RunNameSpace))
            {
                throw new LocalizationException("Namespace is not set");
            }
            if (dictionaryEN == null)
            {
                throw new LocalizationException("English dictionary is empty");
            }

            return dictionaryEN;
        }

        static void LoadEnglish()
        {
            if (string.IsNullOrEmpty(RunNameSpace))
            {
                throw new LocalizationException("Namespace is not set");
            }

            var assembly = typeof(TR).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream(RunNameSpace + GConsts.EMBEDDED_LANG_PATH + "en.json");

            string enJSONfile = "";
            using (var reader = new StreamReader(stream))
            {
                enJSONfile = reader.ReadToEnd();
            }
            dictionaryEN = JsonConvert.DeserializeObject<Dictionary<string, string>>(enJSONfile);

        }

        static void SetCultureInfo(string langCode)
        {
            CultureInfo culture = null;

			try {
				switch (langCode) {
					case Lang.LANG_CODE_BE: culture = new CultureInfo("be-BY"); break;
					case Lang.LANG_CODE_BG: culture = new CultureInfo("bg-BG"); break;
					case Lang.LANG_CODE_CA: culture = new CultureInfo("ca-ES"); break;
					case Lang.LANG_CODE_DE: culture = new CultureInfo("de-DE"); break;
					case Lang.LANG_CODE_ES: culture = new CultureInfo("es-ES"); break;
					case Lang.LANG_CODE_HI: culture = new CultureInfo("hi-IN"); break;
					case Lang.LANG_CODE_PT: culture = new CultureInfo("pt-PT"); break;
					case Lang.LANG_CODE_RU: culture = new CultureInfo("ru-RU"); break;
					case Lang.LANG_CODE_UK: culture = new CultureInfo("uk-UA"); break;
					default: culture = new CultureInfo("en-US"); break;
				}
			} catch (Exception ex) {
				log(ex.Message, nameof(SetCultureInfo));
				try {
					culture = new CultureInfo(langCode);
				} catch (Exception exception) {
					log(exception.Message, nameof(SetCultureInfo));
					culture = new CultureInfo("en-US");
				}
			}

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        public static void SetLanguage(string langCode)
        {
            if (string.IsNullOrEmpty(RunNameSpace))
            {
                throw new LocalizationException("Namespace is not set");
            }

            if (Lang.checkIfSupported(langCode) == false)
            {
                SetLanguage(Lang.LANG_CODE_EN);
                return;
            }

            try
            {

                language = langCode;

                SetCultureInfo(language);

                dictionary.Clear();

                var assembly = typeof(Lang).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream(RunNameSpace + GConsts.EMBEDDED_LANG_PATH + language + ".json");
                string JSONfile = "";
                using (var reader = new StreamReader(stream))
                {
                    JSONfile = reader.ReadToEnd();
                }
                dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSONfile);
            }
            catch(Exception ex)
            {
				log(ex.Message, nameof(SetLanguage));
				SetLanguage(Lang.LANG_CODE_EN);
                SetCultureInfo(Lang.LANG_CODE_EN);
            }

        }

        public static string TrEn(string tag)
        {
            if (string.IsNullOrEmpty(RunNameSpace))
            {
                throw new LocalizationException("Namespace is not set");
            }

            string translation = string.Empty;
            try
            {
                translation = dictionaryEN[tag];
            }
            catch (KeyNotFoundException ex)
            {
				log(ex.Message, nameof(TrEn));
                translation = string.Empty;
            }
            return translation;
        }

        public static string Tr(string tag)
        {
            if (string.IsNullOrEmpty(RunNameSpace))
            {
                throw new LocalizationException("Namespace is not set");
            }
            string translation;

            try
            {
                translation = dictionary[tag];
            }
            catch (KeyNotFoundException)
            {
                try
                {
                    translation = dictionaryEN[tag];
                }
                catch (KeyNotFoundException ex)
                {
					log(ex.Message, nameof(Tr));
					translation = tag;
                }
            }
            return translation;
        }

		static void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(TR));
		}
    }
}

