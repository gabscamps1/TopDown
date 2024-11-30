using UnityEngine;
using System.Collections;

public class ShakeScreenEffect : MonoBehaviour
{
    private Vector3 initialPosition;
    private Coroutine shakeCoroutine;

    private void OnEnable()
    {
        // Define a posi��o inicial como a posi��o atual quando o script � ativado
        initialPosition = transform.position;
    }

    /// <summary>
    /// Inicia o efeito de tremor de tela.
    /// </summary>
    /// <param name="duration">Dura��o do tremor em segundos.</param>
    /// <param name="magnitude">Intensidade do tremor.</param>
    public void Shake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Pega a posi��o atual para evitar conflito com movimenta��es
            initialPosition = transform.position;

            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(initialPosition.x + offsetX, initialPosition.y + offsetY, initialPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Garante que a c�mera volte � posi��o din�mica
        transform.position = initialPosition;
    }
}
