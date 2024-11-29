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


        PlayerPrefs.SetInt("BarWomanFlag", gameData.BarWomanFlag);
        PlayerPrefs.SetInt("SellerFlag", gameData.SellerFlag);
        PlayerPrefs.SetInt("BossHandyFlag", gameData.BossHandyFlag);
        PlayerPrefs.SetInt("SabeTudoFlag", gameData.SabeTudoFlag);


        PlayerPrefs.SetInt("TALKED_TO_BOSS", gameData.TALKED_TO_BOSS);
        PlayerPrefs.SetInt("TALKED_TO_SELLER", gameData.TALKED_TO_SELLER);
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



        if (PlayerPrefs.HasKey("BarWomanFlag"))
            gameData.BarWomanFlag = PlayerPrefs.GetInt("BarWomanFlag");

        if (PlayerPrefs.HasKey("SellerFlag"))
            gameData.SellerFlag = PlayerPrefs.GetInt("SellerFlag");

        if (PlayerPrefs.HasKey("BossHandyFlag"))
            gameData.BarWomanFlag = PlayerPrefs.GetInt("BossHandyFlag");

        if (PlayerPrefs.HasKey("SabeTudoFlag"))
            gameData.SabeTudoFlag = PlayerPrefs.GetInt("SabeTudoFlag");

     



        if (PlayerPrefs.HasKey("TALKED_TO_BOSS"))
            gameData.TALKED_TO_BOSS = PlayerPrefs.GetInt("TALKED_TO_BOSS");

        if (PlayerPrefs.HasKey("TALKED_TO_SELLER"))
            gameData.TALKED_TO_SELLER = PlayerPrefs.GetInt("TALKED_TO_SELLER");

    }

    public void NewGame()
    {
        if (gameData == null) return;

        // Cria um novo save.
        gameData.currentLevel = "Tutorial 1";
        gameData.deaths = 0;
        gameData.money = 0;

        //Flags e Triggers
        gameData.BarWomanFlag = 0;
        gameData.SellerFlag = 0;
        gameData.BossHandyFlag = 0;
        gameData.SabeTudoFlag = 0;

        gameData.TALKED_TO_BOSS = 0;
        gameData.TALKED_TO_SELLER = 0;

        PlayerPrefs.DeleteAll(); // Salva as informações de um novo jogo.
    }

    //Converte as flags bool para int e vice versa
    int boolToInt(bool val)
    {
        if (val)
            return 1;
        else
            return 0;
    }

    bool intToBool(int val)
    {
        if (val != 0)
            return true;
        else
            return false;
    }

}
