using UnityEngine;
using UnityEngine.SceneManagement;

public class TempButton : MonoBehaviour
{
    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
