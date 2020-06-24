
//Uncomment the #define line to use the recording extensions for iVidCapPro for iOS.
//Android recording support is in progress and will be released in a later patch
//The plugin can be acquired here, but is built into the project:
//http://eccentric-orbits.com/eoe/site/ividcappro-unity-plugin/

//For iOS:

#define SHOULD_USE_RECORDING_PLUGIN

using System.Collections;
using UnityEngine;
using MergeCube;

public class RecordingManager : MonoBehaviour {

	public GameObject recordStartExample;
	public GameObject recordSavingExample;
	public GameObject fullscreenRecordingCamera;
	private RenderTexture recordingTexture;

#if SHOULD_USE_RECORDING_PLUGIN
	public readonly static bool isUsingRecordingPlugin = true;
#else
	public readonly static bool isUsingRecordingPlugin = false;
#endif

	public static RecordingManager instance;

	private void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyImmediate (this.gameObject);
	}

#if SHOULD_USE_RECORDING_PLUGIN


#if UNITY_IOS
	private iVidCapPro vidCapManager;
	private int recFrames = 0;
#endif

	private Vector2 targetAspect;

	private bool isRecording;
	private bool isInitialized;


	private Callback OnRecordingSaveComplete;

	private void Start ()
	{
		Initialize ();

#if UNITY_IOS
		vidCapManager = this.GetComponent<iVidCapPro> ();
		vidCapManager.RegisterSessionCompleteDelegate (HandleRecSaveComplete);

		vidCapManager.SetDebug (true);

		targetAspect.x = Mathf.Max (Screen.width, Screen.height);
		targetAspect.y = Mathf.Min (Screen.width, Screen.height);

#endif

	}

	private void Initialize ()
	{
		if (Camera.main.gameObject.GetComponent<AudioListener> () == null) {
			Camera.main.gameObject.AddComponent<AudioListener> ();

			if (Camera.main.transform.GetComponent<AudioListener> () != null) {
				DestroyImmediate (Camera.main.transform.GetComponent<AudioListener> ());
			}
		}

#if UNITY_IOS
		if (Camera.main.transform.GetComponent<iVidCapProAudio> () == null) {
			Camera.main.gameObject.AddComponent<iVidCapProAudio> ();
		}


		vidCapManager = this.GetComponent<iVidCapPro> ();
		vidCapManager.saveAudio = Camera.main.GetComponent<iVidCapProAudio> ();

#endif

		MergeAndroidBridge.recordingStartState += StartSuccessHandle;
		isInitialized = true;
	}

	public void StartRec (string outputName, Callback OnRecordingComplete)
	{
#if UNITY_IOS && !UNITY_EDITOR
		if (MergeIOSBridge.CheckPhoto () != 1) {
			return;
		}
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
	//JON:
		if(!MergeAndroidBridge.HasPermission(AndroidPermission.WRITE_EXTERNAL_STORAGE)){
			Debug.Log("Expected Write permissions");
			return;
		}

		if(!MergeAndroidBridge.HasPermission(AndroidPermission.RECORD_AUDIO)){
			Debug.Log("Expected audio recording permissions");
			return;
		}
#endif
		if (!isInitialized)
			return;

		if (!isRecording) {
			StartCoroutine (StartRecAction (outputName, OnRecordingComplete));
		}
	}

	private bool userGrantDone;
	private bool userGrantResult;

	private IEnumerator StartRecAction (string outputName, Callback OnRecordingComplete)
	{
		//		Debug.LogWarning ("Start Recording");
#if UNITY_ANDROID && !UNITY_EDITOR
		userGrantDone = false;
		userGrantResult = false;
		MergeAndroidBridge.StartRecording();
		yield return new WaitUntil (() => userGrantDone);
		if (!userGrantResult) {			
			yield break;
		}
#endif

		//		Debug.LogWarning ("Remove Buttons");
		MergeCubeSDK.instance.RemoveMenuElement (MergeCubeSDK.instance.viewSwitchButton);
		MergeCubeSDK.instance.RemoveMenuElement (MergeCubeSDK.instance.headsetCompatabilityButton);
		//		Debug.LogWarning ("Remove Buttons Done");

		MergeCubeScreenRotateManager.instance.LockToCurrentOrientation ();
		SetRecordingTexture ();
		//		Debug.LogWarning ("Set Texture Done");


#if UNITY_IOS && !UNITY_EDITOR
//	Debug.LogWarning ("Start VidCap");
	vidCapManager.BeginRecordingSession(outputName, recordingTexture.width, recordingTexture.height, 30f, iVidCapPro.CaptureAudio.Audio, iVidCapPro.CaptureFramerateLock.Unlocked);
#endif

		if (OnRecordingComplete != null) {
			OnRecordingSaveComplete = OnRecordingComplete;
		}

		//		Debug.Log("Now recording");

		isRecording = true;

		HandleRecStartSetup ();
		//		Debug.LogWarning ("All Recording Start Action Done");
		yield return null;
	}

	public void StartSuccessHandle (bool isSuccess)
	{
		//		if (!isSuccess) {
		//			StopRec (true);
		//		}
		userGrantResult = isSuccess;
		userGrantDone = true;
	}


	//Remap: low2 + (value - low1) * (high2 - low2) / (high1 - low1)
	// targetAspect.x * (1920 / targetAspect.x)
	//recordingTexture = ( targetAspect.x / ( targetAspect.x / 1920 ) )
	private void SetRecordingTexture ()
	{
		if (Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight) {
#if UNITY_IOS || UNITY_EDITOR
			if (MergeCube.MergeCubeSDK.instance.viewMode == MergeCubeBase.ViewMode.HEADSET) {
				recordingTexture = MergeCube.MergeCubeSDK.instance.GetHeadsetTexture ();
				//				Debug.Log( "E: " + recordingTexture.width + " X " + recordingTexture.height );
			} else {
				if (targetAspect.x <= 1920) {
					//					Debug.Log( "A: " + ( int )targetAspect.x + " x " + ( int )targetAspect.y );
					recordingTexture = new RenderTexture ((int)targetAspect.x, (int)targetAspect.y, 24, RenderTextureFormat.ARGB32);
				} else {
					//					Debug.Log( "B: " + ( int )targetAspect.x / 2 + " x " + ( int )targetAspect.y / 2 );
					recordingTexture = new RenderTexture ((int)targetAspect.x / 2, (int)targetAspect.y / 2, 24, RenderTextureFormat.ARGB32);
				}

			}
			recordingTexture.Create ();
			//			#elif UNITY_ANDROID
			//			if(MergeCube.MergeCubeSDK.instance.viewMode == MergeCubeBase.ViewMode.HEADSET)
			//			{
			//				recordingTexture = MergeCube.MergeCubeSDK.instance.GetHeadsetTexture();
			//			}
#endif
		} else {
#if UNITY_IOS || UNITY_EDITOR
			if (targetAspect.x <= 1920) {
				//				Debug.Log( "C: " + ( int )targetAspect.y + " x " + ( int )targetAspect.x );
				recordingTexture = new RenderTexture ((int)targetAspect.y, (int)targetAspect.x, 24, RenderTextureFormat.ARGB32);
			} else {
				//				Debug.Log( "C: " + ( int )targetAspect.y / 2 + " x " + ( int )targetAspect.x / 2 );
				recordingTexture = new RenderTexture ((int)targetAspect.y / 2, (int)targetAspect.x / 2, 24, RenderTextureFormat.ARGB32);
			}
			recordingTexture.Create ();
			//			#elif UNITY_ANDROID
			//			recordingTexture = new RenderTexture(432, 768, 24);
#endif
		}

#if UNITY_IOS || UNITY_EDITOR
		Camera.main.targetTexture = recordingTexture;
		//		#elif UNITY_ANDROID
		//		if (MergeCube.MergeCubeSDK.instance.viewMode == MergeCubeBase.ViewMode.HEADSET) {
		//			Camera.main.targetTexture = recordingTexture;
		//		}
#endif
#if UNITY_IOS
		MergeCubeSDK.instance.videoTexture.SetTexture ("_Texture", recordingTexture);
#endif
#if UNITY_IOS || UNITY_EDITOR
		if (MergeCubeSDK.instance.viewMode == MergeCube.MergeCubeBase.ViewMode.FULLSCREEN) {
			//			Debug.Log( "FULLSCREEN!!!!" );
			fullscreenRecordingCamera.SetActive (true);
		}
#endif

#if UNITY_IOS
		//		Debug.Log("Should have set a recording texture to: " + vidCapManager);
		vidCapManager.SetCustomRenderTexture (Camera.main.targetTexture);
#endif
	}

	public ToggleSprite RecordingButton;

	public void StopRec ()
	{
		if (!isInitialized)
			return;

		//		Debug.Log("Stop Rec try");

		if (isRecording) {
			//			Debug.Log("Stop Rec -- Start Stop");
#if UNITY_ANDROID && !UNITY_EDITOR
			MergeAndroidBridge.StopRecording();
#endif
			//			Debug.Log("Stop Rec -- Native Done");
#if UNITY_IOS && !UNITY_EDITOR
			vidCapManager.EndRecordingSession(iVidCapPro.VideoDisposition.Save_Video_To_Album, out recFrames);
#endif

			if (MergeCubeSDK.instance.viewMode == MergeCube.MergeCubeBase.ViewMode.FULLSCREEN) {
				Camera.main.targetTexture = null;
				fullscreenRecordingCamera.SetActive (false);
			} else {
#if UNITY_IOS
				Camera.main.targetTexture = MergeCubeSDK.instance.GetHeadsetTexture ();
#endif
			}
			//			Debug.Log("Stop Rec -- ViewSetDone");
#if UNITY_IOS
			MergeCubeSDK.instance.videoTexture.SetTexture ("_Texture", MergeCubeSDK.instance.GetHeadsetTexture ());
#endif
			recordingTexture = null;

			MergeCubeScreenRotateManager.instance.UnlockCurrentOrientation ();
			Resources.UnloadUnusedAssets ();
			//			Debug.Log("Stop Rec -- Set Texture Done");

			if (!MergeCubeSDK.deviceIsTablet) {
				MergeCubeSDK.instance.AddMenuElement (MergeCubeSDK.instance.viewSwitchButton, 3);
				MergeCubeSDK.instance.AddMenuElement (MergeCubeSDK.instance.headsetCompatabilityButton, 4);
			}

			//			Debug.Log("Stop Rec -- Done Set Recording False");

			isRecording = false;

#if UNITY_ANDROID && !UNITY_EDITOR
			HandleRecSaveComplete();
#endif
		}
	}

	private void HandleRecStartSetup ()
	{
		Debug.Log ("HandleRecStartSetup");
		//		recordStartExample.SetActive(true);
		RecordingButton.SetState (true);
	}

	private void HandleRecSaveComplete ()
	{
		Debug.Log ("HandleRecSaveComplete");

		CancelInvoke ("ResetBusyStatus");
		Invoke ("ResetBusyStatus", 1.5f);

		RecordingButton.SetState (false);

		//Handle External Saving complete calls here
		if (OnRecordingSaveComplete != null) {
			OnRecordingSaveComplete.Invoke ();
			OnRecordingSaveComplete = null;
		}

		Resources.UnloadUnusedAssets ();
	}

#endif

	//	public TimerUI timer;
	private bool isBusy;

	public void ToggleRecording ()
	{
#if SHOULD_USE_RECORDING_PLUGIN

		if (isBusy) {
			return;
		}

		CancelInvoke ("ResetBusyStatus");
		//		CancelInvoke( "ToggleRecording" );

		if (!isRecording) {
#if UNITY_IOS && !UNITY_EDITOR
			if(MergeIOSBridge.CheckPhoto() == 2){
				MergeIOSBridge.RequestPhoto();
				return;
			}
			else if(MergeIOSBridge.CheckPhoto() != 1){
				MergeIOSBridge.OpenPhotoSettings();
				return;
			}
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			//JON:
			if(!MergeAndroidBridge.HasPermission(AndroidPermission.WRITE_EXTERNAL_STORAGE )){
				Debug.Log("Expects external storage permission");
				MergeAndroidBridge.CheckPermissionAndReDirectToSettingsScreen(AndroidPermission.READ_EXTERNAL_STORAGE);
				return;
			}
			if(!MergeAndroidBridge.HasPermission(AndroidPermission.RECORD_AUDIO)){
				Debug.Log("Expects record audio permission");
				MergeAndroidBridge.CheckPermissionAndReDirectToSettingsScreen(AndroidPermission.RECORD_AUDIO);
				return;
			}
#endif

			StartRec (System.DateTime.Now.Day.ToString () + "_" + System.DateTime.Now.Month.ToString () + "_" +
				System.DateTime.Now.Year.ToString () + "_" + System.DateTime.Now.Hour.ToString () + "_" + System.DateTime.Now.Minute.ToString (), null);

			//			RecordingButton.gameObject.SetActive( false );
			//			timer.StartTimer();
			isBusy = true;

			Invoke ("ResetBusyStatus", 3f);

			//Stop recording after 30 seconds if still running.
			//			Invoke( "ToggleRecording", 30f );
		} else {
			//			Debug.LogWarning("Should Stop Recording.");

			//			RecordingButton.gameObject.SetActive( true );
			//			timer.StopTimer();
			isBusy = true;
			StopRec ();

			//			Debug.LogWarning("Should Set Recording to False.");
		}

#endif
	}

	private void ResetBusyStatus ()
	{
		CancelInvoke ("ResetBusyStatus");
		isBusy = false;
	}
}
