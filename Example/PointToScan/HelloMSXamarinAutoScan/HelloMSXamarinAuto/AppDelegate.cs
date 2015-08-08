using Foundation;
using UIKit;
using Moodstocks;
using System;

namespace HelloMSXamarinAuto
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		const string MSApiKey = "YourApiKey";
		const string MSApiSecret = "YourApiSecret";
		MSScanner _scanner;

		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			NSError error;
			var path = MSScanner.CachesPathFor("scanner.db");
			_scanner = new MSScanner ();
			_scanner.OpenWithPath(path, MSApiKey, MSApiSecret, out error);
			_scanner.SyncInBackgroundWithBlock (CompletedBlockHandler, ProgressBlockHandler);

			window = new UIWindow(UIScreen.MainScreen.Bounds);

			window.BackgroundColor = UIColor.White;
			var scannerVC = new ScannerViewController();
			scannerVC.Scanner = _scanner;

			window.RootViewController = scannerVC;
			window.MakeKeyAndVisible();
			
			return true;
		}

		void CompletedBlockHandler(MSSync sync, NSError error)
		{
			if (error != null)
				Console.WriteLine (string.Format("Sync failed with error: {0}", Moodstocks_NSError.Ms_message(error)));
			else 
			{
				NSError outError;
				Console.WriteLine(string.Format("Sync succeeded ({0} images(s))", _scanner.Count(out outError)));
			}
		}

		void ProgressBlockHandler(int percent) 
		{
			Console.WriteLine(string.Format("Sync progressing: {0}", percent));
		}

		public override void WillTerminate (UIApplication application)
		{
			NSError error;
			_scanner.Close(out error);
		}
	}
}

