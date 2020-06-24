using UnityEngine;

public class InputHandle : MonoBehaviour
{
	public void Click()
	{
		transform.localScale = Vector3.one * 1.2f;
		CancelInvoke("SetBack");
		Invoke("SetBack", .4f);
	}

	private void SetBack()
	{
		transform.localScale = Vector3.one;
	}
	// Use this for initialization
	private void Start()
	{
		
	}
	
	// Update is called once per frame
	private void Update()
	{
		
	}
}
