using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class ItensPickUp : MonoBehaviour
{
    [SerializeField] private AudioClip moneyPickupSound;

    // Chama a fun��o quando algum GameObject entra no cen�rio.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Money"))
        {
            SoundFXManager.instance.PlaySoundFXClip(moneyPickupSound, transform, 1f);

            Money moneyObject = collision.GetComponent<Money>(); // Pega o Script do Money

            if (GameManager.instance != null)
            {
                GameManager.instance.money = moneyObject.moneyAmount; // Pega a quantia de dinheiro que est� no GameObject Money e coloca no GameManager.
            }
            Destroy(moneyObject.gameObject); // Destr�i o GameObject do Money coletado.
        }
    }

}
