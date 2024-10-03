using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    public GameObject[] itensDrop; // Itens que podem ser dropados pelos inimigos.
    public int[] itenChance; // Porcentagem de chance de drop de cada arma - N° do array corresponde ao N° do array do itensDrop.
    public GameObject money; // GameObject do dinheiro.
    // Start is called before the first frame update

    private void Start()
    {
        // Destroy(gameObject,2f);
    }
    private void OnDestroy()
    {
        DropChance();
        if (money != null) DropMoney();
    }

    // Calcula a chance de drop de cada item.
    private void DropChance()
    {
        int aleatoryDrop = Random.Range(0, itensDrop.Length);
        int aleatoryNumber = Random.Range(0, 100);

        if (aleatoryNumber < itenChance[aleatoryDrop])
        {
            Drop(aleatoryDrop);
        }
    }

    private void Drop(int aleatoryDrop)
    {
        if (itensDrop[aleatoryDrop] != null)
        {
            Instantiate(itensDrop[aleatoryDrop], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }
    }
    private void DropMoney()
    {
        Instantiate(money, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
    }
}
