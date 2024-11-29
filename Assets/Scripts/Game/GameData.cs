using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public int money; // Dinheiro do Player.
    public int deaths; // N�mero de mortes de um jogador.
    public bool died; // Player morreu?

    public string currentLevel = "Tutorial"; // Level do jogo em que o Jogador parou. N�o necess�riamente a scene que est� no momento.

    public float masterVolume;
    public float soundFXVolume;
    public float musicVolume;


    [Header("NPC FLAGS")]
    public int BarWomanFlag;
    public int SellerFlag;
    public int BossHandyFlag;
    public int SabeTudoFlag;

    //N�o usar bool por conta do save game
    public int TALKED_TO_BOSS;
    public int TALKED_TO_SELLER;
}
