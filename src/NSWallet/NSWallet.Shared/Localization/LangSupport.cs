namespace NSWallet.Shared
{
	public partial class Lang
	{
		public const string LANG_CODE_EN = "en";
		public const string LANG_NAME_EN = "English";

		public const string LANG_CODE_RU = "ru";
		public const string LANG_NAME_RU = "Russian";
		public const string LANG_NAME_RU_LOCAL = "Русский";

		public const string LANG_CODE_DE = "de";
		public const string LANG_NAME_DE = "German";
		public const string LANG_NAME_DE_LOCAL = "Deutsch";

		public const string LANG_CODE_ES = "es";
		public const string LANG_NAME_ES = "Spanish";
		public const string LANG_NAME_ES_LOCAL = "Español";

		public const string LANG_CODE_PT = "pt";
		public const string LANG_NAME_PT = "Portuguese";
		public const string LANG_NAME_PT_LOCAL = "Portugues";

		public const string LANG_CODE_UK = "uk";
		public const string LANG_NAME_UK = "Ukrainian";
		public const string LANG_NAME_UK_LOCAL = "Українська";

		public const string LANG_CODE_CA = "ca";
		public const string LANG_NAME_CA = "Catalan";
		public const string LANG_NAME_CA_LOCAL = "Català";

		public const string LANG_CODE_BG = "bg";
		public const string LANG_NAME_BG = "Bulgarian";
		public const string LANG_NAME_BG_LOCAL = "Български";

		public const string LANG_CODE_PL = "pl";
		public const string LANG_NAME_PL = "Polish";
		public const string LANG_NAME_PL_LOCAL = "Polski";

		public const string LANG_CODE_HI = "hi";
		public const string LANG_NAME_HI = "Hindi";
		public const string LANG_NAME_HI_LOCAL = "हिन्दी भाषा";

		public const string LANG_CODE_BE = "be";
		public const string LANG_NAME_BE = "Belarusian";
		public const string LANG_NAME_BE_LOCAL = "Беларуская";

		static void SupportedLangs()
		{
			// Sort them manually by english locale
			_listLangs.Add(new Lang(LANG_CODE_BE, LANG_NAME_BE, LANG_NAME_BE_LOCAL)); // Belarusian
			_listLangs.Add(new Lang(LANG_CODE_BG, LANG_NAME_BG, LANG_NAME_BG_LOCAL)); // Bulgarian
			_listLangs.Add(new Lang(LANG_CODE_CA, LANG_NAME_CA, LANG_NAME_CA_LOCAL)); // Catalan
			_listLangs.Add(new Lang(LANG_CODE_DE, LANG_NAME_DE, LANG_NAME_DE_LOCAL)); // German
			_listLangs.Add(new Lang(LANG_CODE_HI, LANG_NAME_HI, LANG_NAME_HI_LOCAL)); // Hindi
			_listLangs.Add(new Lang(LANG_CODE_PL, LANG_NAME_PL, LANG_NAME_PL_LOCAL)); // Polish
			_listLangs.Add(new Lang(LANG_CODE_PT, LANG_NAME_PT, LANG_NAME_PT_LOCAL)); // Portugese
			_listLangs.Add(new Lang(LANG_CODE_RU, LANG_NAME_RU, LANG_NAME_RU_LOCAL)); // Russian
			_listLangs.Add(new Lang(LANG_CODE_ES, LANG_NAME_ES, LANG_NAME_ES_LOCAL)); // Spanish
			_listLangs.Add(new Lang(LANG_CODE_UK, LANG_NAME_UK, LANG_NAME_UK_LOCAL)); // Ukranian
		}
	}
}
