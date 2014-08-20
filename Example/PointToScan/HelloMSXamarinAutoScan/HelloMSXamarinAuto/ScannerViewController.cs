
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Moodstocks;
using MonoTouch.AVFoundation;

namespace HelloMSXamarinAuto
{
	public partial class ScannerViewController : UIViewController
	{
		const int KMSResultTypes = (int)(MSResultType.Image  | MSResultType.QRCode | MSResultType.EAN13);

		public MSScanner Scanner { get; set; }

		MSAutoScannerSession _scannerSession;

		public ScannerViewController () : base ("ScannerViewController", null){}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			_scannerSession = new MSAutoScannerSession(Scanner);
			_scannerSession.Delegate = new ScannerDelegate(this);
			_scannerSession.ResultTypes = KMSResultTypes;

			var videoPreviewLayer = videoPreview.Layer;
			videoPreviewLayer.MasksToBounds = true;

			var captureLayer = _scannerSession.CaptureLayer;
			captureLayer.Frame = videoPreview.Bounds;

			videoPreviewLayer.InsertSublayer (captureLayer, 0);

			_scannerSession.StartRunning ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
			_scannerSession.StopRunning ();
		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);

			UpdateInterfaceOrientation (toInterfaceOrientation);
		}

		void UpdateInterfaceOrientation(UIInterfaceOrientation interfaceOrientation)
		{
			_scannerSession.InterfaceOrientation = interfaceOrientation;

			var captureLayer = _scannerSession.CaptureLayer;

			captureLayer.Frame = View.Bounds;

			switch (interfaceOrientation) {
			case UIInterfaceOrientation.Portrait:
				captureLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
				break;
			case UIInterfaceOrientation.PortraitUpsideDown:
				captureLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.PortraitUpsideDown;
				break;
			case UIInterfaceOrientation.LandscapeLeft:
				captureLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeLeft;
				break;
			case UIInterfaceOrientation.LandscapeRight:
				captureLayer.Connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeRight;
				break;
			default:
				break;
			}
		}

		protected void ResumeProcessing()
		{
			_scannerSession.ResumeProcessing ();
		}

		class ScannerDelegate : MSAutoScannerSessionDelegate
		{
			ScannerViewController _viewController;

			public ScannerDelegate(ScannerViewController viewController):base()
			{
				_viewController = viewController;
			}

			public override void DidFindResult (NSObject scannerSession, MSResult result)
			{
				var title = result.Type == MSResultType.Image ? "Image" : "Barcode";
				var message = string.Format("{0}:\n{1}", title, result.String);
				var aSheet = new UIActionSheet (message, null, "OK", null, null);

				aSheet.Clicked += (object sender, UIButtonEventArgs e) => {
					_viewController.ResumeProcessing();
				};

				aSheet.ShowInView (_viewController.View);
			}
		}
	}
}

