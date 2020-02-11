using System;
using AppKit;
using Foundation;
using ImageCircle.Forms.Plugin.Mac;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace NSWallet.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow _window;

        public AppDelegate()
        {
			var style = NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

			var rect = new CoreGraphics.CGRect(200, 1000, 800, 512);
			_window = new NSWindow(rect, style, NSBackingStore.Buffered, false) {
				Title = "NS Wallet on Mac!",
				TitleVisibility = NSWindowTitleVisibility.Hidden
			};

			NSApplication.SharedApplication.MainMenu = GetMenuBar();
			//NSApplication.SharedApplication.MainMenu = new NSMenu();
		}

		static NSMenu GetMenuBar()
		{
			// top bar app menu
			NSMenu menubar = new NSMenu();
			NSMenuItem appMenuItem = new NSMenuItem("NS Wallet");

			menubar.AddItem(appMenuItem);

			NSMenu appMenu = new NSMenu();

			var quitMenuItem = new NSMenuItem("Quit", "q", delegate {
				NSApplication.SharedApplication.Terminate(menubar);
			});
			appMenu.AddItem(quitMenuItem);

			menubar.SetSubmenu(appMenu, appMenuItem);
			return menubar;

		}

		public override NSWindow MainWindow
        {
            get { return _window; }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();
			ImageCircleRenderer.Init();
			FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
			LoadApplication(new App());

			ObjCRuntime.Class.ThrowOnInitFailure = false;
            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }


    }
}
