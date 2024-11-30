using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    Button continueButton;
    // Start is called before the first frame update
    void Start()
    {
        continueButton = GetComponent<Button>();

        if (continueButton != null)
        {
            bool hasSave = GameObject.Find("GameManager").GetComponent<SaveData>().HasSave();
            if (!hasSave)
            {
                //continueButton.interactable = false;

                Color corPersonalizada = new Color(1, 1, 1f, 0.5f);
                continueButton.GetComponent<ButtonHoverEffectTMP>().enabled = false;
                continueButton.GetComponentInChildren<TextMeshProUGUI>().color = corPersonalizada;
            }
        }
        
    }
}
