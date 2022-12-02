using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupHandler : MonoBehaviour
{
    [SerializeField] string firstActiveScene = "MainMenu";
    [SerializeField] List<string> redirectIfThisScene = new List<string>();
    public static string activeScene = "MainMenu";

    [RuntimeInitializeOnLoadMethod]
    static void RunTimeMethod()
    {
      
        activeScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("StartupScene", LoadSceneMode.Single);
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        
#if UNITY_EDITOR
        if (activeScene != firstActiveScene && !redirectIfThisScene.Contains(activeScene) && SceneManager.GetActiveScene().name != activeScene) {
            SceneManager.LoadScene(activeScene, LoadSceneMode.Single);
            return;
        }
#endif
        
        SceneManager.LoadScene(firstActiveScene, LoadSceneMode.Single);
    }

}
