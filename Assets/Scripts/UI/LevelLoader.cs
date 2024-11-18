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
       StartCoroutine(LoadLevelInteger(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadSpecificLevel(string levelName, string transition)
    {
        StartCoroutine(LoadLevelString(levelName,transition));
    }

    public void NewGame(){
        StartCoroutine(LoadLevelString("Tutorial 1", "Wipe"));
    }

    public void Continue(){
        StartCoroutine(LoadLevelString("SoundTest","Wipe"));
    }

    public void MainMenu(){
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        StartCoroutine(LoadLevelString("MainMenu","Wipe"));
    }

    IEnumerator LoadLevelString(string levelIndexString, string transition){
        //Toca anima��o
        if(transition != null)
            animatorTransition.SetTrigger(transition);

        //Espera
        yield return new WaitForSeconds(transitionTime);

        //Carrega Cena
        SceneManager.LoadScene(levelIndexString);

    }

    IEnumerator LoadLevelInteger(int levelIndexInt){
        //Toca anima��o
        animatorTransition.SetTrigger("Start");

        //Espera
        yield return new WaitForSeconds(transitionTime);

        //Carrega Cena
        SceneManager.LoadScene(levelIndexInt);

    }
}
