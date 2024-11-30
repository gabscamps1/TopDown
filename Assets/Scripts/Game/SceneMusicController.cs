using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    public AudioSource sceneAudioSource;

    private void Start()
    {
        if (sceneAudioSource != null)
        {
            MusicManager.Instance.PlayMusic(sceneAudioSource);
        }
        else
        {
            Debug.LogWarning("Nenhum AudioSource foi atribuído nesta cena.");
        }
    }
}
