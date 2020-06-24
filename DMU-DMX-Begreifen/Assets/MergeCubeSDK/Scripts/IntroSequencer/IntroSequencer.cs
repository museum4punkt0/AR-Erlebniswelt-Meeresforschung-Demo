using System.Collections;
using UnityEngine;
using MergeCube;

public class IntroSequencer : MonoBehaviour
{
	public static IntroSequencer instance;
	public bool debug;

	private void Awake()
	{
		if ( debug )
		{
			PlayerPrefs.DeleteAll();
		}
		if ( instance == null )
			instance = this;
		else if ( instance != this )
		{
			DestroyImmediate( this.gameObject );
			return;
		}
	}

	//This allows the intro sequence to play out of the box with no other managers handling calling it's start.
	public bool shouldAutoStart = true;

	private bool isIntroStart;

	public Callback OnIntroSequenceComplete;


	private void Start()
	{
		MergeCubeSDK.instance.OnInitializationComplete += SignalSDKReady;

		if ( shouldAutoStart )
			StartCoroutine( WaitForSDKInit() );
	}

	private bool mergeCubeSDKReady;

	private void SignalSDKReady()
	{
		mergeCubeSDKReady = true;
	}

	public void StartIntroSequencer()
	{
		StartCoroutine( WaitForSDKInit() );
	}

	private IEnumerator WaitForSDKInit()
	{
		if ( isIntroStart )
		{
			yield break;
		}
		isIntroStart = true;
		yield return new WaitUntil( () => mergeCubeSDKReady );
		BeginSequencer();
	}

	//Entry
	private void BeginSequencer()
	{
//		Screen.autorotateToLandscapeLeft = false;
//		Screen.autorotateToLandscapeRight = false;
//		Screen.autorotateToPortrait = true;
//		Screen.autorotateToPortraitUpsideDown = false;

		MergeCubeSDK.instance.RemoveMenuElement( MergeCubeSDK.instance.viewSwitchButton );

		SplashScreenManager.instance.OnSplashSequenceEnd += HandleSplashSequenceComplete;
		TitleScreenManager.instance.OnTitleSequenceComplete += HandleTitleSequenceComplete;

		SplashScreenManager.instance.StartSplashSequence();
	}

	private void HandleSplashSequenceComplete()
	{
		TitleScreenManager.instance.ShowTitleScreen();
	}

	private void HandleTitleSequenceComplete(bool shouldSwitchModeTp)
	{
		shouldSwitchMode = shouldSwitchModeTp;
		if ( PermissionProcessor.instance != null )
		{
			PermissionProcessor.instance.permissionProcessDone += HandlePermissionProcessDone;
			PermissionProcessor.instance.StartProcess();
		}
		else
		{
			HandlePermissionProcessDone();
		}
	}

	private bool shouldSwitchMode;

	private void HandlePermissionProcessDone()
	{
		Debug.LogWarning( "Process Should Done" );
		if ( shouldSwitchMode )
		{
			MergeCubeSDK.instance.SwitchView();
		}
		else
		{
			MergeCubeScreenRotateManager.instance.SetOrientation( false );
		}

		EndIntroSequence();

	}
		
	//Exit
	private void EndIntroSequence()
	{
		if ( !MergeCubeSDK.deviceIsTablet )
		{
			MergeCubeSDK.instance.AddMenuElement( MergeCubeSDK.instance.viewSwitchButton, 3 );
		}

		if ( TrackOnce.instance != null )
		{
			TrackOnce.instance.IntroDone();
		}

		if ( OnIntroSequenceComplete != null )
		{
			OnIntroSequenceComplete.Invoke();
		}
	}
}
