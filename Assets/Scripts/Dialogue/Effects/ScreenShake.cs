using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform;  // A refer�ncia � Transform da c�mera
    //public RectTransform canvasTransform;  // A refer�ncia � Transform do Canvas
    //public RectTransform imageTransform;  // A refer�ncia � Transform da Imagem
    public float shakeDuration = 0.5f;  // Dura��o do chacoalhar
    public float shakeMagnitude = 0.1f;  // A intensidade do chacoalhar
    public float dampingSpeed = 1.0f;  // Velocidade que diminui o chacoalhar

    private Vector3 initialCameraPosition;
    private Vector3 initialCanvasPosition;
    private Vector3 initialImagePosition;
    private float originalShakeMagnitude;  // Para armazenar o valor original da magnitude

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        /*if (canvasTransform == null)
        {
            canvasTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
        }
        if (imageTransform == null)
        {
            imageTransform = GameObject.Find("Image").GetComponent<RectTransform>();  // Assumindo que sua imagem se chama "Image"
        }*/

        // Armazena as posi��es iniciais e a magnitude original
        initialCameraPosition = cameraTransform.localPosition;
        //initialCanvasPosition = canvasTransform.localPosition;
        //initialImagePosition = imageTransform.localPosition;
        originalShakeMagnitude = shakeMagnitude;  // Salva a magnitude original
    }

    public void TriggerShake()
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

            // Gera uma posi��o aleat�ria para a c�mera
            float camX = Random.Range(-1f, 1f) * shakeMagnitude;
            float camY = Random.Range(-1f, 1f) * shakeMagnitude;
            cameraTransform.localPosition = new Vector3(camX, camY, initialCameraPosition.z);

            // Gera uma posi��o aleat�ria para o Canvas
            /*float canvasX = Random.Range(-1f, 1f) * shakeMagnitude;
            float canvasY = Random.Range(-1f, 1f) * shakeMagnitude;
            canvasTransform.localPosition = new Vector3(canvasX, canvasY, initialCanvasPosition.z);

            // Gera uma posi��o aleat�ria para a Imagem
            float imageX = Random.Range(-1f, 1f) * shakeMagnitude;
            float imageY = Random.Range(-1f, 1f) * shakeMagnitude;
            imageTransform.localPosition = new Vector3(imageX, imageY, initialImagePosition.z);*/

            // Gradualmente reduz a magnitude do chacoalhar
            shakeMagnitude = Mathf.Lerp(shakeMagnitude, 0, elapsedTime / shakeDuration);

            yield return null;  // Espera o pr�ximo frame
        }

        // Retorna �s posi��es iniciais
        cameraTransform.localPosition = initialCameraPosition;
        //canvasTransform.localPosition = initialCanvasPosition;
        //imageTransform.localPosition = initialImagePosition;
    }
}
