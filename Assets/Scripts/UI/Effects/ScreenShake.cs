using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform;  // Referência à Transform da câmera
    public float shakeDuration = 0.5f;  // Duração do chacoalhar
    public float shakeMagnitude = 0.1f;  // Intensidade do chacoalhar
    public float dampingSpeed = 1.0f;  // Velocidade que diminui o chacoalhar

    private Vector3 initialCameraPosition;
    private float originalShakeMagnitude;  // Para armazenar o valor original da magnitude

    void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Arraste a câmera no Screenshake do DialogueManager");
            return;
        }

        // Armazena a posição inicial da câmera e a magnitude original
        initialCameraPosition = cameraTransform.localPosition;
        originalShakeMagnitude = shakeMagnitude;
    }

    public void TriggerShake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Gera uma posição aleatória para a câmera
            float camX = Random.Range(-1f, 1f) * originalShakeMagnitude;
            float camY = Random.Range(-1f, 1f) * originalShakeMagnitude;
            cameraTransform.localPosition = initialCameraPosition + new Vector3(camX, camY, 0);

            yield return null;  // Espera o próximo frame
        }

        // Retorna à posição inicial
        cameraTransform.localPosition = initialCameraPosition;
    }
}
