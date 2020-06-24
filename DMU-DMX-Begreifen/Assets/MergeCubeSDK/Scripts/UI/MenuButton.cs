using UnityEngine;

[System.Serializable]
public class MenuButton 
{
	public Transform button;
	public bool useFixedPosition;
	public int targetPosition;

	public MenuButton( Transform buttonTransform, int targetPos, bool shouldUseFixedPosition = false )
	{
		button = buttonTransform;
		targetPosition = targetPos;
		useFixedPosition = shouldUseFixedPosition;
	}
}
