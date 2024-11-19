using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckConditionPassScene : MonoBehaviour
{
    enum Condition { AllEnemys, Key, Coordinates, DeadCharacter }
    [SerializeField] Condition condition;
    // AllEnemys - Quando todos os Inimigos estão mortos na cena, o Player consegue passar pela porta.
    // Key - Quando o Player consegue uma chave, o Player consegue passar pela porta.
    // Coordinate -O Player consegue passar pela porta sem precisar de nada.
    // DeadCharacter - Quando um Inimigo especifico for morto, o Player consegue passar de fase.

    enum Levels {currentLevel, Tutorial1, Tutorial2, Tutorial3, Hub, Level1_1, Level1_2, Level1_3, Level1_4, Level1_5}
    [SerializeField] Levels levels;

    [SerializeField] GameObject character; // Mêcanica do DeadCharacter - Inimigo específico a ser morto.
    

    // Update is called once per frame
    void Update()
    {
        switch (condition)
        {
            case Condition.DeadCharacter:

                if (character == null) return;

                if (character.IsDestroyed())
                {
                    PassScene();
                }
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        switch (condition)
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

    void PassScene()
    {
        string levelName = "Tutorial 1";

        switch (levels)
        {
            case Levels.currentLevel:
                break;
            case Levels.Tutorial1:
                levelName = "Tutorial 1";
                break;
            case Levels.Tutorial2:
                levelName = "Tutorial 2";
                break;
            case Levels.Tutorial3:
                levelName = "Tutorial 3";
                break;
            case Levels.Hub:
                levelName = "Hub";
                break;
            case Levels.Level1_1:
                levelName = "Level 1-1";
                break;
            case Levels.Level1_2:
                levelName = "Level 1-2";
                break;
            case Levels.Level1_4:
                levelName = "Level 1-4";
                break;
            default:
                return;
        }

        LevelLoader levelLoader = GameManager.instance.levelLoader;
        levelLoader.LoadSpecificLevel(levelName,"Wipe");
    }
}
