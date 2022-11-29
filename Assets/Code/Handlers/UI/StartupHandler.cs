using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupHandler : MonoBehaviour
{
    [SerializeField] string firstActiveScene = "Amber-TestScene";
    [SerializeField] List<string> redirectIfThisScene = new List<string>();
    public static string activeScene = "Amber-TestScene";

    [RuntimeInitializeOnLoadMethod]
    static void RunTimeMethod()
    {
        
        activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("StartupScene", LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        
#if UNITY_EDITOR
        if (activeScene != firstActiveScene && !redirectIfThisScene.Contains(activeScene) && SceneManager.GetActiveScene().name != activeScene) {
            SceneManager.LoadScene(activeScene, LoadSceneMode.Additive);
            return;
        }
#endif
        
        SceneManager.LoadScene(firstActiveScene, LoadSceneMode.Additive);
    }

}
