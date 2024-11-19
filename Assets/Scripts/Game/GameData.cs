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

    public string currentLevel = "level1"; // Level do jogo em que o Jogador parou. Não necessáriamente a scene que está no momento.

<<<<<<< HEAD
=======
    public int deaths;

    public int globalFlag;

    public int BarWomanFlag;


>>>>>>> 7aebd6de4af611aad499a63126c12be32e98054b
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
