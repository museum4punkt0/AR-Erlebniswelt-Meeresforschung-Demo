using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class MergeSDKUpdateHelper : EditorWindow
{
	public string[] androidFilesToMove = new string[]
	{ 
		"Assets/Plugins/Android/MergeAndroidNative.jar" 
	};

	public string[] iOSFilesToMove = new string[]
	{
		"Assets/Plugins/iOS/libiVidCapPro.a", 
		"Assets/Plugins/iOS/MergeIOSNative.h", 
		"Assets/Plugins/iOS/MergeIOSNative.m"

	};

	public string[] iVidCapProFilesToMove = new string[]
	{
		"Assets/Plugins/iVidCapPro/GPUImage_License.txt", 
		"Assets/Plugins/iVidCapPro/iVidCapPro_License.txt", 
		"Assets/Plugins/iVidCapPro/iVidCapPro_SampleGUISkin.guiskin", 
		"Assets/Plugins/iVidCapPro/iVidCapPro_SampleManager.cs",
		"Assets/Plugins/iVidCapPro/iVidCapPro.cs",
		"Assets/Plugins/iVidCapPro/iVidCapProAudio.cs",
		"Assets/Plugins/iVidCapPro/iVidCapProEdit.cs",
		"Assets/Plugins/iVidCapPro/iVidCapProVideo.cs",
		"Assets/Plugins/iVidCapPro/iVidCapPro_Doc_1_7.zip",
		"Assets/Plugins/iVidCapPro/xirod.ttf"
	};

	public string[] filesToDelete = new string[]
	{
		"Assets/Plugins/Android/InAppBrowser.jar", 
		"Assets/Plugins/iOS/InAppBrowserViewController.h", 
		"Assets/Plugins/iOS/InAppBrowserViewController.mm",
		"Assets/Plugins/iOS/iOSInAppBrowser.mm",
		"Assets/Plugins/iVidCapPro"
	};

	public string[] vuforiaFilesToDelete = new string[]
	{
		"Assets/Vuforia", 
		"Assets/Plugins/Android/Vuforia.jar", 
		"Assets/Plugins/Android/VuforiaUnityPlayer.jar",
		"Assets/Plugins/Android/libs/armeabi-v7a/libVuforia.so",

		"Assets/Plugins/Android/libs/armeabi-v7a/libVuforiaUnityPlayer.so",
		"Assets/Plugins/Android/libs/armeabi-v7a/libVuforiaWrapper.so",
		"Assets/Plugins/Android/src/com/Vuforia",
		"Assets/Plugins/iOS/libVuforia.a",

		"Assets/Plugins/iOS/libVuforiaUnityPlayer.a",
		"Assets/Plugins/iOS/Vuforia.UnityExtensions.iOS.dll",
		"Assets/Plugins/iOS/Vuforia.UnityExtensions.iOS.xml",
		"Assets/Plugins/iOS/VuforiaNativeRendererController.mm",

		"Assets/Plugins/iOS/VuforiaRenderDelegate.h",
		"Assets/Plugins/iOS/VuforiaRenderDelegate.mm",
		"Assets/Plugins/iOS/VuforiaUnityPlayer.h",
		"Assets/Plugins/VuforiaWrapper.bundle",

		"Assets/Plugins/WSA/x64/Vuforia.dll",
		"Assets/Plugins/WSA/x64/VuforiaWrapper.dll",
		"Assets/Plugins/WSA/x86/Vuforia.dll",
		"Assets/Plugins/WSA/x86/VuforiaWrapper.dll",

		"Assets/Plugins/x64/VuforiaWrapper.dll",
		"Assets/Plugins/x64/VuforiaWrapper.dll.signature",
		"Assets/Plugins/x64/VuforiaWrapper.exp",
		"Assets/Plugins/x64/VuforiaWrapper.lib",

		"Assets/Plugins/x86/VuforiaWrapper.dll",
		"Assets/Plugins/x86/VuforiaWrapper.dll.signature",
		"Assets/Plugins/x86/VuforiaWrapper.exp",
		"Assets/Plugins/x86/VuforiaWrapper.lib",

		"Assets/Plugins/Editor/Unzip.js"
	};

	[MenuItem( "Merge/MergeCubeSDK Update Helper" )]
	static void Init()
	{
		MergeSDKUpdateHelper window = ( MergeSDKUpdateHelper )EditorWindow.GetWindow( typeof( MergeSDKUpdateHelper ) );
		window.Show();
	}

	void OnGUI()
	{

		GUIStyle titleStyle = new GUIStyle( EditorStyles.centeredGreyMiniLabel );
		titleStyle.normal.textColor = Color.grey;
		titleStyle.fontSize = 12;
		titleStyle.fontStyle = FontStyle.Bold;


		GUILayout.Label( "MergeCubeSDK Updater :", titleStyle );

		GUILayout.Space( 5 );
		GUILayout.Label( "This tool will move the old MergeCubeSDK items from the root plugins folder to the MergeCubeSDK folder to help maintain asset file organization.", EditorStyles.wordWrappedLabel );
		GUILayout.Space( 10 );

		if ( GUILayout.Button( "Clean MergeCubeSDK" ) )
		{
			if ( AssetDatabase.IsValidFolder( "Assets/MergeCubeSDK" ) )
			{
				if ( !AssetDatabase.IsValidFolder( "Assets/MergeCubeSDK/Plugins" ) )
				{
					AssetDatabase.CreateFolder( "Assets/MergeCubeSDK", "Plugins" );
					AssetDatabase.CreateFolder( "Assets/MergeCubeSDK/Plugins", "Android" );
					AssetDatabase.CreateFolder( "Assets/MergeCubeSDK/Plugins", "iOS" );
					AssetDatabase.CreateFolder( "Assets/MergeCubeSDK/Plugins", "iVidCapPro" );
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
			}
			else
			{
				Debug.LogError( "MergeCubeSDK Folder not found. Please ensure it has been imported and exists in the Asset root: Assets/MergeCubeSDK/" );
			}

			for ( int index = 0; index < androidFilesToMove.Length; index++ )
			{
				AssetDatabase.MoveAsset( androidFilesToMove[ index ], androidFilesToMove[ index ].Replace( "Assets/Plugins/Android/", "Assets/MergeCubeSDK/Plugins/Android/" ) );
			}

			for ( int index = 0; index < iOSFilesToMove.Length; index++ )
			{
				AssetDatabase.MoveAsset( iOSFilesToMove[ index ], iOSFilesToMove[ index ].Replace( "Assets/Plugins/iOS/", "Assets/MergeCubeSDK/Plugins/iOS/" ) );
			}

			for ( int index = 0; index < iVidCapProFilesToMove.Length; index++ )
			{
				AssetDatabase.MoveAsset( iVidCapProFilesToMove[ index ], iVidCapProFilesToMove[ index ].Replace( "Assets/Plugins/iVidCapPro/", "Assets/MergeCubeSDK/Plugins/iVidCapPro/" ) );
			}

			for ( int index = 0; index < filesToDelete.Length; index++ )
			{
				AssetDatabase.DeleteAsset( filesToDelete[ index ] );
			}

			RemoveEmptyFolders( "Assets/Plugins" );

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		GUILayout.Space( 30 );

		GUILayout.Label( "EXPERIMENTAL : Vuforia 2017.2 Updater :", titleStyle );
		GUILayout.Label( "This will delete the 2017.1 and older Vuforia files so you can cleanly import the new Vuforia files via the internal importer.", EditorStyles.wordWrappedLabel );
		GUILayout.Label( "To do this manually, please refer to the documentation provided by Vuforia:\n\nhttps://library.vuforia.com/content/vuforia-library/en/articles/Solution/migrate-vuforia-62-to-65.html", EditorStyles.wordWrappedLabel );
		GUILayout.Label( "USE AT YOUR OWN RISK", titleStyle );
		GUILayout.Space( 10 );
		if ( GUILayout.Button( "Strip Vuforia" ) )
		{
			for ( int index = 0; index < vuforiaFilesToDelete.Length; index++ )
			{
				AssetDatabase.DeleteAsset( vuforiaFilesToDelete[ index ] );
			}

			RemoveEmptyFolders( "Assets/Plugins" );

		}

		GUILayout.Space( 15 );

		if ( GUILayout.Button( "Cancel" ) )
		{
			this.Close();
		}
	}

	void RemoveEmptyFolders(string path)
	{
		string[] subDirectories = Directory.GetDirectories( path );
		foreach ( string subtp in subDirectories )
		{
			RemoveEmptyFolders( subtp );
		}
		string[] subFiles = Directory.GetFiles( path );
		if ( subFiles.Length == 0 )
		{
			AssetDatabase.DeleteAsset( path );
		}
		return;
	}
}
