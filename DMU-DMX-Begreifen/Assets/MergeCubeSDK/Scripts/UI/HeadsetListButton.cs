using UnityEngine;
using UnityEngine.UI;
using MergeCube;

public class HeadsetListButton : MonoBehaviour
{
	public string name;
	public Image backPanel;
	public Texture2D thumbnail;
	public bool isSupported;

	public HeadsetsConfigurationData configurationData;
	public bool configDataIsReady;
	public int priority;
	private Renderer rend;

	private void Start()
	{
		RawImage raw = GetComponent<RawImage>();
		raw.texture = thumbnail;
	}

	public void Initialize(HeadsetsButtonData headsetData)
	{
		thumbnail = new Texture2D( 2, 2 );

		name = headsetData.name;
		isSupported = headsetData.isSupported;
		thumbnail.LoadImage( headsetData.thumb );

		if ( !isSupported )
		{
			priority = 1;
		}
	}

	public void SelectHeadset()
	{
		if ( isSupported )
		{
			Debug.Log( "Fetching headset data and adjusting lens" );

			if ( !configDataIsReady )
			{
				FetchConfigData();
			}
			else
			{
				AdjustLensAndCloseMenu();
			}
		}
		else
		{
			Debug.LogError( "Selected Headset is not supported!" );
			//Error out to user. Not Supp. buy a merge headset.
		}
	}

	private void FetchConfigData()
	{
		if ( HeadsetCompatibilityCore.instance.CheckConf( name ) )
		{
			configurationData = HeadsetCompatibilityCore.instance.GetConf( name );
			configDataIsReady = true;
			AdjustLensAndCloseMenu();
		}
		else
		{
			ExpandingMenuManager.instance.RemoveDeadButton( this );
		}
	}

	private void AdjustLensAndCloseMenu()
	{
		if ( configDataIsReady && configurationData != null )
		{
			LensGenerator.instance.AdjustLens( configurationData );
			ExpandingMenuManager.instance.SetCurrentHeadset( this );
		}
		else
		{
			Debug.LogError( "CONFIG NOT FOUND: " + configDataIsReady + " : " + configurationData );
			//Error, no configuration file found.
		}
	}
}
