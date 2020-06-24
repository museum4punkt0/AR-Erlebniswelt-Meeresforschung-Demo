using UnityEngine;

/**
 * How To Use:
 * Attach this script to the ImageTarget. Then register to the OnRotationChange event to listen for changes in rotation.
 * This event call will happen every update.
 * 
 * The InputRelativeRotation script is useful for getting the relative rotation of the image target compared to the head's rotation.
 **/

public class InputRelativeRotation : MonoBehaviour 
{
	public delegate void RotationEvent( Vector3 deltaRotation );
	public RotationEvent OnRotationChange;

	private Transform imageTargetTransform;
	private Transform headTransform;
	private Transform rotationTracker;
	private bool isTracking;

	private void Start()
	{
		imageTargetTransform = new GameObject ().transform;
		imageTargetTransform.name = "RelativeRotation_Tracker_InCube";
		imageTargetTransform.parent = transform;
		imageTargetTransform.localPosition = Vector3.zero;
		imageTargetTransform.localRotation = Quaternion.identity;
		imageTargetTransform.localScale = Vector3.one;

		headTransform = new GameObject ().transform;
		headTransform.name = "RelativeRotation_Tracker_WorldRoot";
		headTransform.position = Camera.main.transform.position;
		headTransform.localScale = Vector3.one;


		rotationTracker = new GameObject ().transform;
		rotationTracker.name = "RelativeRotation_Tracker_InWorld";
		rotationTracker.parent = headTransform;
		rotationTracker.localPosition = Vector3.zero;
		rotationTracker.localRotation = Quaternion.identity;
		rotationTracker.localScale = Vector3.one;

		GetComponent<MergeMultiTarget>().OnTrackingFound += OnTrackingFound;
		GetComponent<MergeMultiTarget>().OnTrackingLost += OnTrackingLost;
	}

	private void OnTrackingFound()
	{
		headTransform.LookAt (imageTargetTransform.position);
		imageTargetTransform.LookAt (headTransform.position);
		isTracking = true;
	}

	private void OnTrackingLost()
	{
		isTracking = false;
	}

	private void Update()
	{	
		if (isTracking) 
		{
			TrackRotation ();
		}
	}

	private void TrackRotation()
	{
		headTransform.LookAt (imageTargetTransform.position);
		rotationTracker.rotation = imageTargetTransform.rotation;
		Vector3 deltaRotation = rotationTracker.localEulerAngles;
		imageTargetTransform.LookAt (headTransform.position);

		if (Mathf.Abs (deltaRotation.x) < .5f) 
		{
			deltaRotation.x = 0;
		}

		if (Mathf.Abs (deltaRotation.z) < .5f) 
		{
			deltaRotation.z = 0;
		}

		if (Mathf.Abs (deltaRotation.y) > 0) 
		{
			deltaRotation.y = deltaRotation.y-180f;
		} 
		else 
		{
			deltaRotation.y = 180f + deltaRotation.y;
		}

		if (Mathf.Abs (deltaRotation.y) < .5f) 
		{
			deltaRotation.y = 0;
		}

		if (OnRotationChange != null)
		{
			OnRotationChange.Invoke(deltaRotation);
		}
	}
}
