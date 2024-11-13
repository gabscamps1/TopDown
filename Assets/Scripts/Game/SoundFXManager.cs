using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume) {
        //Spawna o objeto de som
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //Define qual som vai ser tocado
        audioSource.clip = audioClip;

        //Define o volume do som 
        audioSource.volume = volume;

        //Duração do som
         float clipLenght = audioSource.clip.length;

        //Destrói o objeto de som
        Destroy(audioSource.gameObject, clipLenght);

    }
}
