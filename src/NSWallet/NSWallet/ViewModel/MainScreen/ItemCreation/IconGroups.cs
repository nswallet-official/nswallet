using System;
using System.Collections.Generic;
using System.Linq;
using NSWallet.Shared;

namespace NSWallet
{
	public static class IconGroups
	{
		public static string[] GetIconGroupsForPopup()
		{
			var iconGroups = GetIconGroups(true);
			var iconGroupsList = new List<string>(iconGroups);
			iconGroupsList.Insert(0, TR.Tr("create_icon_group_popup_item"));
			return iconGroupsList.ToArray();
		}

		public static string[] GetIconGroups(bool translated = false)
		{
			var nswGroups = BL.GetGroups();
			if (nswGroups != null) {
				var categories = nswGroups.Select(x => x.Name).ToArray();

				if (!translated) {
					return categories;
				}

				var translatedCategories = new string[categories.Length];
				for (int i = 0; i < categories.Length; i++) {
					translatedCategories[i] = TR.Tr(categories[i]);
				}

				Array.Sort(translatedCategories);
				return translatedCategories;
			}
			return null;
		}

		/// <summary>
		/// Gets the IDB y category scheme.
		/// </summary>
		/// <returns>The IDB y category scheme.</returns>
		/// <param name="category">Category scheme (e.g. category_games).</param>
		public static int GetIDByCategoryScheme(string category)
		{
			try {
				var groups = BL.GetGroups();
				if (groups != null) {
					var group = groups.Find(x => x.Name == category);
					if (group != null) {
						return group.GroupID;
					}
				}
				return -1;
			} catch {
				return -1;
			}
		}

		/// <summary>
		/// Gets the IDB y category scheme.
		/// </summary>
		/// <returns>The IDB y category name.</returns>
		/// <param name="category">Category name (e.g. Games).</param>
		public static int GetIDByCategoryName(string category)
		{
			try {
				var groups = BL.GetGroups();
				if (groups != null) {
					var group = groups.Find(x => TR.Tr(x.Name) == category);
					if (group != null) {
						return group.GroupID;
					}
				}
				return -1;
			} catch {
				return -1;
			}
		}

		public static Dictionary<string, string> GetIconsWithGroups()
		{
			return new Dictionary<string, string> {
				{ "accesspoint", "category_internet" },
				{ "airplane", "category_travelling" },
				{ "amazon", "category_internet" },
				{ "americanexpress", "category_finances" },
				{ "apple", "category_technologies" },
				{ "document", "category_common" },
				{ "aspirin", "category_common" },
				{ "badoo", "category_social" },
				{ "banking", "category_finances" },
				{ "bitcoin", "category_finances" },
				{ "blackberry", "category_technologies" },
				{ "blogger", "category_internet" },
				{ "book", "category_common" },
				{ "calendar", "category_common" },
				{ "cd", "category_common" },
				{ "chrome", "category_internet" },
				{ "clock", "category_common" },
				{ "coins", "category_finances" },
				{ "comp", "category_technologies" },
				{ "comp2", "category_technologies" },
				{ "comp3", "category_technologies" },
				{ "compass", "category_common" },
				{ "corona", "category_development" },
				{ "cp", "category_development" },
				{ "dollar", "category_finances" },
				{ "droid", "category_technologies" },
				{ "dropbox", "category_clouds" },
				{ "earth", "category_internet" },
				{ "earth2", "category_internet" },
				{ "ebay", "category_internet" },
				{ "ebook", "category_technologies" },
				{ "euro", "category_finances" },
				{ "evernote", "category_internet" },
				{ "exchange", "category_internet" },
				{ "facebook", "category_social" },
				{ "firefox", "category_internet" },
				{ "fireman", "category_common" },
				{ "firstaid", "category_common" },
				{ "flag", "category_common" },
				{ "forpdaru", "category_internet" },
				{ "gameloft", "category_games" },
				{ "gift", "category_common" },
				{ "giftcard", "category_common" },
				{ "giftcard2", "category_common" },
				{ "gmail", "category_internet" },
				{ "googlecheckout", "category_finances" },
				{ "googledrive", "category_clouds" },
				{ "gplay", "category_games" },
				{ "gplus", "category_social" },
				{ "gtalk", "category_social" },
				{ "heart", "category_common" },
				{ "helicopter", "category_common" },
				{ "home", "category_common" },
				{ "hostgator", "category_internet" },
				{ "hrs", "category_travelling" },
				{ "icq", "category_social" },
				{ "info", "category_common" },
				{ "instg", "category_social" },
				{ "ipad", "category_technologies" },
				{ "ipad2", "category_technologies" },
				{ "ipad3", "category_technologies" },
				{ "iphone", "category_technologies" },
				{ "iphone2", "category_technologies" },
				{ "iphone3", "category_technologies" },
				{ "key", "category_common" },
				{ "key2", "category_common" },
				{ "kinopoisk", "category_internet" },
				{ "lingualeo", "category_internet" },
				{ "linkedin", "category_social" },
				{ "linux1", "category_technologies" },
				{ "linux2", "category_technologies" },
				{ "live", "category_social" },
				{ "lj", "category_social" },
				{ "lock", "category_common" },
				{ "mac", "category_technologies" },
				{ "maestro", "category_finances" },
				{ "mail3", "category_internet" },
				{ "mailbox", "category_common" },
				{ "mastercard", "category_finances" },
				{ "medical", "category_common" },
				{ "milesmore", "category_travelling" },
				{ "money", "category_finances" },
				{ "money2", "category_finances" },
				{ "oovoo", "category_social" },
				{ "passport", "category_common" },
				{ "paypal", "category_finances" },
				{ "pencil", "category_common" },
				{ "phone1", "category_technologies" },
				{ "phone2", "category_technologies" },
				{ "phone3", "category_technologies" },
				{ "phone4", "category_technologies" },
				{ "phone5", "category_technologies" },
				{ "picasa", "category_internet" },
				{ "pocket", "category_common" },
				{ "police", "category_common" },
				{ "psp", "category_games" },
				{ "owncloud", "category_clouds" },
				{ "redcar", "category_common" },
				{ "rss", "category_internet" },
				{ "safe", "category_finances" },
				{ "satellitedish", "category_common" },
				{ "securitye", "category_common" },
				{ "securityq", "category_common" },
				{ "server", "category_internet" },
				{ "settings", "category_common" },
				{ "skydrive", "category_clouds" },
				{ "skype", "category_social" },
				{ "sportcar", "category_common" },
				{ "steam", "category_games" },
				{ "stick", "category_technologies" },
				{ "surfingbird", "category_internet" },
				{ "sync", "category_common" },
				{ "taxi", "category_common" },
				{ "teamviewer", "category_internet" },
				{ "terminal", "category_development" },
				{ "truck", "category_common" },
				{ "truecrypt", "category_technologies" },
				{ "tune", "category_common" },
				{ "twitter", "category_social" },
				{ "ubuntu", "category_technologies" },
				{ "unfuddle", "category_development" },
				{ "user", "category_common" },
				{ "utorrent", "category_internet" },
				{ "vacation", "category_common" },
				{ "vimeo", "category_internet" },
				{ "visa", "category_finances" },
				{ "visadebit", "category_finances" },
				{ "visaelectron", "category_finances" },
				{ "vkontakte", "category_social" },
				{ "wall", "category_internet" },
				{ "whitecar", "category_common" },
				{ "wiki", "category_internet" },
				{ "win", "category_technologies" },
				{ "windows", "category_technologies" },
				{ "wireless", "category_internet" },
				{ "wmk", "category_finances" },
				{ "wmlogo", "category_finances" },
				{ "wordpress", "category_internet" },
				{ "xcode", "category_development" },
				{ "xmarks", "category_internet" },
				{ "yahoo", "category_internet" },
				{ "yandex", "category_internet" },
				{ "youtube", "category_internet" }
			};
		}
	}
}