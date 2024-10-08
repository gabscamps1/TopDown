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
        //if (Input.GetMouseButtonDown(0)) {
        //    LoadNextLevel();
        ////
    }

    public void LoadNextLevel() {

       StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        //
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LoadLevel(int levelIndex) {
        //Toca anima��o
        transition.SetTrigger("Start");

        //Espera
        yield return new WaitForSeconds(transitionTime);

        //Carrega Cena
        SceneManager.LoadScene(levelIndex);

    }
}
