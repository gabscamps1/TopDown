using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] GameObject[] itensDrop; // Itens que podem ser dropados pelos inimigos.
    [SerializeField] int[] itenChance; // Porcentagem de chance de drop de cada item - N� do array corresponde ao N� do array do itensDrop.
    [SerializeField] GameObject money; // GameObject do dinheiro.
    
    private void Start()
    {
        Destroy(gameObject,2f);
    }
    private void OnDestroy()
    {
        DropChance(); // Chama a fun��o de calculo de drop quando o Objecto � destruido.
        if (money != null) DropMoney();
    }

    // Calcula a chance de drop de cada item.
    private void DropChance()
    {
        // Calcula o item que ser� selecionado para o drop.
        int aleatoryDrop = Random.Range(0, itensDrop.Length); 

        // Calcula o drop do item selecionado.
        int aleatoryNumber = Random.Range(0, 100);
        if (aleatoryNumber < itenChance[aleatoryDrop])
        {
            Drop(aleatoryDrop); // Chama a fun��o de drop do item.
        }
    }

    // Fun��o de drop do item.
    private void Drop(int aleatoryDrop)
    {
        if (itensDrop[aleatoryDrop] != null)
        {
            Instantiate(itensDrop[aleatoryDrop], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))); // Inst�ncia o GameObject do item no cen�rio.
        }
    }

    // Fu���o de drop do dinheiro.
    private void DropMoney()
    {
        Instantiate(money, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))); // Inst�ncia o GameObject do dinheiro no cen�rio.
    }
}
