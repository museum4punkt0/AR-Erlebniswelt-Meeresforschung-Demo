using UnityEngine;
using Vuforia;

public class MergeTrackableEventHandler : MonoBehaviour, ITrackableEventHandler {
	private TrackableBehaviour mTrackableBehaviour;
	private bool isTracking;
	private bool isCompeating = true;
	private float timeCount;
	private void Start(){ MergeMultiTarget.instance.AddMergeTrackable (this); isTracking = false; mTrackableBehaviour = GetComponent<TrackableBehaviour> (); if (mTrackableBehaviour) { mTrackableBehaviour.RegisterTrackableEventHandler (this); } }
	private void Update(){ if (isTracking && isCompeating) { timeCount += Time.deltaTime; if (timeCount > 10f) { isCompeating = false; MergeMultiTarget.instance.LockToTrackable (this); } } }
	public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus ){
		if (newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED || newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {
			isTracking = true;
			MergeMultiTarget.instance.OnMergeTrackingFound (this);
		} else {
			isTracking = false;
			MergeMultiTarget.instance.OnMergeTrackingLost (this);
		}
	}
	public void Die(){
		isCompeating = false;
		GetComponent<MultiTargetBehaviour>().enabled = false;
		gameObject.SetActive (false);
		Destroy (this);
	}
}
