using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animatorTransition;
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown(KeyCode.K)){
            LoadNextLevel();
        }

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "SplashScreen"){
            if (Input.anyKey){
                LoadNextLevel();
            }
        }
    }

    public void LoadNextLevel() {
       StartCoroutine(LoadLevelInteger(SceneManager.GetActiveScene().buildIndex + 1, "Wipe"));
    }

    public void LoadSpecificLevel(string levelName, string transition)
    {
        StartCoroutine(LoadLevelString(levelName,transition));
    }

    public void NewGame(){
        StartCoroutine(LoadLevelString("Tutorial 1", "Wipe"));
    }

    public void Continue()
    {
        StartCoroutine(LoadLevelString("Hub", "Wipe"));
    }

    public void Death()
    {
        transform.parent.Find("HUD").transform.Find("DeathScreen").gameObject.SetActive(false);
        
        StartCoroutine(LoadLevelString("Hub", "Death"));
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        StartCoroutine(LoadLevelString("MainMenu", "Wipe"));
    }

    IEnumerator LoadLevelString(string levelIndexString, string transition){
        //Toca animação
        if (transition != null)
            animatorTransition.SetTrigger(transition);

        //Espera
        yield return new WaitForSeconds(transitionTime);

        //Carrega Cena
        SceneManager.LoadScene(levelIndexString);

    }

    IEnumerator LoadLevelInteger(int levelIndexInt, string transition){
        if (transition != null)
            animatorTransition.SetTrigger(transition);

        //Espera
        yield return new WaitForSeconds(transitionTime);

        //Carrega Cena
        SceneManager.LoadScene(levelIndexInt);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
