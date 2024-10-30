using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueEffects : MonoBehaviour
{
    [Header("Fade")]
    public Image fadeImage;  // Imagem do fade in e fade out
    public float fadeDuration = 1.0f;

    [Header("Dialogue Box")]
    public RectTransform dialogBox;  // Caixa de Diálogo
    private Vector3 initialDialogPosition;





    [Header("Screen Shake")]
    //Screen Shake
    public Transform cameraTransform;  // A referência à Transform da câmera
    //public RectTransform canvasTransform;  // A referência à Transform do Canvas
    //public RectTransform imageTransform;  // A referência à Transform da Imagem
    public float shakeDuration = 0.5f;  // Duração do chacoalhar
    public float shakeMagnitude = 0.1f;  // A intensidade do chacoalhar
    public float dampingSpeed = 1.0f;  // Velocidade que diminui o chacoalhar

    private Vector3 initialCameraPosition;
    private Vector3 initialCanvasPosition;
    private Vector3 initialImagePosition;
    private float originalShakeMagnitude;  // Para armazenar o valor original da magnitude




    void Start()
    {
        if (dialogBox != null)
        {
            initialDialogPosition = dialogBox.localPosition;  // Armazena a posição inicial da caixa de diálogo
        }


        //ScreenShake
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        initialCameraPosition = cameraTransform.localPosition;

    }

    public void TriggerFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    public void TriggerFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public void CenterDialog()
    {
        if (dialogBox != null)
        {
            dialogBox.localPosition = new Vector3(-77, 85, 0);
            //dialogBox.gameObject.SetActive(false);  // Oculta a caixa de diálogo
        }
    }

    public void MiddleDialog()
    {
        if (dialogBox != null)
        {
            dialogBox.localPosition = new Vector3(-77, 0, 0);
            //dialogBox.gameObject.SetActive(false);  // Oculta a caixa de diálogo
        }
    }


 
    public void ResetDialogPosition()
    {
        if (dialogBox != null)
        {
            dialogBox.localPosition = initialDialogPosition;
            //dialogBox.gameObject.SetActive(true);  // Exibe a caixa de diálogo
        }
    }

    /*public void TriggerShake()
    {
        shakeMagnitude = originalShakeMagnitude;  // Reseta a magnitude ao valor original
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Gera uma posição aleatória para a câmera
            float camX = Random.Range(-1f, 1f) * shakeMagnitude;
            float camY = Random.Range(-1f, 1f) * shakeMagnitude;
            cameraTransform.localPosition = new Vector3(camX, camY, initialCameraPosition.z);

            // Gradualmente reduz a magnitude do chacoalhar
            shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0, elapsedTime / shakeDuration);

            yield return null;  // Espera o próximo frame
        }

        // Retorna às posições iniciais
        cameraTransform.localPosition = initialCameraPosition;
    }*/

    private IEnumerator FadeInCoroutine()
    {
        Color color = fadeImage.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;
    }

    private IEnumerator FadeOutCoroutine()
    {
        Color color = fadeImage.color;
        float elapsed = 0f;

        if (color.a == 1){
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Lerp(1, 0, elapsed / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }

            color.a = 0;
            fadeImage.color = color;
        }

       
    }
}
