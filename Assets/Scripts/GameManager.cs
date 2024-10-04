using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData; // Referencia do GameData.
    public int money; // Recebe o valor do Player.

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
        gameData.money = money; // Passa o valor do dinheiro atual para o GameData, para esse valor se manter durante as cenas.
    }
}
