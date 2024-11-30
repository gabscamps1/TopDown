using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource currentAudioSource;

    private void Awake()
    {
        // Garante que apenas uma inst�ncia exista
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Troca a m�sica atual para a m�sica do AudioSource fornecido.
    /// </summary>
    /// <param name="newAudioSource">O AudioSource configurado com o �udio da cena.</param>
    public void PlayMusic(AudioSource newAudioSource)
    {
        if (newAudioSource == null || newAudioSource.clip == null)
        {
            Debug.LogWarning("Nenhum AudioSource v�lido foi fornecido!");
            return;
        }

        // Para a m�sica atual, se houver
        if (currentAudioSource != null && currentAudioSource.isPlaying)
        {
            currentAudioSource.Stop();
        }

        // Configura o novo AudioSource
        currentAudioSource = newAudioSource;
        currentAudioSource.loop = true; // Garante o loop
        currentAudioSource.Play();

        //Debug.Log($"Tocando nova m�sica: {newAudioSource.clip.name}");
    }
}
