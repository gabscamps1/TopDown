using System.Collections.Generic;
using UnityEngine;


public class GunsPickup : MonoBehaviour
{
    public int selectGun; // Arma selecionada.
    [SerializeField] float throwForce; // Força com que a arma será arremessada.
    public float damage; // Dano que o impacto da arma causará quando arremessada.
    public GameObject[] inventory; // Inventário das armas.
    [SerializeField] Transform gunPlaceholder; // Posição que as armas ficaram.
    public bool hasGun; // Confere se tem alguma arma no inventory. Usado para alterar animações no script de PlayerMovement.
    List<GameObject> gunList = new List<GameObject>(); // Lista das armas que estão no chão dentro do collider do GameObject ItemPlaceholder.

    void Update()
    {
        SelectGun(); // Ativar arma do slot selecionado.
        SelectSlot(); // Inputs para trocar de slot.

        // Input para pegar armas no chão.
        if (Input.GetKeyDown(KeyCode.F))
        {
            DisarmGun();
            SearchNearestGun();
        }

        hasGun = inventory[selectGun]; // Seta para true se tem alguma arma no inventory.
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Verificar se o objeto que colidiu é o jogador.
        if (collision.CompareTag("GunPlayer"))
        {
            // Adiciona o game object na lista se ele não estiver nela. 
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

    // Procura a arma mais próxima do Player.
    void SearchNearestGun()
    {
        GameObject nearestGun = null; // Arma mais próxima.
        float nearestDistance = Mathf.Infinity; // Distância da arma mais próxima.

        // Verifica a arma mais próxima.
        foreach (var gun in gunList)
        {
            float distance = Vector3.Distance(transform.position, gun.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestGun = gun; // Seleciona a arma mais próxima.
            }
        }

        // Se uma arma foi encontrada, equipe-a.
        if (nearestGun != null)
        {
            EquipGun(nearestGun);
        }
    }

    // Pega a arma do chão.
    private void EquipGun(GameObject nearestGun)
    {
        inventory[selectGun] = nearestGun; // Coloca no inventário a arma mais próxima.
        inventory[selectGun].transform.position = gunPlaceholder.position; // Coloca a arma no GunPlaceholder do Player.
        inventory[selectGun].transform.SetParent(transform); // Seta a arma como filha do Player.
        
        // Desativa a colisão da Arma.
        Collider2D colliderGun = inventory[selectGun].GetComponent<Collider2D>();
        if (colliderGun != null) 
            colliderGun.enabled = false;

        // Remove a arma coletada da lista de armas que estão no chão.
        gunList.Remove(inventory[selectGun]);

        // Altera váriaveis no código da arma que será equipada.
        GunsPlayer scriptGun = inventory[selectGun].GetComponent<GunsPlayer>();
        if (scriptGun != null)
        {
            scriptGun.isHold = true; // Player está segurando a arma equipada.
            scriptGun.player = transform.parent.gameObject; // Arma possui o gameObject do Player.
        }
            
    }

    void DisarmGun()
    {
        // Verificar se o jogador está perto e apertou o botão direito do mouse.
        if (inventory[selectGun] != null)
        {
            print("teste2");
            // Calcular a posição do mouse no mundo e a direção para arremessar.
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 throwDirection = (mousePosition - transform.position).normalized;

            // Altera váriaveis no código da arma que será tirada do inventário.
            GunsPlayer scriptGun = inventory[selectGun].GetComponent<GunsPlayer>();
            if (scriptGun != null)
            {
                scriptGun.isHold = false; // Player não está mais segurando a arma solta.
                scriptGun.player = null; // Arma não possui mais o gameObject do Player.
            }
                

            // Pega o Rigidbody2D da arma que será tirada do inventário.
            Rigidbody2D rb = inventory[selectGun].GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false; // Transforma o Rigidbody2D da arma em Dynamic.
                rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse); // Aplicar força na direção do mouse.
            }

            inventory[selectGun].transform.SetParent(null);  // Remove a arma como filho do Player.
            inventory[selectGun] = null; // Tira o GameObject da arma do inventário.
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

        // Desativa o slot não selecionado.
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
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Recebe a direção do scroll do mouse.

        // Input para o 1° Slot.
        if (Input.GetKeyDown(KeyCode.Alpha1) || scroll > 0)
        {
            selectGun = 0;
        }

        // Input para o 2° Slot.
        if (Input.GetKeyDown(KeyCode.Alpha2) || scroll < 0)
        {
            selectGun = 1;
        }
    }
}
