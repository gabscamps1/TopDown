using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    enum SpawnerForm { perUnit, perTime }
    [SerializeField] SpawnerForm spawnerForm;

    [SerializeField] 
    List<EnemyList> enemyList = new List<EnemyList>();
    
    

    // Start is called before the first frame update
   /* [SerializeField] GameObject enemyFighter;
    [SerializeField] int amountEnemyFighter;

    [SerializeField] GameObject enemyShooter;
    [SerializeField] int amountEnemyShooter;

    GameObject[] enemys;*/

    [SerializeField] float delayToSpawn;
    float countDelaySpawn;
   
    // Update is called once per frame
    void Update()
    {
        ChooseForm();
    }

    // Chama a função de gerar inimigos de formas diferentes dependendo de qual opção foi selecionada.
    void ChooseForm()
    {
        switch (spawnerForm)
        {
            case SpawnerForm.perUnit:

                TrySpawnPerUnity();

            break;

            case SpawnerForm.perTime:

                countDelaySpawn -= Time.deltaTime;
                if (countDelaySpawn <= 0) TrySpawnPerTime();

            break;
        }
    }

    // Função para gerar inimigos quando os inimigos gerados forem derrotados.
    void TrySpawnPerUnity()
    {
        if (transform.childCount == 0)
        {
            WhatToSpawn();
        }
    }

    // Função para gerar inimigos em determinado tempo.
    void TrySpawnPerTime()
    {
        WhatToSpawn();
        countDelaySpawn = delayToSpawn;
    }

    // Função para decedir qual inimigo gerar e a quantidade.
    void WhatToSpawn()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            int thisMaxQuantity = enemyList[i].maxQuantity; // Quantidade total de Inimigos da lista atual.

            // Confere se ainda deve gerar algum Inimigo da lista atual, se não vai para próxima lista.
            if (thisMaxQuantity > 0)
            {
                // Se não tiver nenhum Inimigo para ser gerado dessa vez, procure a próxima.
                for (int n = 0; n < enemyList[i].spawnPerTime.Length - 1; n++)
                {
                    // Verifica se o número de spawn (spawnPerTime[n]) é maior que 0.
                    if (enemyList[i].spawnPerTime[n] > 0)
                    {
                        // Chama a função para spawnar os inimigos.
                        SpawnEnemys(enemyList[i].enemy, enemyList[i].spawnPerTime[n]);

                        // Diminui a quantidade total de inimigos pela quantidade de inimigos spawnados.
                        enemyList[i].maxQuantity -= enemyList[i].spawnPerTime[n];

                        // Depois de spawnar, zera o número de inimigos gerarados.
                        enemyList[i].spawnPerTime[n] = 0;

                        return;
                    }
                }


                // Caso não tenha nenhum inimigo para ser gerado, gere a quantidade final estabelecida.
                if (enemyList[i].spawnPerTime[enemyList[i].spawnPerTime.Length - 1] > thisMaxQuantity)
                {
                    // Gera a quantidade de inimigos restante dessa lista.
                    SpawnEnemys(enemyList[i].enemy, thisMaxQuantity);

                    // Diminui a quantidade total de inimigos dessa lista.
                    enemyList[i].maxQuantity = 0;
                }
                else
                {
                    // Gera a quantidade de inimigos estabelecidos.
                    SpawnEnemys(enemyList[i].enemy, enemyList[i].spawnPerTime[enemyList[i].spawnPerTime.Length - 1]);

                    // Diminui a quantidade total de inimigos dessa lista.
                    enemyList[i].maxQuantity -= enemyList[i].spawnPerTime[enemyList[i].spawnPerTime.Length - 1];
                }

                return;
            }
        }
    }

    // Função para gerar Inimigos.
    void SpawnEnemys(GameObject enemy, int quantitty)
    {
        // Gera o número de inimigos solicitados.
        for (int i = 0; i < quantitty; i++)
        {   
            Instantiate(enemy, transform.position, Quaternion.identity, transform);
        }
        
    }
}

[System.Serializable]

public class EnemyList
{
    public GameObject enemy;
    public int maxQuantity;
    public int[] spawnPerTime;
}