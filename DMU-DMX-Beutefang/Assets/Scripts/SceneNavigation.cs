using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public void MainScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void GameScene()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
