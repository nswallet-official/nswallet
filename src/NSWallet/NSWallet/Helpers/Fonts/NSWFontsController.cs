using System.Collections.Generic;
using System.Linq;
using NSWallet.Helpers;
using NSWallet.Shared;
using Xamarin.Forms;

namespace NSWallet.NetStandard.Helpers.Fonts
{
	public static class NSWFontsController
	{
		static List<NSWFont> nswFonts { get; set; }
		static List<string> fontNames { get; set; }
		static string runtimePlatform;

		const string fontExtension = ".ttf";
		const string defaultFontFamily = "default_font_family";
		const string regularAttribute = "-Regular";
		const string lightAttribute = "-Light";
		const string boldAttribute = "-Bold";

		static NSWFontsController()
		{
			initFonts();
			runtimePlatform = Device.RuntimePlatform;
		}

		public static string FontAwesomeRegular
		{
			get {
				if (Device.RuntimePlatform == Device.Android) {
					return "fa-regular-581.otf#Font Awesome 5";
				}
				return "Font Awesome 5 Free";
			}
		}

		public static string FontAwesomeSolid
		{
			get {
				if (Device.RuntimePlatform == Device.Android) {
					return "fa-solid-581.otf#Font Awesome 5";
				}
				return "Font Awesome 5 Free";
			}
		}

		public static string CurrentTypeface {
			get {
				var settingsTypeface = Settings.FontFamily;
				var nswFont = nswFonts.SingleOrDefault(x => x.Typeface == settingsTypeface);
				if (nswFont != null) {
					var typeface = nswFont.Typeface;
					switch (runtimePlatform) {
						case Device.iOS: 
						case Device.macOS:
							return nswFont.Typeface;
						case Device.Android:
							return
								typeface +
								fontExtension +
								"#" +
								typeface;
					}
				}
				return null;
			}
		}

		public static string CurrentBoldTypeface {
			get {
				var typeface = CurrentTypeface;
				if (typeface != null) {
					if (!NSWFontBoldExceptions.List.Exists(typeface.Contains)) {
						if (typeface.Contains(regularAttribute)) {
							return typeface.Replace(regularAttribute, boldAttribute);
						}
						if (typeface.Contains(lightAttribute)) {
							return typeface.Replace(lightAttribute, boldAttribute);
						}
					}
				}
				return typeface;
			}
		}

		public static string GetNameByTypeface(string typeface)
		{
			var nswFont = nswFonts.SingleOrDefault(x => x.Typeface == typeface);
			if (nswFont != null) {
				return nswFont.Name;
			}
			return TR.Tr(defaultFontFamily);
		}

		public static List<NSWFont> GetFonts()
		{
			return nswFonts;
		}

		public static List<string> GetFontNames()
		{
			return fontNames;
		}

		public static void SetFont(string fontName)
		{
			var nswFont = nswFonts.SingleOrDefault(x => x.Name == fontName);
			if (nswFont != null) {
				var typeface = nswFont.Typeface;
				if (typeface == TR.Tr(defaultFontFamily)) {
					Settings.FontFamily = defaultFontFamily;
				} else {
					Settings.FontFamily = typeface;
				}
			}
		}

		static void initFonts()
		{
			nswFonts = new List<NSWFont>();
			fontNames = new List<string>();
			fontAdd("Merriweather", "Merriweather-Regular");
			fontAdd("Montserrat", "Montserrat-Regular");
			fontAdd("Inconsolata", "Inconsolata-Regular");
			fontAdd("Lobster", "Lobster-Regular");
			fontAdd("Open Sans Condensed Light", "OpenSansCondensed-Light");
			fontAdd("Open Sans", "OpenSans-Regular");
			fontAdd("Oswald", "Oswald-Regular");
			fontAdd("Pacifico", "Pacifico-Regular");
			fontAdd("Playfair Display", "PlayfairDisplay-Regular");
			fontAdd("Raleway", "Raleway-Regular");
			nswFonts.Sort((x, y) => string.Compare(x.Name, y.Name));
			fontAdd(TR.Tr(defaultFontFamily), null, true);
		}

		static void fontAdd(string name, string typeface, bool beginning = false)
		{
			if (Device.RuntimePlatform == Device.Android) {
				if (typeface != null) {
					typeface = typeface + fontExtension + "#" + typeface;
				} else {
					typeface = name;
				}
			}

			if (nswFonts != null) {
				if (beginning) {
					nswFonts.Insert(0, new NSWFont {
						Name = name,
						Typeface = typeface
					});
				} else {
					nswFonts.Add(new NSWFont {
						Name = name,
						Typeface = typeface
					});
				}
			}

			if (fontNames != null) {
				if (beginning) {
					fontNames.Insert(0, name);
				} else {
					fontNames.Add(name);
				}
			}
		}
	}
}