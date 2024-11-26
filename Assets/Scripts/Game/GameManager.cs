using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData; // Referência do GameData.
    public GameObject player; // Referência do Player em cena.
    public LevelLoader levelLoader; // Referência do Level Loader.
    public int money; // Recebe o valor do script ItensPickUp do Player.
    int currentMoney; // Dinheiro atual da fase.
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
    }
    // Update is called once per frame
    void Update()
    {
        if (money != 0)
        {
            gameData.money += money; // Passa o valor do dinheiro atual para o GameData, para esse valor se manter durante as cenas.
            currentMoney += money;
            money = 0; // Volta o money para 0 depois de alterar o valor no GameData.
        }

        if (deaths != 0)
        {
            gameData.deaths += deaths; // Passa o valor do dinheiro atual para o GameData, para esse valor se manter durante as cenas.
            deaths = 0; // Volta o money para 0 depois de alterar o valor no GameData.
        }

        if (gameData.died)
        {
            gameData.money -= currentMoney;
            currentMoney = 0;
        }
    }
}
