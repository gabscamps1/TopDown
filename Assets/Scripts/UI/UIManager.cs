using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using JetBrains.Annotations;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    [Header("WeaponGauge")]
    private int primaryCurrentAmmo;
    private int primaryMaxAmmo;
    [SerializeField] TMP_Text primaryAmmo;

    private int secondaryCurrentAmmo;
    private int secondaryMaxAmmo;
    [SerializeField] TMP_Text secondaryAmmo;


    [Header("ReloadIcon")]



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

                    primaryAmmo.text = primaryCurrentAmmo.ToString() + "/<size=50%>" + primaryMaxAmmo.ToString();
                }
            }
            else {
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

                    secondaryAmmo.text = secondaryCurrentAmmo.ToString() + "/<size=50%>" + secondaryMaxAmmo.ToString();
                }
            }
            else {
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
