using System;

namespace Moodstocks
{
	public enum MSResultType {
		None = 0,
		EAN8 = 1 << 0,
		EAN13 = 1 << 1,
		QRCode = 1 << 2,
		Datamatrix = 1 << 3,
		Image = 1 << 31
	}

	public enum MSResultOrigin {
		None = 0,
		Client = 1 << 0,
		Server = 1 << 1
	}

	public enum MSResultExtra {
		None = 0,
		Corners = 1 << 0,
		Homography = 1 << 1,
		Dimensions = 1 << 2
	}

	public enum MSSearchOption {
		Default = 0,
		NoPartial = 1 << 0,
		SmallTarget = 1 << 1
	}

	public enum MSErrorCode {
		Success = 0,
		Error,
		ErrorMisuse,
		ErrorNoPerm,
		ErrorNoFile,
		ErrorBusy,
		ErrorCorrupt,
		ErrorEmpty,
		ErrorAuth,
		ErrorNoConn,
		ErrorTimeout,
		ErrorThread,
		ErrorCredMismatch,
		ErrorSlowConn,
		ErrorNoRec,
		ErrorAbort,
		ErrorUnavail,
		ErrorImg,
		ErrorApiKey,
		ErrorNetworkFail,
		ErrorNotOpen,
		ErrorBundle,
		ErrorApiSecret,
	}
}

