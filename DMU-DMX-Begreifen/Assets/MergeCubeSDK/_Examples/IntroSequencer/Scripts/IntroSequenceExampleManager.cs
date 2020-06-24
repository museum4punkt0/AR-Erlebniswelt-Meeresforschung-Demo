using UnityEngine;

public class IntroSequenceExampleManager : MonoBehaviour
{
	//===============================================================================================================================================================================
	//IntroSequenceExampleManager
	//In this example, we have this manager maintaining the game's flow. It will execute any scene setup that needs to happen it will give flow control over to the IntroSequencer.
	//Once the intro sequencer is done, this manager is given back the flow control.

	//We also have an example of how you can fine tune control when your objects show up when the trackable is found and lost.
	//===============================================================================================================================================================================

	public GameObject trackedObjects;

	private void Start()
	{
		//When the intro sequencer is complete, it will call our void OnIntroDone() method
		IntroSequencer.instance.OnIntroSequenceComplete += OnIntroDone;
		IntroSequencer.instance.StartIntroSequencer();

		//Initial Setup.
		trackedObjects.SetActive( false );
	}

	private void OnIntroDone()
	{
		IntroSequencer.instance.OnIntroSequenceComplete -= OnIntroDone;

		//During the IntroSequence, we may want to have the tracker behave differently. 
		//So instead, we will just listen to the tracking events and set the tracking behaviour on the MergeMultiTarget to "Do Nothing"
		//This will allow us to control when objects appear or disappear during the TrackingFound and Lost events.
		MergeMultiTarget.instance.OnTrackingFound += OnTrackingFound;
		MergeMultiTarget.instance.OnTrackingLost += OnTrackingLost;

		//When we finish the intro sequence, we may aleady be tracking. In this case this means that we have already missed the trackable event. 
		//So we should do a quick check to see if it is currently tracking and then call the appropriate handler.
		if ( MergeMultiTarget.instance.isTracking )
		{
			OnTrackingFound();
		}

		ContinueGameLogic();
	}

	private void ContinueGameLogic()
	{
		Debug.Log( "Hello World. The intro sequence is done and the game may start!" );
	}

	private void OnTrackingFound()
	{
		//This will only activate our objects if the intro sequence is done. 
		//This setup will prevent our objects from showing up during the tutorial if it is chosen to be played

		//To see how this looks when it is incorrect, change the MergeMultiTarget's behaviour mode to "Hide Child" instead of "Do Nothing"
		//Then the trackable will try to assume when the objects should show up, and will incorrectly enable the objects during the tutorial sequence.
		trackedObjects.SetActive( true );
	}

	private void OnTrackingLost()
	{
		trackedObjects.SetActive( false );
	}

}
