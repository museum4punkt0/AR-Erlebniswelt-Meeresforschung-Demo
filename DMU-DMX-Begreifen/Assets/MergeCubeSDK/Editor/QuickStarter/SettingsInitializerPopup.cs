using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SettingsInitializerPopup : EditorWindow
{
	
	//	public static Callback OnChangeBuildPlatformComplete;

	//	[MenuItem( "Merge/Platform Swapper" )]
	[MenuItem( "Merge/Settings Initializer" )]
	public static void LaunchSettingsInitializer()
	{
		SettingsInitializerPopup window = ( SettingsInitializerPopup )EditorWindow.GetWindow( typeof( SettingsInitializerPopup ) );
		window.Show();
	}

	void OnGUI()
	{
		GUIStyle titleStyle = new GUIStyle( EditorStyles.centeredGreyMiniLabel );
		titleStyle.normal.textColor = Color.grey;
		titleStyle.fontSize = 12;
		titleStyle.fontStyle = FontStyle.Bold;

//		GUILayout.Label( "MergeCubeSDK Platform Swapper:", titleStyle );
		GUILayout.Label( "MergeCubeSDK Settings Helper:", titleStyle );

		GUILayout.Space( 5 );
		GUILayout.Label( "This tool will initialize the appropriate device settings.", EditorStyles.centeredGreyMiniLabel );
		GUILayout.Space( 10 );

		GUILayout.Space( 5 );
//		GUILayout.Label( "Change Build Platform?", titleStyle );
		GUILayout.Label( "Set build settings to default values?", titleStyle );
		GUILayout.Space( 10 );

		if ( GUILayout.Button( "Yes" ) )
		{
			InitializeDeviceSettings();
			this.Close();
		}

//		if ( GUILayout.Button( "iOS" ) )
//		{
//			PlatformSwapWatcher.OnPlatformSwapComplete += HandleSwapComplete;
//			EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTargetGroup.iOS, BuildTarget.iOS );
//
//			this.Close();
//		}
//
//		GUILayout.Space( 5 );
//
//		if ( GUILayout.Button( "Android" ) )
//		{
//			PlatformSwapWatcher.OnPlatformSwapComplete += HandleSwapComplete;
//			EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTargetGroup.Android, BuildTarget.Android );
//
//			this.Close();
//		}

		GUILayout.Space( 5 );

		if ( GUILayout.Button( "Cancel" ) )
		{
			this.Close();
		}
	}

	//	[MenuItem( "Merge/HardResePlatformWatchert" )]
	//	public static void HardReset()
	//	{
	//		PlatformSwapWatcher.OnPlatformSwapComplete = null;
	//		Debug.Log( "Reset OnPlatformSwapComplete registry" );
	//	}


	void InitializeDeviceSettings()
	{
//		PlatformSwapWatcher.OnPlatformSwapComplete -= HandleSwapComplete;
//
//		//Setup Build Settings

		UnityEngine.Rendering.GraphicsDeviceType[] supportedAPIs = new UnityEngine.Rendering.GraphicsDeviceType[]{ UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2 };

//		if ( EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android )
//		{
		PlayerSettings.SetPlatformVuforiaEnabled( BuildTargetGroup.Android, true );
//			UnityEngine.Rendering.GraphicsDeviceType[] supportedAPIs = new UnityEngine.Rendering.GraphicsDeviceType[]{ UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2 };
		PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;
		PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
		PlayerSettings.Android.androidIsGame = false;
		PlayerSettings.Android.androidTVCompatibility = false;
		PlayerSettings.SetUseDefaultGraphicsAPIs( BuildTarget.Android, false );
		PlayerSettings.SetGraphicsAPIs( BuildTarget.Android, supportedAPIs );
		PlayerSettings.Android.forceSDCardPermission = true;

		//Later functionality:
		//Auto move Android Manifest file if there isn't an existing one. Warn user if one already exists and inform them on how to manually apply it.

//		}
//		else if ( EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS )
//		{
		PlayerSettings.SetPlatformVuforiaEnabled( BuildTargetGroup.iOS, true );
//			UnityEngine.Rendering.GraphicsDeviceType[] supportedAPIs = new UnityEngine.Rendering.GraphicsDeviceType[]{ UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2 };
		PlayerSettings.iOS.cameraUsageDescription = "Used For Augmented Reality";
		PlayerSettings.SetUseDefaultGraphicsAPIs( BuildTarget.iOS, false );
		PlayerSettings.SetGraphicsAPIs( BuildTarget.iOS, supportedAPIs );
		PlayerSettings.iOS.targetOSVersionString = "9.0";
//		}
//		else
//		{
//			Debug.LogError( EditorUserBuildSettings.activeBuildTarget );
//		}
//			
//		if ( OnChangeBuildPlatformComplete != null )
//		{
//			OnChangeBuildPlatformComplete.Invoke();
//			OnChangeBuildPlatformComplete = null;
//		}
	}
}

//public class PlatformSwapWatcher : UnityEditor.Build.IActiveBuildTargetChanged
//{
//	public static Callback OnPlatformSwapComplete;
//
//	public int callbackOrder { get { return 0; } }
//
//	public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
//	{
//		if ( OnPlatformSwapComplete != null )
//		{
//			OnPlatformSwapComplete.Invoke();
//		}
//	}
//}

//public class AssetPostProcessorHandler : AssetPostprocessor
//{
//	public static Callback OnPostProcessComplete;
//
//	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//	{
//
//		Debug.Log( "Unity Asset Import complete" );
//
//		if ( OnPostProcessComplete != null )
//		{
//			Debug.Log( "Executing asset process callbacks" );
//			OnPostProcessComplete.Invoke();
//		}
//	}
//}

