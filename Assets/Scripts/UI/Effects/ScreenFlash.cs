using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    public Image flashImage;  // A imagem de overlay branca que cobrirá a tela
    public float flashDuration = 0.5f;  // Duração do flash
    public float maxAlpha = 1f;  // O valor máximo de opacidade
    [SerializeField] private AudioClip flashScreenSound;


    void Start()
    {
        // Certifica-se de que a imagem esteja totalmente transparente no início
        flashImage.color = new Color(1, 1, 1, 0);
    }

    public void FlashScreen()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        // Fase 1: Aumentar a opacidade (flash)
        float time = 0;
        while (time < flashDuration / 2)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0, maxAlpha, time / (flashDuration / 2));
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // Fase 2: Diminuir a opacidade (voltar ao normal)
        time = 0;

        SoundFXManager.instance.PlaySoundFXClip(flashScreenSound, transform, 1f);
        while (time < flashDuration / 2)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(maxAlpha, 0, time / (flashDuration / 2));
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // Certifica-se de que a opacidade final seja zero
        flashImage.color = new Color(1, 1, 1, 0);
    }
}
