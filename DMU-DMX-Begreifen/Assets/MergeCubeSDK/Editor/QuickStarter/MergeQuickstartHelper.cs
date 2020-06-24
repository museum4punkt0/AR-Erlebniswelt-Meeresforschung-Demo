using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MergeQuickstartHelper
{
	[MenuItem( "Merge/Make Quickstart Scene" )]
	public static void MakeQuickStartScene()
	{
		EditorSceneManager.newSceneCreated += CreateQuickstartScene;
		Scene newScene = EditorSceneManager.NewScene( NewSceneSetup.EmptyScene, NewSceneMode.Single );
	}

	public static void CreateQuickstartScene(Scene scene, NewSceneSetup setup, NewSceneMode mode)
	{
		EditorSceneManager.newSceneCreated -= CreateQuickstartScene;

//		if ( EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android && EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS )
//		{
//			ChangeBuildPlatformPopup.OnChangeBuildPlatformComplete = FinishSceneSetup;
//			ChangeBuildPlatformPopup window = ( ChangeBuildPlatformPopup )EditorWindow.GetWindow( typeof( ChangeBuildPlatformPopup ) );
//			window.Show();
//		}
//		else
//		{
		FinishSceneSetup();
//		}
	}

	//	public static void ImportVuforiaAssets()
	//	{
	//		Debug.Log( "Vuforia Asset Import skipped until we learn how to do it. This must be handled manually before going through quickstart" );
	//
	//		Debug.Log("Importing Vuforia Assets");
	//
	//		Vuforia.EditorClasses.VuforiaAssetImporter vuforiaAssetImporter = new Vuforia.EditorClasses.VuforiaAssetImporter();
	//		vuforiaAssetImporter.ImportOrUpdateAssets(() =>
	//			{
	//				FinishSceneSetup();
	//			});
	//	}

	public static void FinishSceneSetup()
	{
		//Lights:
		Light light = ( new GameObject( "Directional Light", typeof( Light ) ) ).GetComponent<Light>();
		light.type = LightType.Directional;
		light.gameObject.transform.localPosition = new Vector3( 0f, 3f, 0f );
		light.gameObject.transform.localRotation = Quaternion.Euler( 50f, -30f, 0f );
		light.color = new Color( 1f, 0.956f, 0.839f );
		light.shadows = LightShadows.Soft;

		//Camera:
		Vuforia.EditorClasses.GameObjectFactory.CreateARCamera();

		//Action!
		Object mcsdk = PrefabUtility.InstantiatePrefab( AssetDatabase.LoadAssetAtPath<Object>( "Assets/MergeCubeSDK/Prefab/MergeCubeSDK.prefab" ) );
		mcsdk.name = "MergeCubeSDK";

		Object multitarget = PrefabUtility.InstantiatePrefab( AssetDatabase.LoadAssetAtPath<Object>( "Assets/MergeCubeSDK/Prefab/MergeMultiTarget.prefab" ) );
		multitarget.name = "MergeMultiTarget";
	}
}