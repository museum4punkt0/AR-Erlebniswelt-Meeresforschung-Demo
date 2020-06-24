using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor( typeof( MergeCube.MergeCubeSDK ) )]
public class MergeCubeSDKInspector : Editor
{
	private static readonly string[] _dontIncludeMe = new string[]{ "m_Script" };

	SerializedProperty configurationFile;
	SerializedProperty cubeConfiguration;
	SerializedProperty isUsingMergeReticle;

	void OnEnable()
	{
		configurationFile = serializedObject.FindProperty( "confFile" );
		isUsingMergeReticle = serializedObject.FindProperty( "isUsingMergeReticle" );
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField( configurationFile, new GUIContent( "Merge Configuration File:" ) );
		EditorGUILayout.PropertyField( isUsingMergeReticle );
		EditorGUILayout.Space();

		serializedObject.ApplyModifiedProperties();
	}
}
