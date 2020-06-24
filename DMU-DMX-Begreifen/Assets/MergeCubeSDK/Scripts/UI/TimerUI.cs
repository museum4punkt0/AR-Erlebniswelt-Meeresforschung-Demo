using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
	public Transform timerComponents;
	public Image progress;
	public Text counter;
	private bool timerShouldContinue;

	public float timerDuration;

	private Callback cb;
	//Potential timing issues? On same frame start stop?

	public void StartTimer(Callback callback = null)
	{
		if ( callback != null )
		{
			cb = callback;
		}

		timerComponents.gameObject.SetActive( true );
		timerShouldContinue = true;
		StartCoroutine( BeginTimer() );
	}

	public void StopTimer()
	{
		timerShouldContinue = false;
	}

	public IEnumerator BeginTimer()
	{
		float elapsedDuration = 0;

		while ( elapsedDuration < timerDuration && timerShouldContinue )
		{
			counter.text = ( ( int )( timerDuration - elapsedDuration ) ).ToString();
			progress.fillAmount = Mathf.Clamp01( elapsedDuration / timerDuration );
			elapsedDuration += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		OnTimerComplete();
	}

	private void OnTimerComplete()
	{
		timerComponents.gameObject.SetActive( false );
		timerShouldContinue = false;
		counter.text = "30";
		progress.fillAmount = 0f;

		if ( cb != null )
		{
			cb.Invoke();
			cb = null;
		}
	}
}
