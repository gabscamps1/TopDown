using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    // Start is called before the first frame update
    GameData gameData;
    bool hasSave;
    void Start()
    {
        if (GameManager.instance.gameData != null)
            gameData = GameManager.instance.gameData;

        if (gameData.died)
        {
            SaveGameDeath();
            gameData.died = false;
        }
        else
        {
            SaveGame();
        }

        LoadGame();
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

    public void LoadGame()
    {
        if (gameData == null) return;

        if(PlayerPrefs.HasKey("currrentLevel"))
            gameData.currentLevel = PlayerPrefs.GetString("currrentLevel");

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
