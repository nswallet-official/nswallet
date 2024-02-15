using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using NSWallet.Helpers;
using NSWallet.Shared;
using NSWallet.Shared.Helpers.Logs.AppLog;
using Plugin.InAppBilling;
using Xamarin.Forms;

namespace NSWallet.Premium
{
	public static class PremiumManagement
	{
		static bool forcedPremium = false; // Demo only (needed for screenshots on simulator)
										   // Properties
										   //static string currentDevice { get; set; }
		static string[] currentProductIDs { get; set; }
		static string[] allProductIDs { get; set; }
		public static List<string> PurchasesList { get; set; }

		// Constants
		const string undetected_phone = "undetected_device";
		const string undetected_product_id = "undetected_product_id";

		const string status_not_set = "not_set";
		const string status_premium = "premium";
		const string status_subscription = "subscription";
		const string status_free = "free";

		const string purchase_state_canceled = "canceled";
		const string purchase_state_deferred = "deferred";
		const string purchase_state_failed = "failed";
		const string purchase_state_payment_pending = "payment_pending";
		const string purchase_state_purchased = "purchased";
		const string purchase_state_purchasing = "purchasing";
		const string purchase_state_restored = "restored";
		const string purchase_state_unknown = "unknown";

		const string android_product_id_premium = "com.nyxbull.nswallet.premium";
		const string android_product_id_sale = "com.nyxbull.nswallet.premium.sale";
		const string android_product_id_subscription = "ns.wallet.premium.subscription";
		const string ios_product_id_subscription = "com.nyxbull.nswallet.subscription";
		const string ios_product_id_free_inapp = "com.nyxbull.nswallet.free.inapp";

		static string[] all_android_product_ids = { android_product_id_premium, android_product_id_sale, android_product_id_subscription };
		static string[] all_ios_product_ids = { ios_product_id_subscription, ios_product_id_free_inapp };

		const string Payload = "nswpayload";

		static PremiumManagement()
		{
			PurchasesList = new List<string>();
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					//     currentDevice = Device.iOS;
					currentProductIDs = all_ios_product_ids;
					break;
				case Device.Android:
					//   currentDevice = Device.Android;
					currentProductIDs = allProductIDs;
					break;
					/*
                default:
                    currentDevice = undetected_phone;
                    allProductIDs = non_consumables_ios_product_ids.Concat(non_consumables_android_product_ids).ToArray();
                    break;
                    */
			}
		}

		public static void ShowBuyPremiumPopup()
		{
			Device.BeginInvokeOnMainThread(
				() => Application.Current.MainPage.DisplayAlert(
					TR.Tr("premium"),
					TR.Tr("premium_description"),
					TR.Tr("premium_buy"),
					TR.Cancel
				).ContinueWith(t => {
					if (t.Result)
					{
						Device.BeginInvokeOnMainThread(() => AppPages.Premium(Application.Current.MainPage.Navigation));
					}
				})
			);
		}

		/// <summary>
		/// Gets the subscription price.
		/// </summary>
		/// <returns>The subscription price.</returns>
		public static async Task<string> GetSubscriptionPrice()
		{
			var billing = CrossInAppBilling.Current;
			var errorPrice = TR.Tr("price_loading_failed");

			try
			{
				var connected = await billing.ConnectAsync();

				if (!connected)
				{
					return errorPrice;
				}

				string productID = undetected_product_id;
				switch (Device.RuntimePlatform)
				{
					case Device.iOS:
						productID = ios_product_id_subscription;
						break;
					case Device.Android:
						productID = android_product_id_subscription;
						break;
				}

				var purchases = await billing.GetProductInfoAsync(ItemType.Subscription, productID);

				if (purchases != null)
				{
					var purchasesList = purchases.ToList();
					if (purchasesList.Count > 0)
					{
						return purchasesList[0].LocalizedPrice + " " + purchasesList[0].CurrencyCode;
					}
					else
					{
						return errorPrice;
					}
				}

				return errorPrice;
			}
			catch (InAppBillingPurchaseException ex)
			{
				log(ex.Message, nameof(GetSubscriptionPrice));
				return errorPrice;
			}
			catch (Exception ex)
			{
				log(ex.Message, nameof(GetSubscriptionPrice));
				return errorPrice;
			}
			finally
			{
				await billing.DisconnectAsync();
			}
		}

		/// <summary>
		/// Purchase the specified premiumStatus.
		/// </summary>
		/// <returns>The purchase.</returns>
		/// <param name="premiumStatus">Premium status.</param>
		public static async Task<string> Purchase(PremiumStatus premiumStatus)
		{
			string productID = undetected_product_id;

			switch (premiumStatus)
			{
				// Non-consumable
				/*
                case PremiumStatus.Premium:
                    switch (currentDevice)
                    {
                        case Device.iOS:
                            productID = ios_product_id_free_inapp;
                            break;
                        case Device.Android:
                            productID = android_product_id_premium;
                            break;
                    }
                    break;
                    */

				case PremiumStatus.Subscription:
					switch (Device.RuntimePlatform)
					{
						case Device.iOS:
							productID = ios_product_id_subscription;
							break;
						case Device.Android:
							productID = android_product_id_subscription;
							break;
					}
					break;
			}

			if (productID == undetected_product_id)
			{
				return "Product ID is not set";
			}

			var billing = CrossInAppBilling.Current;

			try
			{
				var connected = await billing.ConnectAsync();
				if (!connected)
				{
					SetCommonStatus(false);
					return TR.Tr("settings_purchase_premium_fail_connection");
				}

				var purchase = await billing.PurchaseAsync(productID, ItemType.Subscription);

				/*
                switch (premiumStatus)
                {
                    case PremiumStatus.Premium:
                        purchase = await billing.PurchaseAsync(productID, ItemType.InAppPurchase, Payload);
                        break;
                    case PremiumStatus.Subscription:
                        purchase = await billing.PurchaseAsync(productID, ItemType.Subscription, Payload);
                        break;
                }
                */

				if (purchase == null)
				{
					SetCommonStatus(false);
					return TR.Tr("settings_purchase_premium_fail");
				}

				SetPremiumSubscription(true);
				SetSubscriptionDetails(purchase.AutoRenewing, purchase.State, purchase.TransactionDateUtc);

				/*
                    if (premiumStatus.Equals(PremiumStatus.Subscription))
                    {
                        
                    }
                    else if (premiumStatus.Equals(PremiumStatus.Premium))
                    {
                        SetOldPremium(true);
                    }
                    */

				SetCommonStatus(true, true);
				return TR.Tr("settings_purchase_premium_success");

			}
			catch (InAppBillingPurchaseException ex)
			{
				log(ex.Message, nameof(Purchase));
				switch (ex.PurchaseError)
				{
					// FIXME: handle errors
					case PurchaseError.AppStoreUnavailable:
						var failureAppStore = "settings_purchase_premium_fail_app_store";
						SetCommonStatus(false);
						log(failureAppStore);
						return TR.Tr(failureAppStore);
					default:
						var failure = "settings_purchase_premium_fail";
						log(failure);
						SetCommonStatus(false);
						return TR.Tr("settings_purchase_premium_fail");
				}
			}
			catch (Exception ex)
			{
				SetCommonStatus(false);
				log(ex.Message, nameof(Purchase));
				return TR.Tr("settings_purchase_premium_fail");
			}
			finally
			{
				await billing.DisconnectAsync();
			}
		}

		/// <summary>
		/// Sets the old premium.
		/// </summary>
		/// <param name="isOldPremium">If set to <c>true</c> is old premium.</param>
		public static void SetOldPremium(bool isOldPremium)
		{
			Settings.IsPremiumOld = isOldPremium;
		}

		/// <summary>
		/// Sets the premium subscription.
		/// </summary>
		/// <param name="isSubscription">If set to <c>true</c> is subscription.</param>
		public static void SetPremiumSubscription(bool isSubscription)
		{
			Settings.IsPremiumSubscription = isSubscription;
		}

		/// <summary>
		/// Sets the subscription details.
		/// </summary>
		/// <param name="autoRenewing">If set to <c>true</c> auto renewing.</param>
		/// <param name="state">State.</param>
		/// <param name="transactionDate">Transaction date.</param>
		public static void SetSubscriptionDetails(bool autoRenewing, PurchaseState state, DateTime transactionDate)
		{
			//Settings.PremiumAutoRenewing = autoRenewing;
			Settings.PremiumSubscriptionState = checkPurchaseState(state);
			Settings.PremiumSubscriptionDate = transactionDate;
		}

		static async Task<bool> wasSubscriptionPurchased(string productID)
		{
			var billing = CrossInAppBilling.Current;

			try
			{
				var connected = await billing.ConnectAsync();

				if (!connected)
				{
					return false;
				}

				var purchases = await billing.GetPurchasesAsync(ItemType.Subscription);

				if (purchases != null)
				{
					foreach (var purchase in purchases)
					{
						if (purchase.ProductId == productID)
						{
							return true;
						}
					}
				}

				return false;
			}
			catch (InAppBillingPurchaseException ex)
			{
				log(ex.Message, nameof(wasSubscriptionPurchased));
				return false;
			}
			catch (Exception ex)
			{
				log(ex.Message, nameof(wasSubscriptionPurchased));
				return false;
			}
			finally
			{
				await billing.DisconnectAsync();
			}
		}

		/// <summary>
		/// Checks if the item purchased.
		/// </summary>
		/// <returns>The item purchased.</returns>
		/// <param name="productID">Product identifier.</param>
		static async Task<bool> wasNonConsumableItemPurchased(string productID)
		{
			var billing = CrossInAppBilling.Current;

			try
			{
				var connected = await billing.ConnectAsync();

				if (!connected)
				{
					return false;
				}

				var purchases = await billing.GetPurchasesAsync(ItemType.InAppPurchase);

				if (purchases?.Any(p => p.ProductId == productID) ?? false)
				{
					return true;
				}

				return false;

			}
			catch (InAppBillingPurchaseException ex)
			{
				log(ex.Message, nameof(wasNonConsumableItemPurchased));
				return false;
			}
			catch (Exception ex)
			{
				log(ex.Message, nameof(wasNonConsumableItemPurchased));
				return false;
			}
			finally
			{
				await billing.DisconnectAsync();
			}
		}

		/// <summary>
		/// Gets the premium status.
		/// </summary>x
		/// <value>The premium status.</value>
		public static PremiumStatus PremiumStatus
		{
			get
			{
				if (forcedPremium)
				{
					return PremiumStatus.Subscription;
				}
				var premiumStatus_string = Settings.PremiumStatus;
				var premiumStatus = checkPremiumStatus(premiumStatus_string);

				if (premiumStatus.Equals(PremiumStatus.NotSet))
				{
					PremiumStatus = PremiumStatus.Free;
				}

				return premiumStatus;
			}
			set
			{
				Settings.PremiumStatus = checkPremiumStatus(value);
			}
		}


		/// <summary>
		/// Gets the purchases.
		/// </summary>
		/// <returns>The purchases.</returns>
		public static async Task<PremiumStatus> GetPurchases()
		{
			try
			{
				// Temporary BETA for Mac Desktop version
				if (Device.RuntimePlatform == Device.macOS)
				{
					SetPremiumSubscription(true);
					SetCommonStatus(true, true);
					return PremiumStatus.Subscription;
				}
				// Temporary BETA for Windows Desktop version
				if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.WPF)
				{
					SetPremiumSubscription(true);
					SetCommonStatus(true, true);
					return PremiumStatus.Subscription;
				}

				// Permanent Premium for Linux Desktop version :)
				if (Device.RuntimePlatform == Device.GTK)
				{
					SetPremiumSubscription(true);
					SetCommonStatus(true, true);
					return PremiumStatus.Subscription;
				}

				string[] prodIDs = null;

				switch (Device.RuntimePlatform)
				{
					case Device.iOS:
						prodIDs = all_ios_product_ids;
						break;
					case Device.Android:
						prodIDs = all_android_product_ids;
						break;
					default:
						return PremiumStatus.Free;
				}

				if (prodIDs != null)
				{
					bool subscriptionFound = false;
					bool oldPurchasesFound = false;

					PurchasesList = new List<string>();

					foreach (var productID in prodIDs)
					{
						var wasPurchasedNonConsumable = await wasNonConsumableItemPurchased(productID);
						var wasPurchasedSubscription = await wasSubscriptionPurchased(productID);

						if (wasPurchasedNonConsumable)
						{
							PurchasesList.Add("Non-consumable: " + productID);
							oldPurchasesFound = true;
						}

						if (wasPurchasedSubscription)
						{
							PurchasesList.Add("Subscription: " + productID);
							subscriptionFound = true;
						}
					}

					if (subscriptionFound)
					{
						SetPremiumSubscription(true);
						SetCommonStatus(true, true);
						return PremiumStatus.Subscription;
					}

					if (oldPurchasesFound)
					{
						SetOldPremium(true);
						SetCommonStatus(true);
						return PremiumStatus.LegacyPremium;
					}
				}
			}
			catch (Exception ex)
			{
				log(ex.Message, nameof(GetPurchases));
			}

			SetCommonStatus(false);
			return PremiumStatus.Free;
		}

		/// <summary>
		/// Checks the state of the purchase.
		/// </summary>
		/// <returns>The purchase state.</returns>
		/// <param name="purchaseState">Purchase state.</param>
		static string checkPurchaseState(PurchaseState purchaseState)
		{
			switch (purchaseState)
			{
				case PurchaseState.Canceled:
					return purchase_state_canceled;
				case PurchaseState.Deferred:
					return purchase_state_deferred;
				case PurchaseState.Failed:
					return purchase_state_failed;
				case PurchaseState.PaymentPending:
					return purchase_state_payment_pending;
				case PurchaseState.Purchased:
					return purchase_state_purchased;
				case PurchaseState.Purchasing:
					return purchase_state_purchasing;
				case PurchaseState.Restored:
					return purchase_state_restored;
				case PurchaseState.Unknown:
					return purchase_state_unknown;
				default:
					return purchase_state_unknown;
			}
		}

		/// <summary>
		/// Checks the state of the purchase.
		/// </summary>
		/// <returns>The purchase state.</returns>
		/// <param name="purchaseState">Purchase state.</param>
		static PurchaseState checkPurchaseState(string purchaseState)
		{
			switch (purchaseState)
			{
				case purchase_state_canceled:
					return PurchaseState.Canceled;
				case purchase_state_deferred:
					return PurchaseState.Deferred;
				case purchase_state_failed:
					return PurchaseState.Failed;
				case purchase_state_payment_pending:
					return PurchaseState.PaymentPending;
				case purchase_state_purchased:
					return PurchaseState.Purchased;
				case purchase_state_purchasing:
					return PurchaseState.Purchasing;
				case purchase_state_restored:
					return PurchaseState.Restored;
				case purchase_state_unknown:
					return PurchaseState.Unknown;
				default:
					return PurchaseState.Unknown;
			}
		}

		/// <summary>
		/// Checks the premium status.
		/// </summary>
		/// <returns>The premium status.</returns>
		/// <param name="premiumStatus">Premium status.</param>
		static PremiumStatus checkPremiumStatus(string premiumStatus)
		{
			switch (premiumStatus)
			{
				case status_not_set: return PremiumStatus.NotSet;
				case status_premium: return PremiumStatus.LegacyPremium;
				case status_subscription: return PremiumStatus.Subscription;
				case status_free: return PremiumStatus.Free;
				default: return PremiumStatus.NotSet;
			}
		}

		/// <summary>
		/// Checks the premium status.
		/// </summary>
		/// <returns>The premium status.</returns>
		/// <param name="premiumStatus">Premium status.</param>
		static string checkPremiumStatus(PremiumStatus premiumStatus)
		{

			switch (premiumStatus)
			{
				case PremiumStatus.NotSet: return status_not_set;
				case PremiumStatus.LegacyPremium: return status_premium;
				case PremiumStatus.Subscription: return status_subscription;
				case PremiumStatus.Free: return status_free;
				default: return status_not_set;
			}
		}


		/// <summary>
		/// Sets the status.
		/// </summary>
		/// <param name="isPremium">If set to <c>true</c>, status sets to premium.</param>
		public static void SetCommonStatus(bool isPremium, bool isSubscription = false)
		{
			if (isPremium)
			{
				if (isSubscription)
					Settings.PremiumStatus = checkPremiumStatus(PremiumStatus.Subscription);
				else
					Settings.PremiumStatus = checkPremiumStatus(PremiumStatus.LegacyPremium);
			}
			else
				Settings.PremiumStatus = checkPremiumStatus(PremiumStatus.Free);
		}

		public static bool IsOldPremium
		{
			get
			{
				if (Settings.IsPremiumOld)
					return true;
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:NSWallet.Premium.PremiumManagement"/> is subscription.
		/// </summary>
		/// <value><c>true</c> if is subscription; otherwise, <c>false</c>.</value>
		public static bool IsPremiumSubscription
		{
			get
			{
				if (Settings.IsPremiumSubscription)
					return true;
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:NSWallet.Premium.PremiumManagement"/> is old premium.
		/// </summary>
		/// <value><c>true</c> if is premium; otherwise, <c>false</c>.</value>
		public static bool IsLegacyPremium
		{
			get
			{
				if (PremiumStatus.Equals(PremiumStatus.LegacyPremium))
					return true;
				return false;
			}
		}

		public static bool IsAnyPremium
		{
			get
			{
				if (PremiumStatus.Equals(PremiumStatus.Subscription))
					return true;
				if (PremiumStatus.Equals(PremiumStatus.LegacyPremium))
					return true;
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:NSWallet.Premium.PremiumManagement"/> is free.
		/// </summary>
		/// <value><c>true</c> if is free; otherwise, <c>false</c>.</value>
		public static bool IsFree
		{
			get
			{
				if (PremiumStatus.Equals(PremiumStatus.Free))
					return true;
				return false;
			}
		}

		public static void RetrievePremiumStatus()
		{
			if (PremiumStatus == PremiumStatus.NotSet)
			{
				Task.Run(async () => {
					await GetPurchases();
				});
			}
		}

		public static bool CheckPreviousPremium(Page page)
		{
			try
			{
				bool premiumRecovered = false;
				if (Settings.FirstLaunch &&
					(Settings.PremiumStatus == checkPremiumStatus(PremiumStatus.Free)
					 || Settings.PremiumStatus == checkPremiumStatus(PremiumStatus.NotSet)))
				{
					var res = page.DisplayAlert(TR.Tr("attention"), TR.Tr("premium_start_description"), TR.Tr("restore"), TR.Cancel).ContinueWith(async (x) => {
						if (x.Result)
						{
							var status = await GetPurchases();
							if (IsAnyPremium)
							{
								premiumRecovered = true;

								if (IsPremiumSubscription)
								{
									MessageBox.ShowMessage(TR.Tr("settings_restore_premiumsubscription_success"));
								}
								else if (IsLegacyPremium)
								{
									MessageBox.ShowMessage(TR.Tr("settings_restore_premium_success"));
								}

							}
							else
							{
								MessageBox.ShowMessage(TR.Tr("restore_premium_fail"));
							}
						}
					});

					Settings.FirstLaunch = false;
				}
				return premiumRecovered;
			}
			catch (Exception ex)
			{
				AppLogs.Log(ex.Message, nameof(CheckPreviousPremium), nameof(PremiumManagement));
				return false;
			}
		}

		static void log(string message, string method = null)
		{
			AppLogs.Log(message, method, nameof(PremiumManagement));
		}
	}
}
