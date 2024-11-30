using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool isPaused;
    public static bool isOpen;
    public static bool CanPause;
    [SerializeField] private AudioClip popupOpenSound;

    // Start is called before the first frame update
    void Start(){
        if (pauseMenu != null) {
            pauseMenu.SetActive(false);
        }
        
        CanPause = true;
        
    }

    // Update is called once per frame
    void Update(){
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu"){
            
                if (Input.GetKeyDown(KeyCode.Escape)){

                    if (isOpen) {
                        //SoundFXManager.instance.PlaySoundFXClip(popupOpenSound, transform, 1f);
                        PopupOpen();
                    } else {
                    //SoundFXManager.instance.PlaySoundFXClip(popupOpenSound, transform, 1f);
                    PopupClose();
                    }

                }
            
        }else{
            if (CanPause){ 
                if (Input.GetKeyDown(KeyCode.Escape)){
                    //SoundFXManager.instance.PlaySoundFXClip(popupOpenSound, transform, 1f);
                    if (isPaused){
                        SoundFXManager.instance.PlaySoundFXClip(popupOpenSound, transform, 1f);
                        ResumeGame();
                    }else{
                        SoundFXManager.instance.PlaySoundFXClip(popupOpenSound, transform, 1f);
                        PauseGame();
                    }
                }
             }
        }
    }

    public void PopupOpen(){
        

        pauseMenu.SetActive(true);

        isOpen = true;
    }


    public void PopupClose(){
        pauseMenu.SetActive(false);
        
        isOpen = false;
    }


    public void PauseGame(){
        pauseMenu.SetActive(true);
       
        Time.timeScale = 0f;
        isPaused = true;
    }


    public void ResumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GoToMainMenu(){
        //Time.timeScale = 1f;
        //isPaused = false;
        //SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame(){
        Application.Quit();
    }

}
