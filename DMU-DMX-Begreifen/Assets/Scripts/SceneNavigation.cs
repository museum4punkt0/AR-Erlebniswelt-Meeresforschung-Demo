using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{

    private static bool initialized;

    public void MergeScene()
    {
        if (initialized)
        {
            SceneManager.LoadScene("MergeAfter", LoadSceneMode.Single);

            ToggleMerge(true);
        }
        else
        {
            SceneManager.LoadScene("Merge", LoadSceneMode.Single);
            initialized = true;
        }
    }

    public void MainScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);

        if (initialized)
        {
            ToggleMerge(false);
        }
    }
    

    private static void ToggleMerge(bool toggle)
    {
        var root = GameObject.Find("1:DynamicImageTarget-RetailCube002");

        var multiTarget = root != null ? root.transform.Find("MergeMultiTarget").gameObject : GameObject.Find("MergeMultiTarget");

        if (multiTarget != null) multiTarget.SetActive(toggle);
    }
}
