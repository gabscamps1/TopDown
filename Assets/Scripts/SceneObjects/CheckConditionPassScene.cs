using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckConditionPassScene : MonoBehaviour
{
    enum WhenCallToPass { InArea, Global }
    [SerializeField] WhenCallToPass whenCallToPass;
    // InArea - A cena será passada se a Condition selecionada for comprida e o Player entrar na área do Objeto com este Script.
    // Global - A cena será passada automáticamente se a Condition selecionada for comprida.

    enum Condition { AllEnemys, Key, Coordinates, DeadCharacter }
    [SerializeField] Condition condition;
    // AllEnemys - Quando todos os Inimigos estão mortos na cena, o Player consegue passar pela porta.
    // Key - Quando o Player consegue uma chave, o Player consegue passar pela porta.
    // Coordinate -O Player consegue passar pela porta sem precisar de nada.
    // DeadCharacter - Quando um Inimigo especifico for morto, o Player consegue passar de fase.

    enum Levels {currentLevel, Tutorial1, Tutorial2, Tutorial3, Hub, Level1_1, Level1_2, Level1_3, Level1_4, Level1_5}
    [SerializeField] Levels levels;

    enum Achievements {None, Tutorial, Level1}
    [SerializeField] Achievements achievements;

    [SerializeField] GameObject character; // Mêcanica do DeadCharacter - Inimigo específico a ser morto.
    GameData gameData;

    private void Start()
    {
        if (GameManager.instance.gameData != null)
            gameData = GameManager.instance.gameData;

    }
    // Update is called once per frame
    void Update()
    {
        if (whenCallToPass == WhenCallToPass.Global)
        {
            CallCondition();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        if (whenCallToPass == WhenCallToPass.InArea)
        {
            CallCondition();
        }

    }

    void CallCondition()
    {
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
                    ChooseAchievements();
                }

            break;

            case Condition.Key:
            break;

            case Condition.Coordinates:

                PassScene();
                ChooseAchievements();

            break;

            case Condition.DeadCharacter:

                if (character.IsDestroyed())
                {
                    PassScene();
                    ChooseAchievements();
                }

            break;
        }
    }

    void ChooseAchievements()
    {
        if (gameData == null) return;

        switch (achievements)
        {
            case Achievements.Tutorial:
                GameManager.instance.gameData.currentLevel = "Tutorial 1";
                break;

            case Achievements.Level1:
                GameManager.instance.gameData.currentLevel = "Level 1-1";
                break;
        }
    }

    void PassScene()
    {
        string levelName = "Tutorial 1";

        switch (levels)
        {
            case Levels.currentLevel:
                if (gameData != null)
                levelName = gameData.currentLevel;
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
            case Levels.Level1_3:
                levelName = "Level 1-3";
                break;
            case Levels.Level1_4:
                levelName = "Level 1-4";
                break;
            case Levels.Level1_5:
                levelName = "Level 1-5";
                break;
            default:
                return;
        }

        LevelLoader levelLoader = GameManager.instance.levelLoader;
        levelLoader.LoadSpecificLevel(levelName,"Wipe");
    }
}
