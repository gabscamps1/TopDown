using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("WeaponGauge")]
    [SerializeField] GameObject hudWeapons;
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

    [Header("ReloadIcon")]
    [SerializeField] Slider ReloadSlider;
    [SerializeField] Image ReloadIcon;

    [Header("Health")]
    [SerializeField] GameObject hudHealth;
    [SerializeField] int NumberOfHearts;
    [SerializeField] Image[] Hearts;
    [SerializeField] Sprite fullHealth;
    [SerializeField] Sprite emptyHealth;
    [SerializeField] float health;
    [SerializeField] bool isDead;

    [Header("Low Health")]
    public GameObject lowHealthImage;
    public GameObject hudElement;
    [SerializeField] private AudioClip playerHeartbeat;
    private bool isHeartbeatPlaying = false;
    private Image heartImage;
    private Color heartOriginalColor;

    [Header("Money")]
    [SerializeField] GameObject hudMoney;
    [SerializeField] int playerMoney;
    [SerializeField] TMP_Text moneyText;

    [Header("Death")]
    public GameObject deathScreen;
    public TMP_Text deathCount;
    public TMP_Text deathMoneyText;
    public static bool isInDeathScreen;

    [Header("Store")]
    [SerializeField]GameObject hudStore;


    GameObject player;
    void Start()
    {
        player = GameManager.instance.player;

        if (hudElement != null)
        {
            heartImage = hudElement.GetComponent<Image>();
            if (heartImage != null)
            {
                heartOriginalColor = heartImage.color; // Salva a cor original
            }
        }
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
                    else
                    {
                        Slot1.sprite = primaryGun.hudIcon;
                    }


                    primaryAmmo.text = primaryCurrentAmmo.ToString() + "/<size=50%>" + primaryMaxAmmo.ToString();
                }
            }
            else
            {
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
            else
            {
                Slot2.sprite = noWeapon;
                secondaryAmmo.text = "";
            }

            PlayerDamage playerInfo = player.GetComponent<PlayerDamage>();
            if (playerInfo != null)
            {
                health = playerInfo.lives;
            }

            if (DialogueManager.isTalking)
            {
                hudHealth.SetActive(false);
                hudMoney.SetActive(false);
                hudWeapons.SetActive(false);
                //hudStore.SetActive(true);
            }
            else {
                hudHealth.SetActive(true);
                hudMoney.SetActive(true);
                hudWeapons.SetActive(true);
                //hudStore.SetActive(true);
            }
            UpdateHealth();
            UpdateMoney();

        }
        else
        {
            PlayerDied();
        }

    }

    void UpdateHealth()
    {
        if (health == 1 && !isHeartbeatPlaying)
        {
            lowHealthImage.SetActive(true);
            StartHeartbeat();

        }
        else if (health > 1 && isHeartbeatPlaying)
        {
            lowHealthImage.SetActive(false);
            StopHeartbeat();

        }

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

    void PlayerDied()
    {
        PauseMenu.CanPause = false;
        if (deathScreen != null) deathScreen.SetActive(true);

        if (GameManager.instance != null)
        {
            deathCount.text = "<size=50%>x</size>" + GameManager.instance.gameData.deaths.ToString("D2");
            deathMoneyText.text = "<size=50%>x</size>" + GameManager.instance.gameData.money.ToString("D3");
        }

        hudHealth.SetActive(false);
        hudMoney.SetActive(false);
        hudWeapons.SetActive(false);
    }

    void UpdateMoney() {
        if (GameManager.instance != null) moneyText.text = "x" + GameManager.instance.gameData.money.ToString("D3");
    }

    public void OpenStore()
    {
        print("LojaAberta");
        hudHealth.SetActive(false);
        hudMoney.SetActive(false);
        hudWeapons.SetActive(false);
        hudStore.SetActive(true);
    }

    public void CloseStore() {
        PauseMenu.CanPause = true;
        hudHealth.SetActive(true);
        DialogueManager.isTalking = false;
        hudMoney.SetActive(true);
        hudWeapons.SetActive(true);
        hudStore.SetActive(false);
    }

    void StartHeartbeat(){
        isHeartbeatPlaying = true;
        InvokeRepeating(nameof(PlayHeartbeatSound), 0f, 1f); //repete a cada um segundo
        if (heartImage != null)
        {
            InvokeRepeating(nameof(FlashHUD), 0f, 0.5f); // pisca o icone de coração
        }
    }

    void StopHeartbeat(){
        isHeartbeatPlaying = false;
        CancelInvoke(nameof(PlayHeartbeatSound));
        if (heartImage != null)
        {
            CancelInvoke(nameof(FlashHUD));
            heartImage.color = heartOriginalColor; 
        }
    }

    void PlayHeartbeatSound(){
        SoundFXManager.instance.PlaySoundFXClip(playerHeartbeat, transform, 1f);
    }

    void FlashHUD(){
        if (heartImage != null)
        {
            heartImage.color = heartImage.color == heartOriginalColor ? Color.red : heartOriginalColor;
        }
    }
}
