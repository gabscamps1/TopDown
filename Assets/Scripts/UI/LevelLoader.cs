using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadNextLevel();
        }

        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "SplashScreen")
        {
            if (Input.anyKey)
            {
                LoadNextLevel();
            }


        }
    }



    public void LoadNextLevel() {

       StartCoroutine(LoadLevelInteger(SceneManager.GetActiveScene().buildIndex + 1));
        //
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()
    {
        StartCoroutine(LoadLevelString("Tutorial 1"));
    }

    public void Continue()
    {
        StartCoroutine(LoadLevelString("DialogueRoom"));
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
        StartCoroutine(LoadLevelString("MainMenu"));
    }

    IEnumerator LoadLevelString(string levelIndexString)
    {
        //Toca animação
        transition.SetTrigger("Start");

        //Espera
        yield return new WaitForSeconds(transitionTime);

        //Carrega Cena
        SceneManager.LoadScene(levelIndexString);

    }

    IEnumerator LoadLevelInteger(int levelIndexInt) {
        //Toca animação
        transition.SetTrigger("Start");

        //Espera
        yield return new WaitForSeconds(transitionTime);

        //Carrega Cena
        SceneManager.LoadScene(levelIndexInt);

    }
}
