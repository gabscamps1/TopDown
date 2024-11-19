using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    // Start is called before the first frame update
    GameData gameData;
    void Start()
    {
        if (GameManager.instance.gameData != null)
            gameData = GameManager.instance.gameData;
    }

    // Update is called once per frame
    void Update()
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

    public void SaveGame()
    {
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
        gameData.currentLevel = PlayerPrefs.GetString("currrentLevel");
        gameData.deaths = PlayerPrefs.GetInt("deaths");
        gameData.money = PlayerPrefs.GetInt("money");
    }
}
