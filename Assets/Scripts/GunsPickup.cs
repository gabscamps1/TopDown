using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunsPickup : MonoBehaviour
{
    public int selectGun; // Arma selecionada.
    public float throwForce = 10f; // For�a com que o item ser� arremessado.
    public GameObject[] inventory; // Invent�rio das armas.
    List<GameObject> gunList = new List<GameObject>();

    void Update()
    {
        SelectGun();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectGun = 0;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectGun = 1;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            DisarmGun();
            EquipNearestGun();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Verificar se o objeto que colidiu � o jogador.
        if (collision.CompareTag("Gun"))
        {
            // guns[selectGun] != collision.gameObject &&
            if (!gunList.Contains(collision.gameObject)) gunList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verificar se o jogador saiu de perto do item.
        if (collision.CompareTag("Gun"))
        {
            gunList.Remove(collision.gameObject);
        }
    }

    
    void EquipNearestGun()
    {
        // Encontra todas as armas na cena
        GameObject nearestWeapon = null;
        float nearestDistance = Mathf.Infinity;

        // Verifica a arma mais pr�xima
        foreach (var gun in gunList)
        {
            float distance = Vector3.Distance(transform.position, gun.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestWeapon = gun;
            }
        }

        // Se uma arma foi encontrada, equipe-a
        if (nearestWeapon != null)
        {
            EquipGun(nearestWeapon);
        }
    }

    private void EquipGun(GameObject nearestWeapon)
    {
        nearestWeapon.transform.position = transform.position;
        nearestWeapon.transform.SetParent(transform);
        inventory[selectGun] = nearestWeapon;

        Collider2D colliderGun = inventory[selectGun].GetComponent<Collider2D>();
        if (colliderGun != null) 
            colliderGun.enabled = false;
        gunList.Remove(inventory[selectGun]);

        Guns scriptGun = inventory[selectGun].GetComponent<Guns>();
        if (scriptGun != null)
        {
            scriptGun.isHold = true;
            scriptGun.player = transform.parent.gameObject;
        }
            
    }

    void DisarmGun()
    {
        // Verificar se o jogador est� perto e apertou o bot�o direito do mouse.

        if (inventory[selectGun] != null)
        {
            // Calcular a posi��o do mouse no mundo e a dire��o para arremessar.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDirection = (mousePosition - transform.position).normalized;

            Guns scriptGun = inventory[selectGun].GetComponent<Guns>();
            if (scriptGun != null)
            {
                scriptGun.isHold = false;
                scriptGun.player = null;
            }
                

            // Aplicar for�a na dire��o do mouse
            Rigidbody2D rb = inventory[selectGun].GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
            }

            inventory[selectGun].transform.SetParent(null);  // Remove o item como filho do jogador.
            inventory[selectGun] = null;
        }
    }

    void SelectGun()
    {
        // Ativa a Arma selecionada.
        /*if (inventory[selectGun] != null)
        {
            inventory[selectGun].SetActive(true);
        }

        // Desativa a arma n�o selecionada.
        if (inventory[(selectGun + 1) % 2] != null)
        {
            inventory[(selectGun + 1) % 2].SetActive(false);
        }*/
        

        if (inventory[selectGun] != null)
        {
            inventory[selectGun].SetActive(true);
        }
        if (selectGun == 0 && inventory[1] != null)
        {
            inventory[1].SetActive(false);
        }
        else if (selectGun == 1 && inventory[0] != null)
        {
            inventory[0].SetActive(false);
        }


    }

}
