using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GazeToggleSprite : MonoBehaviour, GazeResponder
{
	public Sprite onSprite;
	public Sprite offSprite;
	public bool isOn;

	public UnityEvent OnGazeStart;
	public UnityEvent OnGazeEnd;
	public UnityEvent OnGazeInput;
	public UnityEvent OnGazeInputEnd;

	public void OnGazeEnter()
	{
		OnGazeStart.Invoke();
	}

	public void OnGazeExit()
	{
		OnGazeEnd.Invoke();
	}

	public void OnGazeTrigger()
	{
		OnGazeInput.Invoke();
		ToggleSpriteVisuals();
	}

	public void OnGazeTriggerEnd()
	{
		OnGazeInputEnd.Invoke();
	}

	private void ToggleSpriteVisuals()
	{
		GetComponent<Image>().sprite = (isOn) ? offSprite : onSprite;
		isOn = !isOn;
	}
}
