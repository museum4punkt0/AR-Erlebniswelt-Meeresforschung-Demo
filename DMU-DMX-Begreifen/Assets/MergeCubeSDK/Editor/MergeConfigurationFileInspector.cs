using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MergeConfigurationFile))]
public class MergeConfigurationFileInspector : Editor 
{
	private static readonly string[] _dontIncludeMe = new string[]{"m_Script"};

	SerializedProperty soLicenseKey;
	SerializedProperty soScaleFactor;

	SerializedProperty soCustomScale;
	SerializedProperty soChosenScale;

	void OnEnable()
	{
		soLicenseKey =  serializedObject.FindProperty("licenseKey");
		soScaleFactor = serializedObject.FindProperty("scaleFactor");

		soCustomScale = serializedObject.FindProperty("customScaleFactor");
		soChosenScale = serializedObject.FindProperty("chosenScaleFactor");

	}

	public override void OnInspectorGUI ()
	{ 
		serializedObject.Update ();

		EditorStyles.textField.wordWrap = true;
		EditorGUILayout.PropertyField( soLicenseKey, new GUIContent("Merge Development Key:"), GUILayout.MinHeight(80) );
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Cube Scale Factor:");

		switch (soScaleFactor.enumValueIndex)
		{
			default:
			case (int)MergeConfigurationFile.ScaleFactor.ONE_METER:
				EditorGUILayout.HelpBox("The cube in Unity will be scaled to 1 meter size.",MessageType.Info);
				EditorGUILayout.PropertyField( soScaleFactor );
				soChosenScale.floatValue = MergeConfigurationFile.oneScale;
				break;
			case (int)MergeConfigurationFile.ScaleFactor.PHYS_SIZE:
				EditorGUILayout.HelpBox("The cube in Unity will be scaled to 0.072 meters in size. This is the cubes actual dimensions in reality.",MessageType.Info);
				EditorGUILayout.PropertyField( soScaleFactor );
				soChosenScale.floatValue = MergeConfigurationFile.realScale;
				break;
			case (int)MergeConfigurationFile.ScaleFactor.CUSTOM:
				EditorGUILayout.HelpBox("The cube in Unity will be scaled to a user defined size in meters.",MessageType.Info);
				EditorGUILayout.PropertyField( soScaleFactor );
				EditorGUILayout.PropertyField(soCustomScale);
				soChosenScale.floatValue = (float)soCustomScale.intValue;
				break;
		}

//		EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
//		EditorGUILayout.Space();

		//Debug:
//		EditorGUILayout.LabelField(new GUIContent("DEBUG-ChosenScaleFactor:"), new GUIContent(soChosenScale.floatValue.ToString()));

		serializedObject.ApplyModifiedProperties ();
	}
}