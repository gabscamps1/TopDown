using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] GameObject[] itensDrop; // Itens que podem ser dropados pelos inimigos.
    [SerializeField] int[] itenChance; // Porcentagem de chance de drop de cada item - N° do array corresponde ao N° do array do itensDrop.
    [SerializeField] GameObject money; // GameObject do dinheiro.

    // Calcula a chance de drop de cada item.
    public void DropChance()
    {
        if (itensDrop.Length <= 0 && itenChance.Length <= 0) return;

        // Calcula o item que será selecionado para o drop.
        int aleatoryDrop = Random.Range(0, itensDrop.Length); 

        // Calcula o drop do item selecionado.
        int aleatoryNumber = Random.Range(0, 100);
        if (aleatoryNumber < itenChance[aleatoryDrop])
        {
            Drop(aleatoryDrop); // Chama a função de drop do item.
        }
    }

    // Função de drop do item.
    private void Drop(int aleatoryDrop)
    {
        if (itensDrop[aleatoryDrop] != null)
        {
            Instantiate(itensDrop[aleatoryDrop], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))); // Instância o GameObject do item no cenário.
        }
    }

    // Função de drop do dinheiro.
    public void DropMoney()
    {
        if (money != null) Instantiate(money, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))); // Instância o GameObject do dinheiro no cenário.
    }
}
