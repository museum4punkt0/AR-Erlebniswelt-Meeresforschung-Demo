using System.Collections.Generic;
using UnityEngine;

/**
 * Script to navigate between the simulation scenes.
 */
public class SimulationNavigation : MonoBehaviour
{

    [Header("Scenes")]
    [SerializeField] private GameObject scene1;
    [SerializeField] private GameObject scene2;
    [SerializeField] private GameObject scenePicture1;
    [SerializeField] private GameObject scenePicture2;
    [SerializeField] private GameObject scenePicture3;
    [SerializeField] private GameObject scenePicture4;
    [SerializeField] private GameObject scenePicture5;
    [SerializeField] private GameObject scenePicture6;
    [SerializeField] private GameObject scene3;
    [SerializeField] private GameObject scene4;
    [SerializeField] private GameObject scene5;

    private List<GameObject> scenes;

    private GameObject currentScene;

    private void Start()
    {
        scenes = new List<GameObject>(new[] { scene2, scenePicture1, scenePicture2, scenePicture3, scenePicture4, scenePicture5, scenePicture6, scene3, scene4, scene5 });
        currentScene = scene1;
    }

    public void NextScene()
    {
        if (currentScene.name.Equals("Scene4")) GameObject.Find("Ambient").GetComponent<AudioSource>().Play();

        currentScene.SetActive(false);
        scenes.Add(currentScene);
        currentScene = scenes[0];
        scenes.Remove(currentScene);
        currentScene.SetActive(true);

        if (currentScene.name.Equals("Scene4")) GameObject.Find("Ambient").GetComponent<AudioSource>().Stop();
        if (currentScene.name.Equals("Scene5")) currentScene.GetComponent<SinkRiseScript>().Play();
    }

    public void PreviousScene()
    {
        if (currentScene.name.Equals("Scene4")) GameObject.Find("Ambient").GetComponent<AudioSource>().Play();

        currentScene.SetActive(false);
        scenes.Insert(0, currentScene);
        currentScene = scenes[scenes.Count - 1];
        scenes.Remove(currentScene);
        currentScene.SetActive(true);

        if (currentScene.name.Equals("Scene4")) GameObject.Find("Ambient").GetComponent<AudioSource>().Stop();
        if (currentScene.name.Equals("Scene5")) currentScene.GetComponent<SinkRiseScript>().Play();
    }
}
