using System;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
using MonoTouch.CoreMedia;

namespace Moodstocks {

	public delegate void CancelBlock(MSSync sync, NSError error);
	public delegate void CompletedBlock(MSSync sync, NSError error);
	public delegate void ProgressBlock(int percent);

	[BaseType (typeof (NSOperation))]
	public partial interface MSApiSearch {

		[Export ("error", ArgumentSemantic.Retain)]
		NSError Error { get; }

		[Export ("result", ArgumentSemantic.Retain)]
		MSResult Result { get; }

		[Export ("initWithScanner:query:")]
		IntPtr Constructor (MSScanner scanner, MSImage query);

		[Export ("progressBlock:setProgressBlock")]
		void ProgressBlock(int percent);

		[Export ("cancelBlock:setCancelBlock")]
		void CancelBlock(NSError error);

		[Export ("completedBlock:setCompletedBlock")]
		void CompletedBlock(NSError error);
	}

	[BaseType (typeof (NSObject))]
	public partial interface MSAutoScannerSession {

		[Export ("delegate", ArgumentSemantic.Assign)]
		MSAutoScannerSessionDelegate Delegate { get; set; }

		[Export ("resultTypes")]
		int ResultTypes { get; set; }

		[Export ("resultExtras")]
		int ResultExtras { get; set; }

		[Export ("searchOptions")]
		int SearchOptions { get; set; }

		[Export ("interfaceOrientation")]
		UIInterfaceOrientation InterfaceOrientation { get; set; }

		[Export ("captureLayer")]
		AVCaptureVideoPreviewLayer CaptureLayer { get; }

		[Export ("initWithScanner:")]
		IntPtr Constructor (MSScanner scanner);

		[Export ("startRunning")]
		void StartRunning ();

		[Export ("stopRunning")]
		void StopRunning ();

		[Export ("pauseProcessing")]
		bool PauseProcessing();

		[Export ("resumeProcessing")]
		bool ResumeProcessing();
	}

	[Model, BaseType (typeof (NSObject)), Protocol]
	public partial interface MSAutoScannerSessionDelegate {

		[Export ("session:didFindResult:")]
		void DidFindResult (NSObject scannerSession, MSResult result);

		[Export ("session:didFindResult:forVideoFrame:")]
		void DidFindResult (NSObject scannerSession, MSResult result, UIImage videoFrame);

		[Export ("session:didEncounterWarning:")]
		void DidEncounterWarning (NSObject scannerSession, string warning);
	}

	[BaseType (typeof (NSObject))]
	public partial interface MSImage {

		[Static, Export ("imageWithBuffer:error:")]
		NSObject ImageWithBuffer (CMSampleBuffer imageBuffer, out NSError error);

		[Static, Export ("imageWithBuffer:orientation:error:")]
		NSObject ImageWithBuffer (CMSampleBuffer imageBuffer, AVCaptureVideoOrientation videoOrientation, out NSError error);

		[Static, Export ("imageWithUIImage:error:")]
		NSObject ImageWithUIImage (UIImage image, out NSError error);

		[Static, Export ("imageWithGrayscalePixels:width:height:stride:orientation:error:")]
		NSObject ImageWithGrayscalePixels ([PlainString] string pixels, int width, int height, int stride, AVCaptureVideoOrientation orientation, out NSError error);
	}

	[BaseType (typeof (NSObject))]
	public partial interface MSManualScannerSession {

		[Export ("delegate", ArgumentSemantic.Assign)]
		MSManualScannerSessionDelegate Delegate { get; set; }

		[Export ("resultTypes")]
		int ResultTypes { get; set; }

		[Export ("wantsQuery")]
		bool WantsQuery { get; set; }

		[Export ("interfaceOrientation")]
		UIInterfaceOrientation InterfaceOrientation { get; set; }

		[Export ("captureLayer")]
		AVCaptureVideoPreviewLayer CaptureLayer { get; }

		[Export ("initWithScanner:")]
		IntPtr Constructor (MSScanner scanner);

		[Export ("startRunning")]
		void StartRunning ();

		[Export ("stopRunning")]
		void StopRunning ();

		[Export ("pauseProcessing")]
		bool PauseProcessing();

		[Export ("resumeProcessing")]
		bool ResumeProcessing();

		[Export ("snap")]
		bool Snap();

		[Export ("cancel")]
		bool Cancel();
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface MSManualScannerSessionDelegate {

		[Export ("sessionWillStartServerRequest:")]
		void SessionWillStartServerRequest(NSObject scannerSession);

		[Export ("session:didFindResult:optionalQuery:")]
		void DidFindResult (NSObject scannerSession, MSResult result, UIImage query);

		[Export ("session:didFailWithError:")]
		void DidFailWithError (NSObject scannerSession, NSError error);

		[Export ("session:didEncounterWarning:")]
		void DidEncounterWarning (NSObject scannerSession, string warning);
	}

	public delegate void ResultBlock();


	[BaseType (typeof (NSObject))]
	public partial interface MSResult {

		[Export ("type")]
		MSResultType Type { get; }

		[Export ("origin")]
		MSResultOrigin Origin { get; }

		[Export ("data", ArgumentSemantic.Strong)]
		NSData Data { get; }

		[Export ("string", ArgumentSemantic.Strong)]
		string String { get; }

		[Export ("corners", ArgumentSemantic.Strong)]
		NSValue Corners { get; }

		[Export ("homography", ArgumentSemantic.Strong)]
		NSValue Homography { get; }

		[Export ("dimensions", ArgumentSemantic.Strong)]
		NSValue Dimensions { get; }

		[Export ("warpImage:block:")]
		void WarpImage (UIImage image, ResultBlock resultBlock);

		[Export ("warpImage:scale:block:")]
		void WarpImage (UIImage image, float scale, ResultBlock resultBlock);

		[Static, Export ("dataFromBase64URLString:")]
		NSData DataFromBase64URLString (string value);
	}

	[BaseType (typeof (NSObject))]
	public partial interface MSScanner {

		[Export ("isSyncing")]
		bool IsSyncing { get; }

		[Export ("syncProgress")]
		int SyncProgress { get; }

		[Export ("syncError")]
		NSError SyncError { get; }

		[Export ("openWithPath:key:secret:error:")]
		bool OpenWithPath (string path, string key, string secret, out NSError error);

		[Export ("close:")]
		bool Close (out NSError error);

		[Static, Export ("cachesPathFor:")]
		string CachesPathFor (string fileName);

		[Export ("importBundle:error:")]
		bool ImportBundle (NSBundle bundle, out NSError error);

		[Export ("syncInBackground")]
		void SyncInBackground ();

		[Export ("syncInBackgroundWithBlock:progressBlock:")]
		void SyncInBackgroundWithBlock (CompletedBlock completedBlock, ProgressBlock progressBlock);

		[Export ("cancelSync")]
		void CancelSync ();

		[Export ("count:")]
		int Count (out NSError error);

		[Export ("info:")]
		NSObject [] Info (out NSError error);

		[Export ("searchWithQuery:options:extras:error:")]
		MSResult SearchWithQuery (MSImage query, int options, int extras, out NSError error);

		[Export ("decodeWithQuery:formats:extras:error:")]
		MSResult DecodeWithQuery (MSImage query, int formats, int extras, out NSError error);

		[Export ("apiSearchInBackgroundWithQuery:block:")]
		void ApiSearchInBackgroundWithQuery (MSImage query, CompletedBlock completedBlock);

		[Export ("cancelApiSearches")]
		void CancelApiSearches ();

		[Static, Export ("detectProxySettings:")]
		NSDictionary DetectProxySettings (out NSError error);

		[Static, Export ("detectProxySettings:error:")]
		NSDictionary DetectProxySettings (out NSDictionary systemSettings, out NSError error);

		[Export ("setProxySettings:port:error:")]
		bool SetProxySettings (string host, NSNumber port, out NSError error);

		[Export ("setProxySettings:port:username:password:error:")]
		bool SetProxySettings (string host, NSNumber port, string username, string password, out NSError error);
	}

	[BaseType (typeof (NSOperation))]
	public partial interface MSSync {

		[Export ("error", ArgumentSemantic.Retain)]
		NSError Error { get; }

		[Export ("initWithScanner:")]
		IntPtr Constructor (MSScanner scanner);

		[Export ("progressBlock:setProgressBlock")]
		void ProgressBlock(int percent);

		[Export ("cancelBlock:setCancelBlock")]
		void CancelBlock(NSError error);

		[Export ("completedBlock:setCompletedBlock")]
		void CompletedBlock(NSError error);
	}

	[Category, BaseType (typeof (NSError))]
	public partial interface Moodstocks_NSError {

		[Static, Export ("ms_errorWithCode:")]
		NSError Ms_errorWithCode (int code);

		[Export ("ms_message")]
		string Ms_message ();
	}
}


