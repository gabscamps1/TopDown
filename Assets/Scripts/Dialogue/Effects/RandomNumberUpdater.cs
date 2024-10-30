

using TMPro;
using UnityEngine;

public class RandomStringUpdater : MonoBehaviour
{
    public TextMeshProUGUI tmpText; // Refer�ncia para o componente TextMeshProUGUI
    public float updateInterval = 1.0f; // Intervalo de tempo entre as atualiza��es, em segundos
    public int minLength = 7; // Comprimento m�nimo da string gerada

    private float timer = 0f; // Temporizador para controlar o intervalo
    private const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%�&*()_+`{}^?:><"; // Poss�veis caracteres

    void Update()
    {
        // Incrementa o temporizador com o tempo passado desde o �ltimo frame
        timer += Time.deltaTime;

        // Verifica se o temporizador atingiu o intervalo definido
        if (timer >= updateInterval)
        {
            // Gera uma string aleat�ria
            string randomString = GenerateRandomString(minLength);

            // Atualiza o texto no componente TextMeshPro
            tmpText.text = randomString;

            // Reseta o temporizador
            timer = 0f;
        }
    }

    // Fun��o para gerar uma string aleat�ria com comprimento m�nimo especificado
    private string GenerateRandomString(int length)
    {
        char[] randomChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            randomChars[i] = characters[Random.Range(0, characters.Length)];
        }
        return new string(randomChars);
    }
}

