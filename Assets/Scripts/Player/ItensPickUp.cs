using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class ItensPickUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Money"))
        {
            Money moneyObject = collision.GetComponent<Money>(); // Pega o Script do Money

            if (GameManager.instance != null)
            {
                GameManager.instance.money = moneyObject.moneyAmount; // Pega a quantia de dinheiro que está no Object Money e coloca no GameManager.
            }
            Destroy(moneyObject.gameObject);
        }
    }

}
