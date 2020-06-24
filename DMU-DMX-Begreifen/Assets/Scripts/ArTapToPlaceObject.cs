using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArTapToPlaceObject : MonoBehaviour
{

    [Header("Objekte")]
    [SerializeField] private GameObject basalt;
    [SerializeField] private GameObject kupferZinkErz;
    [SerializeField] private GameObject methanHydrat;
    [SerializeField] private GameObject koralle;
    [SerializeField] private GameObject korallenSchwamm;
    [SerializeField] private GameObject manganKnolle;
    [SerializeField] private GameObject raucher;
    [SerializeField] private GameObject jago;

    [Header("InfoTafeln")]
    [SerializeField] private GameObject alleTafeln;
    [SerializeField] private GameObject basaltTafel;
    [SerializeField] private GameObject kupferZinkTafel;
    [SerializeField] private GameObject methanHydratTafel;
    [SerializeField] private GameObject koralleTafel;
    [SerializeField] private GameObject korallenSchwammTafel;
    [SerializeField] private GameObject manganTafel;
    [SerializeField] private GameObject raucherTafel;
    [SerializeField] private GameObject jagoTafel;

    [Header("Überschriften")]
    [SerializeField] private GameObject basaltUeberschrift;
    [SerializeField] private GameObject kupferZinkUeberschrift;
    [SerializeField] private GameObject methanHydratUeberschrift;
    [SerializeField] private GameObject koralleUeberschrift;
    [SerializeField] private GameObject korallenSchwammUeberschrift;
    [SerializeField] private GameObject manganUeberschrift;
    [SerializeField] private GameObject raucherUeberschrift;
    [SerializeField] private GameObject jagoUeberschrift;

    [Header("Inidikator")]
    [SerializeField] private GameObject placementIndicator;

    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private ARRaycastManager arRaycast;

    private List<GameObject> objectsToPlace;
    private List<GameObject> infoMenus;
    private List<GameObject> headlines;

    private GameObject objectToPlace;
    private GameObject currentMenu;
    private GameObject currentHeadline;

    private bool placementPoseIsValid;
    private GameObject current;

    private void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arRaycast = arOrigin.GetComponent<ARRaycastManager>();
        objectsToPlace = new List<GameObject>(new[] { kupferZinkErz, methanHydrat, koralle, korallenSchwamm, manganKnolle, raucher, jago });
        infoMenus = new List<GameObject>(new[] { kupferZinkTafel, methanHydratTafel, koralleTafel, korallenSchwammTafel, manganTafel, raucherTafel, jagoTafel });
        headlines = new List<GameObject>(new[] { kupferZinkUeberschrift, methanHydratUeberschrift, koralleUeberschrift, korallenSchwammUeberschrift, manganUeberschrift, raucherUeberschrift, jagoUeberschrift });
        objectToPlace = basalt;
        currentMenu = basaltTafel;
        currentHeadline = basaltUeberschrift;
    }

    private void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (isJago)
            {
                PlaceJago();
            } else
            {
                PlaceObject();
            }
            
        }
    }

    private void PlaceObject()
    {
        if (current != null)
        {
            return;
        }

        placementIndicator.SetActive(false);

        current = Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
        arOrigin.MakeContentAppearAt(current.transform, current.transform.position, current.transform.rotation);
    }

    private void PlaceJago()
    {
        if (current != null)
        {
            return;
        }

        placementIndicator.SetActive(false);

        current = Instantiate(jago, placementPose.position, placementPose.rotation);
        arOrigin.MakeContentAppearAt(current.transform, current.transform.position, current.transform.rotation);
    }

    public void Next()
    {
        GameObject temp = current;

        DestroyCurrent();

        objectsToPlace.Add(objectToPlace);
        objectToPlace = objectsToPlace[0];
        objectsToPlace.Remove(objectToPlace);

        current = Instantiate(objectToPlace, temp.transform.position, temp.transform.rotation);

        currentMenu.SetActive(false);
        infoMenus.Add(currentMenu);
        currentMenu = infoMenus[0];
        infoMenus.Remove(currentMenu);
        currentMenu.SetActive(true);

        currentHeadline.SetActive(false);
        headlines.Add(currentHeadline);
        currentHeadline = headlines[0];
        headlines.Remove(currentHeadline);
        currentHeadline.SetActive(true);
    }

    public void Previous()
    {
        GameObject temp = current;

        DestroyCurrent();

        objectsToPlace.Insert(0, objectToPlace);
        objectToPlace = objectsToPlace[objectsToPlace.Count - 1];
        objectsToPlace.Remove(objectToPlace);

        current = Instantiate(objectToPlace, temp.transform.position, temp.transform.rotation);

        currentMenu.SetActive(false);
        infoMenus.Insert(0, currentMenu);
        currentMenu = infoMenus[infoMenus.Count - 1];
        infoMenus.Remove(currentMenu);
        currentMenu.SetActive(true);

        currentHeadline.SetActive(false);
        headlines.Insert(0, currentHeadline);
        currentHeadline = headlines[headlines.Count - 1];
        headlines.Remove(currentHeadline);
        currentHeadline.SetActive(true);
    }

    public void DestroyCurrent()
    {
        /*gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.transform.localRotation = new Quaternion();*/

        if (current != null)
        {
            Destroy(current);
            current = null;
        }
    }

    private bool toggle;
    public void ToggleInfo()
    {
        alleTafeln.SetActive(toggle = !toggle);
    }

    private bool isJago;

    public void IsJago() 
    {
        isJago = true;
    }

    public void IsNotJago()
    {
        isJago = false;
    }

    private void UpdatePlacementIndicator()
    {
        if (current != null)
        {
            return;
        }

        if (placementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arRaycast.Raycast(screenCenter, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
