using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("WeaponGauge")]
    private int primaryCurrentAmmo;
    private int primaryMaxAmmo;
    [SerializeField] TMP_Text primaryAmmo;

    private int secondaryCurrentAmmo;
    private int secondaryMaxAmmo;
    [SerializeField] TMP_Text secondaryAmmo;

    [Header("Weapon Icons")]
    [SerializeField] Image Slot1;
    [SerializeField] Image Slot2;
    [SerializeField] Sprite noWeapon;
    [SerializeField] Sprite unkwnownWeapon;

    /*[[SerializeField] Sprite[] Weapon;

        ///0 - Sem Arma 
        ///1 - Pistola
        ///2 - Thompson
        ///3 - Escopeta
        ///4 - M1917
        ///5 - Revólver
        ///6 - Soco Inglês
        ///7 - Face 
        ///8 - Pé de Cabra
        ///9 */


    [Header("ReloadIcon")]
    [SerializeField] Slider ReloadSlider;



    [Header("Health")]
    [SerializeField] int NumberOfHearts;
    [SerializeField] Image[] Hearts;
    [SerializeField] Sprite fullHealth;
    [SerializeField] Sprite emptyHealth;
    [SerializeField] float health;

    [Header("Money")]
    [SerializeField] int playerMoney;
    [SerializeField] TMP_Text moneyText;


    GameObject player;
    void Start()
    {

        player = GameObject.Find("Player");

    }

    void Update()
    {
        if (player != null)
        {
            GunsPickup gunsPickupPlayer = player.GetComponentInChildren<GunsPickup>();

            GameObject primarySlot = gunsPickupPlayer.inventory[gunsPickupPlayer.selectGun];
            if (primarySlot != null)
            {
                GunsPlayer primaryGun = primarySlot.GetComponentInChildren<GunsPlayer>();
                if (primaryGun != null)
                {
                    primaryMaxAmmo = primaryGun.maxAmmo;
                    primaryCurrentAmmo = primaryGun.currentAmmo;

                    if (!primaryGun.hudIcon)
                    {
                        Slot1.sprite = unkwnownWeapon;

                    }
                    else {
                        Slot1.sprite = primaryGun.hudIcon;
                    }
                    

                    primaryAmmo.text = primaryCurrentAmmo.ToString() + "/<size=50%>" + primaryMaxAmmo.ToString();
                }
            }
            else {
                Slot1.sprite = noWeapon;
                primaryAmmo.text = "";
            }
            
            GameObject SecondarySlot = gunsPickupPlayer.inventory[(gunsPickupPlayer.selectGun + 1) % 2];
            if (SecondarySlot != null)
            {
                GunsPlayer SecondaryGun = SecondarySlot.GetComponentInChildren<GunsPlayer>();
                if (SecondaryGun != null)
                {
                    secondaryMaxAmmo = SecondaryGun.maxAmmo;
                    secondaryCurrentAmmo = SecondaryGun.currentAmmo;

                    if (!SecondaryGun.hudIcon)
                    {
                        Slot2.sprite = unkwnownWeapon;

                    }
                    else
                    {
                        Slot2.sprite = SecondaryGun.hudIcon;
                    }

                    secondaryAmmo.text = secondaryCurrentAmmo.ToString() + "/<size=50%>" + secondaryMaxAmmo.ToString();
                }
            }
            else {
                Slot2.sprite = noWeapon;
                secondaryAmmo.text = "";
            }

            PlayerDamage playerInfo = player.GetComponent<PlayerDamage>();
            health = playerInfo.lives;
        }
        else
        {
            health = 0;

        }

        UpdateHealth();
        UpdateMoney();
    }

    void UpdateHealth()
    {

        if (health > NumberOfHearts)
        {
            health = NumberOfHearts;
        }

        for (int i = 0; i < Hearts.Length; i++)
        {

            if (i < health)
            {
                Hearts[i].sprite = fullHealth;
            }
            else
            {
                Hearts[i].sprite = emptyHealth;
            }

            if (i < NumberOfHearts)
            {
                Hearts[i].enabled = true;
            }
            else
            {
                Hearts[i].enabled = false;
            }
        }
    }

    void UpdateMoney()
    {
        moneyText.text = "x" + GameManager.instance.gameData.money.ToString("D3");
    }
}
