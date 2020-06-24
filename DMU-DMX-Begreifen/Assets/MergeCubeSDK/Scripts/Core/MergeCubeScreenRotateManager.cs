using UnityEngine;

namespace MergeCube {
	public class MergeCubeScreenRotateManager : MonoBehaviour {
		public static MergeCubeScreenRotateManager instance;

		private void Awake ()
		{
			if (instance == null)
				instance = this;
			else if (instance != this)
				DestroyImmediate (this);
			lastOrientation = Screen.orientation;
		}

		public Callback<ScreenOrientation> OnOrientationEvent;
		private ScreenOrientation lastOrientation;
		private bool isVRMode;
		private bool wasVRMode;
		private bool isLockedMode;

		private void Update ()
		{
			if (!isLockedMode) {
				CheckOrientation ();
			}
		}

		private void CheckOrientation ()
		{
			if (lastOrientation != Screen.orientation) {
				lastOrientation = Screen.orientation;
				if (OnOrientationEvent != null) {
					OnOrientationEvent.Invoke (Screen.orientation);
				}
				if (wasVRMode != isVRMode) {
					if (isVRMode && IsLandscapeMode)
						SetToVRMode ();
				}
			}
		}
		public void SetOrientation (bool isVR)
		{
			if (!isLockedMode) {
				isVRMode = isVR;
				Screen.orientation = ScreenOrientation.AutoRotation;
				Screen.autorotateToLandscapeLeft = true;
				Screen.autorotateToLandscapeRight = true;
				if (!isVR) {
					SetToARMode ();
				} else if (IsLandscapeMode) {
					SetToVRMode ();
				}
			}
		}

		private void SetToARMode ()
		{
			Screen.autorotateToPortrait = true;
			Screen.autorotateToPortraitUpsideDown = false;
			wasVRMode = isVRMode;
		}

		private void SetToVRMode ()
		{
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			wasVRMode = isVRMode;
		}
		public static bool IsLandscapeMode {
			get {
				return (Screen.orientation == ScreenOrientation.Landscape
						|| Screen.orientation == ScreenOrientation.LandscapeLeft
						|| Screen.orientation == ScreenOrientation.LandscapeRight);
			}
		}
		public void LockToCurrentOrientation ()
		{
			isLockedMode = true;
			Screen.orientation = Screen.orientation;
		}
		public void UnlockCurrentOrientation ()
		{
			isLockedMode = false;
			SetOrientation (isVRMode);
		}
	}
}