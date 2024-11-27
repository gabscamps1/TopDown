using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    // Start is called before the first frame update
    GameData gameData;
    [SerializeField] bool canSave;
    void Start()
    {
        if (GameManager.instance.gameData != null)
            gameData = GameManager.instance.gameData;

        if (canSave)
        {
            if (gameData.died)
            {
                SaveGameDeath();
                gameData.died = false;
            }
            else
            {
                SaveGame();
            }
        }
    }

    public void SaveGame()
    {
        if (gameData == null) return;

        PlayerPrefs.SetString("currentLevel", gameData.currentLevel);
        PlayerPrefs.SetInt("deaths", gameData.deaths);
        PlayerPrefs.SetInt("money", gameData.money);
    }

    public void SaveGameDeath()
    {
        PlayerPrefs.SetInt("deaths", gameData.deaths);
    }

    public bool HasSave()
    {
        bool hasSave = 
            PlayerPrefs.HasKey("currentLevel") && 
            PlayerPrefs.HasKey("deaths") &&
            PlayerPrefs.HasKey("money");

        return hasSave;
    }

    public void LoadGame()
    {
        if (gameData == null) return;

        if(PlayerPrefs.HasKey("currentLevel"))
            gameData.currentLevel = PlayerPrefs.GetString("currentLevel");

        if (PlayerPrefs.HasKey("deaths"))
            gameData.deaths = PlayerPrefs.GetInt("deaths");

        if (PlayerPrefs.HasKey("money"))
            gameData.money = PlayerPrefs.GetInt("money");
    }

    public void NewGame()
    {
        if (gameData == null) return;

        // Cria um novo save.
        gameData.currentLevel = "Tutorial 1";
        gameData.deaths = 0;
        gameData.money = 0;

        PlayerPrefs.DeleteAll(); // Salva as informações de um novo jogo.
    }

}
