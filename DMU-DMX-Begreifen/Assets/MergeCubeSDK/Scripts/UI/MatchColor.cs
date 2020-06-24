using UnityEngine;
using UnityEngine.UI;

public class MatchColor : MonoBehaviour
{
	public Button btn;
	private Text thisText;

	private void Start()
	{
		thisText = this.GetComponent<Text>();
	}

	private void Update()
	{
		if ( btn.interactable )
		{
			
		}
	}
}
