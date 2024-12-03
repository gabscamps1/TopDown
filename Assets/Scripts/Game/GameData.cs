using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public int money = 0; // Dinheiro total do Player.
    public int currentMoney = 0; // Dinheiro do Player durante o level.
    public int deaths = 0; // Número de mortes de um jogador.
    public bool died; // Player morreu?

    public string currentLevel = "Tutorial"; // Level do jogo em que o Jogador parou. Não necessáriamente a scene que está no momento.

    public float masterVolume;
    public float soundFXVolume;
    public float musicVolume;

    public bool inHub2;

    [Header("NPC FLAGS")]
    public int BarWomanFlag;
    public int SellerFlag;
    public int BossHandyFlag;
    public int SabeTudoFlag;
    public int bossAttempt;

    //Não usar bool por conta do save game
    public int TALKED_TO_BOSS;
    public int TALKED_TO_SELLER;
}
