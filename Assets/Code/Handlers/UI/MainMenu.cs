using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : PanelGroupHandler
{




    public void StartGame()
    {
        SceneManager.UnloadSceneAsync("MainMenu");
        SceneManager.LoadScene("Map_1", LoadSceneMode.Additive);
        //StartCoroutine(StartupGame());


    }



    public override void OnBack()
    {
        //if (currentActive != null)
            //if (currentActive.backwardsPanel == null)
                //if (SceneManager.GetActiveScene().name == "GameplayScene") FindObjectOfType<PauseMenu>().TogglePause();
        base.OnBack();
        //FlagHandler.Instance.WriteFile();


    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void BackToTitleScreen()
    {
        //FindObjectOfType<KeyCanvas>()?.Clear();
        ///PauseHandler.Instance.SetUnpaused();
        //ControllerHandler.mouseVisible = true;
        //Time.timeScale = 1;
        //SceneManager.UnloadSceneAsync("GameplayScene");
        //SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Additive);
    }
}
