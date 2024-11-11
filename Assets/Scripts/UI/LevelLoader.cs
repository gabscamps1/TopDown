using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadNextLevel() {

       StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        //
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()
    {

        SceneManager.LoadScene("Tutorial 1");
        //
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadLevel(int levelIndex) {
        //Toca animação
        transition.SetTrigger("Start");

        //Espera
        yield return new WaitForSeconds(transitionTime);

        //Carrega Cena
        SceneManager.LoadScene(levelIndex);

    }
}
