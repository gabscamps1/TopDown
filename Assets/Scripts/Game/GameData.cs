using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public int money; // Dinheiro do Player.

    public int deaths; // N�mero de mortes de um jogador.
    private int deaths_ant;
    public bool died; // Player morreu?

    public string currentLevel = "level1"; // Level do jogo em que o Jogador parou. N�o necess�riamente a scene que est� no momento.

    public int BarWomanFlag;

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
