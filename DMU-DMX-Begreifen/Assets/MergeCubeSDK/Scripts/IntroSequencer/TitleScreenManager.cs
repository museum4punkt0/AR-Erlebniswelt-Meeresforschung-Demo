using UnityEngine;
using UnityEngine.UI;
using MergeCube;

public class TitleScreenManager : MonoBehaviour
{
	public static TitleScreenManager instance;

	private void Awake()
	{
		if ( instance == null )
			instance = this;
		else if ( instance != this )
			DestroyImmediate( this.gameObject );

		titleScreen.SetActive( false );
	}

	public GameObject titleScreen;
	public Transform mergeModeButton;
	public Transform mobileModeButton;
	public Sprite mergeModeDefaultSprite;
	public Sprite mergeModeDisabledSprite;


	public delegate void CallbackBoo( bool boo );

	public CallbackBoo OnTitleSequenceComplete;

	//Entry Method
	public void ShowTitleScreen()
	{
		
		if ( MergeCubeSDK.deviceIsTablet )
		{
			DisableMergeMode();
		}

		titleScreen.SetActive( true );
	}

	//State Management
	public void DisableMergeMode()
	{
		//Show disabled graphics for MergeMode Button
		mergeModeButton.GetComponent<Image>().sprite = mergeModeDisabledSprite;
		mergeModeButton.GetComponent<Button>().interactable = false;
	}

	//Exit Conditions called by GUI elements
	public void PlayInMergeMode()
	{
		bool shouldSwitch = false;
		titleScreen.SetActive( false );

//		Debug.Log(MergeCubeSDK.instance.viewMode);
		if ( MergeCubeSDK.instance.viewMode != MergeCubeSDK.ViewMode.HEADSET )
		{
			shouldSwitch = true;
		}

		if ( OnTitleSequenceComplete != null )
		{
			OnTitleSequenceComplete.Invoke( shouldSwitch );
		}
		UpdateTutorialSetting();
	}

	public void PlayInMobileMode()
	{
		bool shouldSwitch = false;
		titleScreen.SetActive( false );

//		Debug.Log(MergeCubeSDK.instance.viewMode);
		if ( MergeCubeSDK.instance.viewMode != MergeCubeSDK.ViewMode.FULLSCREEN )
		{
			shouldSwitch = true;
		}

		if ( OnTitleSequenceComplete != null )
		{
			OnTitleSequenceComplete.Invoke( shouldSwitch );
		}
		UpdateTutorialSetting();
	}

	private void UpdateTutorialSetting()
	{
		if ( !PlayerPrefs.HasKey( "HasPlayedBefore" ) )
		{
			PlayerPrefs.SetString( "HasPlayedBefore", "true" );
		}
	}

	//	public GameObject mergeCubePopUpPage;

	public void OpenMergeCubeUrl()
	{
		Application.OpenURL( @"https://mergecube.com/needamergecube" );
//		mergeCubePopUpPage.SetActive( true );
	}
}
