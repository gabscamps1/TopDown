using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public int money;

    public int deaths;

    public int globalFlag;

    public int BarWomanFlag;


    // Start is called before the first frame update
    void Start()
    {
        
    }

}
