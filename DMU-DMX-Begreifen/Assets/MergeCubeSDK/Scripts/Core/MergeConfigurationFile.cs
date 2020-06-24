using UnityEngine;

[CreateAssetMenu(fileName = "MergeConfigurationFile",menuName = "Merge/ConfigurationFile")]
public class MergeConfigurationFile : ScriptableObject 
{
	public string licenseKey;
	public enum ScaleFactor{ ONE_METER, PHYS_SIZE, CUSTOM };
	public ScaleFactor scaleFactor;
	public const float oneScale = 1f;
	public const float realScale = 0.072f;

	[Range(1,10)]
	public int customScaleFactor = 1;

	public Callback dataUpdateEvent;

	[HideInInspector]
	public float chosenScaleFactor = oneScale;


	private void OnValidate()
	{
		if (dataUpdateEvent != null)
		{
			dataUpdateEvent.Invoke();
		}
	}
}
