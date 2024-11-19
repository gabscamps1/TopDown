using System.Collections.Generic;
using UnityEngine;


public class GunsPickup : MonoBehaviour
{
    public int selectGun; // Arma selecionada.
    [SerializeField] float throwForce; // For�a com que a arma ser� arremessada.
    public float damage; // Dano que o impacto da arma causar� quando arremessada.
    public GameObject[] inventory; // Invent�rio das armas.
    [SerializeField] Transform gunPlaceholder; // Posi��o que as armas ficaram.
    public bool hasGun; // Confere se tem alguma arma no inventory. Usado para alterar anima��es no script de PlayerMovement.
    List<GameObject> gunList = new List<GameObject>(); // Lista das armas que est�o no ch�o dentro do collider do GameObject ItemPlaceholder.

    void Update()
    {
        SelectGun(); // Ativar arma do slot selecionado.
        SelectSlot(); // Inputs para trocar de slot.

        // Input para pegar armas no ch�o.
        if (Input.GetKeyDown(KeyCode.F))
        {
            DisarmGun();
            SearchNearestGun();
        }

        hasGun = inventory[selectGun]; // Seta para true se tem alguma arma no inventory.
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Verificar se o objeto que colidiu � o jogador.
        if (collision.CompareTag("GunPlayer"))
        {
            // Adiciona o game object na lista se ele n�o estiver nela. 
            if (!gunList.Contains(collision.gameObject)) gunList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verificar se o jogador saiu de perto do item.
        if (collision.CompareTag("GunPlayer"))
        {
            gunList.Remove(collision.gameObject); // Remove a arma da lista.
        }
    }

    // Procura a arma mais pr�xima do Player.
    void SearchNearestGun()
    {
        GameObject nearestGun = null; // Arma mais pr�xima.
        float nearestDistance = Mathf.Infinity; // Dist�ncia da arma mais pr�xima.

        // Verifica a arma mais pr�xima.
        foreach (var gun in gunList)
        {
            float distance = Vector3.Distance(transform.position, gun.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestGun = gun; // Seleciona a arma mais pr�xima.
            }
        }

        // Se uma arma foi encontrada, equipe-a.
        if (nearestGun != null)
        {
            EquipGun(nearestGun);
        }
    }

    // Pega a arma do ch�o.
    private void EquipGun(GameObject nearestGun)
    {
        inventory[selectGun] = nearestGun; // Coloca no invent�rio a arma mais pr�xima.
        inventory[selectGun].transform.position = gunPlaceholder.position; // Coloca a arma no GunPlaceholder do Player.
        inventory[selectGun].transform.SetParent(transform); // Seta a arma como filha do Player.
        
        // Desativa a colis�o da Arma.
        Collider2D colliderGun = inventory[selectGun].GetComponent<Collider2D>();
        if (colliderGun != null) 
            colliderGun.enabled = false;

        // Remove a arma coletada da lista de armas que est�o no ch�o.
        gunList.Remove(inventory[selectGun]);

        // Altera v�riaveis no c�digo da arma que ser� equipada.
        GunsPlayer scriptGun = inventory[selectGun].GetComponent<GunsPlayer>();
        if (scriptGun != null)
        {
            scriptGun.isHold = true; // Player est� segurando a arma equipada.
            scriptGun.player = transform.parent.gameObject; // Arma possui o gameObject do Player.
        }
            
    }

    void DisarmGun()
    {
        // Verificar se o jogador est� perto e apertou o bot�o direito do mouse.
        if (inventory[selectGun] != null)
        {
            print("teste2");
            // Calcular a posi��o do mouse no mundo e a dire��o para arremessar.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDirection = (mousePosition - transform.position).normalized;

            // Altera v�riaveis no c�digo da arma que ser� tirada do invent�rio.
            GunsPlayer scriptGun = inventory[selectGun].GetComponent<GunsPlayer>();
            if (scriptGun != null)
            {
                scriptGun.isHold = false; // Player n�o est� mais segurando a arma solta.
                scriptGun.player = null; // Arma n�o possui mais o gameObject do Player.
            }
                

            // Pega o Rigidbody2D da arma que ser� tirada do invent�rio.
            Rigidbody2D rb = inventory[selectGun].GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false; // Transforma o Rigidbody2D da arma em Dynamic.
                rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse); // Aplicar for�a na dire��o do mouse.
            }

            inventory[selectGun].transform.SetParent(null);  // Remove a arma como filho do Player.
            inventory[selectGun] = null; // Tira o GameObject da arma do invent�rio.
            print("teste5");
        }
    }

    void SelectGun()
    {
        // Ativa o slot selecionado.
        if (inventory[selectGun] != null)
        {
            inventory[selectGun].SetActive(true);
            inventory[selectGun].transform.position = gunPlaceholder.position; // Coloca a arma no GunPlaceholder do Player.
        }

        // Desativa o slot n�o selecionado.
        if (selectGun == 0 && inventory[1] != null)
        {
            inventory[1].SetActive(false); 
        }
        else if (selectGun == 1 && inventory[0] != null)
        {
            inventory[0].SetActive(false);
        }
    }

    void SelectSlot()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Recebe a dire��o do scroll do mouse.

        // Input para o 1� Slot.
        if (Input.GetKeyDown(KeyCode.Alpha1) || scroll > 0)
        {
            selectGun = 0;
        }

        // Input para o 2� Slot.
        if (Input.GetKeyDown(KeyCode.Alpha2) || scroll < 0)
        {
            selectGun = 1;
        }
    }
}
