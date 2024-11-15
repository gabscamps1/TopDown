using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class ItensPickUp : MonoBehaviour
{
    [SerializeField] private AudioClip moneyPickupSound;

    // Chama a função quando algum GameObject entra no cenário.
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Money"))
        {
            SoundFXManager.instance.PlaySoundFXClip(moneyPickupSound, transform, 1f);

            Money moneyObject = collision.GetComponent<Money>(); // Pega o Script do Money

            if (GameManager.instance != null)
            {
                GameManager.instance.money = moneyObject.moneyAmount; // Pega a quantia de dinheiro que está no GameObject Money e coloca no GameManager.
            }
            Destroy(moneyObject.gameObject); // Destrói o GameObject do Money coletado.
        }
    }

}
