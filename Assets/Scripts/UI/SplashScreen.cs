using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float fadeInDuration = 2f; // Tempo para o fade in completar

    private void Start()
    {
        if (textComponent != null)
        {
            // Inicializa o texto com transparência total
            Color color = textComponent.color;
            color.a = 0f;
            textComponent.color = color;
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = textComponent.color;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeInDuration);
            textComponent.color = color;
            yield return null;
        }
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            // Carrega a cena "MainMenu" ao pressionar a barra de espaço
            SceneManager.LoadScene("MainMenu");
        }*/
    }
}
