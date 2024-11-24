using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public int money; // Dinheiro do Player.

    public int deaths; // Número de mortes de um jogador.
    private int deaths_ant;
    public bool died; // Player morreu?

    public string currentLevel = "Tutorial"; // Level do jogo em que o Jogador parou. Não necessáriamente a scene que está no momento.


    [Header("NPC FLAGS")]
    public int BarWomanFlag;
    public int SellerFlag;
    public int BossHandyFlag;
    public bool TALKED_TO_BOSS;
    public bool TALKED_TO_SELLER;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if (deaths != deaths_ant)
        {
            died = true;
            deaths_ant = deaths;
        }
    }

}
