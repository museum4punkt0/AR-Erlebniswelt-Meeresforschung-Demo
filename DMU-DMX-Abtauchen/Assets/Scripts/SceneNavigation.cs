using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Script to navigate between scenes
 */
public class SceneNavigation : MonoBehaviour
{
    public void MainScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void Simulation()
    {
        SceneManager.LoadScene("Simulation", LoadSceneMode.Single);
    }
}