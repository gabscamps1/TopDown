using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckConditionPassScene : MonoBehaviour
{
    enum Condition { AllEnemys, Key, Coordinates }
    [SerializeField] Condition change;

    enum Levels { Tutorial1, Tutorial2, Tutorial3, Level1 }
    [SerializeField] Levels levels;

    
    private void Awake()
    {   
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (change)
            {
                case Condition.AllEnemys:
                    GameObject[] allObject = GameObject.FindObjectsOfType<GameObject>(true);
                    List<GameObject> enemies = new List<GameObject>();

                    foreach (GameObject obj in allObject)
                    {
                        if (obj != null)
                        {
                            if (obj.CompareTag("Enemy"))
                            {
                                enemies.Add(obj);
                            }
                        }
                    }
                    if (enemies.Count == 0)
                    {
                        PassScene();
                    }
                    break;
                case Condition.Key:
                    break;
                case Condition.Coordinates:
                    PassScene();
                    break;
            }
        }
    }

    void PassScene()
    {
        string levelName = "Tutorial 1";

        switch (levels)
        {
            case Levels.Tutorial1:
                levelName = "Tutorial 1";
                break;
            case Levels.Tutorial2:
                levelName = "Tutorial 2";
                break;
            case Levels.Tutorial3:
                levelName = "Tutorial 3";
                break;
            case Levels.Level1:
                levelName = "Level 1-1";
                break;
        }

        LevelLoader levelLoader = GameManager.instance.levelLoader;
        levelLoader.LoadSpecificLevel(levelName,"Wipe");
    }
}
