using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameData gameData; // Referência do GameData.
    public GameObject player; // Referência do Player em cena.
    public LevelLoader levelLoader; // Referência do Level Loader.
    public int money; // Recebe o valor do script ItensPickUp do Player.
    public int deaths; // Recebe o valor do script Damage do player.


    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        if (gameData.inHub2 && SceneManager.GetActiveScene().name == "Hub")
        {
            player.transform.position = new Vector3(7.71f, 0.43f, 0);
            gameData.inHub2 = false;
        }

        if (SceneManager.GetActiveScene().name != "Hub")
        {
            gameData.inHub2 = false;
        }

        if (SceneManager.GetActiveScene().name == "Hub 2")
        {
            gameData.inHub2 = true;
        }
    }

    // Atualizações no GameData.
    void Update()
    {
        if (money != 0)
        {
            gameData.money += money; // Passa o valor do dinheiro atual para o GameData, para esse valor se manter durante as cenas.
            gameData.currentMoney += money;
            money = 0; // Volta o money para 0 depois de alterar o valor no GameData.
        }

        if (deaths != 0)
        {
            gameData.deaths += deaths; // Passa o valor do dinheiro atual para o GameData, para esse valor se manter durante as cenas.
            deaths = 0; // Volta o money para 0 depois de alterar o valor no GameData.
        }

        // Confere se o player morreu. Atualizado pelo script PlayerDamage.
        if (gameData.died)
        {
            gameData.money -= gameData.currentMoney;
            gameData.currentMoney = 0;
        }
    }
}
