using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class ImageTracking : MonoBehaviour
{

    [SerializeField] private GameObject ui;

    private ARTrackedImageManager imageManager;

    private void Awake()
    {
        imageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    public void OnEnable()
    {
        imageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (ui != null) ui.SetActive(false);
    }
}
