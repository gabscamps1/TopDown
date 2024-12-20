using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    enum SpawnerForm { perUnit, perTime }
    [SerializeField] SpawnerForm spawnerForm;

    [SerializeField] 
    List<EnemyList> enemyList = new List<EnemyList>();

    [SerializeField] float delayToSpawn;
    float countDelaySpawn;

    Collider2D areaToSpawn;

    void Start()
    {
        areaToSpawn = GetComponent<Collider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        ChooseForm();
    }

    // Chama a fun��o de gerar inimigos de formas diferentes dependendo de qual op��o foi selecionada.
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

    // Fun��o para gerar inimigos quando os inimigos gerados forem derrotados.
    void TrySpawnPerUnity()
    {
        if (transform.childCount == 0)
        {
            WhatToSpawn();
        }
    }

    // Fun��o para gerar inimigos em determinado tempo.
    void TrySpawnPerTime()
    {
        WhatToSpawn();
        countDelaySpawn = delayToSpawn;
    }

    // Fun��o para decedir qual inimigo gerar e a quantidade.
    void WhatToSpawn()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            int thisMaxQuantity = enemyList[i].maxQuantity; // Quantidade total de Inimigos da lista atual.

            // Confere se ainda deve gerar algum Inimigo da lista atual, se n�o vai para pr�xima lista.
            if (thisMaxQuantity > 0)
            {
                // Se n�o tiver nenhum Inimigo para ser gerado dessa vez, procure a pr�xima.
                for (int n = 0; n < enemyList[i].spawnPerTime.Length - 1; n++)
                {
                    // Verifica se o n�mero de spawn (spawnPerTime[n]) � maior que 0.
                    if (enemyList[i].spawnPerTime[n] > 0)
                    {
                        // Chama a fun��o para spawnar os inimigos.
                        StartCoroutine(SpawnEnemys(enemyList[i].enemy, enemyList[i].spawnPerTime[n]));

                        // Diminui a quantidade total de inimigos pela quantidade de inimigos spawnados.
                        enemyList[i].maxQuantity -= enemyList[i].spawnPerTime[n];

                        // Depois de spawnar, zera o n�mero de inimigos gerarados.
                        enemyList[i].spawnPerTime[n] = 0;

                        return;
                    }
                }


                // Caso n�o tenha nenhum inimigo para ser gerado, gere a quantidade final estabelecida.
                if (enemyList[i].spawnPerTime[enemyList[i].spawnPerTime.Length - 1] > thisMaxQuantity)
                {
                    // Gera a quantidade de inimigos restante dessa lista.
                    StartCoroutine(SpawnEnemys(enemyList[i].enemy, thisMaxQuantity));

                    // Diminui a quantidade total de inimigos dessa lista.
                    enemyList[i].maxQuantity = 0;
                }
                else
                {
                    // Gera a quantidade de inimigos estabelecidos.
                    StartCoroutine(SpawnEnemys(enemyList[i].enemy, enemyList[i].spawnPerTime[enemyList[i].spawnPerTime.Length - 1]));

                    // Diminui a quantidade total de inimigos dessa lista.
                    enemyList[i].maxQuantity -= enemyList[i].spawnPerTime[enemyList[i].spawnPerTime.Length - 1];
                }

                return;
            }
        }
    }

    // Fun��o para gerar Inimigos.
    IEnumerator SpawnEnemys(GameObject enemy, int quantitty)
    {
        // Gera o n�mero de inimigos solicitados.
        for (int i = 0; i < quantitty; i++)
        {
            bool isValidPosition = false;
            Vector2 position = transform.position;

            while (!isValidPosition)
            {
                position = (Vector2)transform.position - ((Vector2)areaToSpawn.bounds.size / 2) + new Vector2(Random.Range(0, areaToSpawn.bounds.size.x), Random.Range(0, areaToSpawn.bounds.size.y));
                Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);

                foreach (Collider2D collider in colliders)
                {
                    if (!collider.isTrigger)
                    {
                        yield return new WaitForNextFrameUnit();
                    }
                }

                isValidPosition = true;
            }
            
            Instantiate(enemy, position, Quaternion.identity, transform);
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