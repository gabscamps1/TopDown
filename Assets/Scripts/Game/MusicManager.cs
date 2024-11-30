using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource currentAudioSource;

    private void Awake()
    {
        // Garante que apenas uma instância exista
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
    /// Troca a música atual para a música do AudioSource fornecido.
    /// </summary>
    /// <param name="newAudioSource">O AudioSource configurado com o áudio da cena.</param>
    public void PlayMusic(AudioSource newAudioSource)
    {
        if (newAudioSource == null || newAudioSource.clip == null)
        {
            Debug.LogWarning("Nenhum AudioSource válido foi fornecido!");
            return;
        }

        // Para a música atual, se houver
        if (currentAudioSource != null && currentAudioSource.isPlaying)
        {
            currentAudioSource.Stop();
        }

        // Configura o novo AudioSource
        currentAudioSource = newAudioSource;
        currentAudioSource.loop = true; // Garante o loop
        currentAudioSource.Play();

        //Debug.Log($"Tocando nova música: {newAudioSource.clip.name}");
    }
}
