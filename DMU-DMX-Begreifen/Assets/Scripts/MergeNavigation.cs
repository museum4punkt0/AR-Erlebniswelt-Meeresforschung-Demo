using System.Collections.Generic;
using UnityEngine;

public class MergeNavigation : MonoBehaviour
{
    private Transform root;
    private GameObject basalt;
    private GameObject kupferZinkErz;
    private GameObject methanHydrat;
    private GameObject koralle;
    private GameObject korallenSchwamm;
    private GameObject manganKnolle;
    private GameObject raucher;
    private GameObject jago;

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

    private List<GameObject> mergeObjects;
    private List<GameObject> infoMenus;
    private List<GameObject> headlines;
    private GameObject currentObject;
    private GameObject currentMenu;
    private GameObject currentHeadline;

    private void Start()
    {
        var bufferedCamera = GameObject.Find("TextureBufferCamera");

        if (bufferedCamera != null) bufferedCamera.SetActive(true);

        var otherRoot = GameObject.Find("1:DynamicImageTarget-RetailCube002");

        root = otherRoot != null ? otherRoot.transform.Find("MergeMultiTarget").transform.GetChild(0) : GameObject.Find("MergeMultiTarget").transform.GetChild(0);

        basalt = root.Find("Basalt").gameObject;
        kupferZinkErz = root.Find("KupferZinkErz").gameObject;
        methanHydrat = root.Find("MethanHydrat").gameObject;
        koralle = root.Find("Koralle").gameObject;
        korallenSchwamm = root.Find("KorallenSchwamm").gameObject;
        manganKnolle = root.Find("ManganKnolle").gameObject;
        raucher = root.Find("Raucher").gameObject;
        jago = root.Find("Jago").gameObject;

        mergeObjects = new List<GameObject>(new[] { kupferZinkErz, methanHydrat, koralle, korallenSchwamm, manganKnolle, raucher, jago });
        infoMenus = new List<GameObject>(new[] { kupferZinkTafel, methanHydratTafel, koralleTafel, korallenSchwammTafel, manganTafel, raucherTafel, jagoTafel });
        headlines = new List<GameObject>(new[] { kupferZinkUeberschrift, methanHydratUeberschrift, koralleUeberschrift, korallenSchwammUeberschrift, manganUeberschrift, raucherUeberschrift, jagoUeberschrift });

        foreach (GameObject x in mergeObjects) 
        {
            x.SetActive(false);
        }

        (currentObject = basalt).SetActive(true);
        currentMenu = basaltTafel;
        currentHeadline = basaltUeberschrift;
    }

    public void NextObject()
    {
        currentObject.SetActive(false);
        mergeObjects.Add(currentObject);
        currentObject = mergeObjects[0];
        mergeObjects.Remove(currentObject);
        currentObject.SetActive(true);

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

    public void PreviousObject()
    {
        currentObject.SetActive(false);
        mergeObjects.Insert(0, currentObject);
        currentObject = mergeObjects[mergeObjects.Count - 1];
        mergeObjects.Remove(currentObject);
        currentObject.SetActive(true);

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

    private bool toggle;
    public void ToggleInfo()
    {
        alleTafeln.SetActive(toggle = !toggle);
    }
}
