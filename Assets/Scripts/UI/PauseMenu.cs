using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused;
    public static bool isOpen;


    // Start is called before the first frame update
    void Start(){

        pauseMenu.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu"){
            
                if (Input.GetKeyDown(KeyCode.Escape)) {

                    if (isOpen) {
                        PopupOpen();
                    } else {
                        PopupClose();
                    }

                }
            

        }else{
            if (!DialogueManager.isTalking){ 
                if (Input.GetKeyDown(KeyCode.Escape)){
                    if (isPaused){
                        ResumeGame();
                    }else{
                        PauseGame();
                    }
                }
             }
        }
    }

    public void PopupOpen()
    {
        pauseMenu.SetActive(true);

       
        isOpen = true;
    }


    public void PopupClose()
    {
        pauseMenu.SetActive(false);
        
        isOpen = false;
    }


    public void PauseGame() {
        pauseMenu.SetActive(true);
       
        Time.timeScale = 0f;
        isPaused = true;
    }


    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu() {
        //Time.timeScale = 1f;
        //isPaused = false;
        //SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame() {
        Application.Quit();

    }

}
