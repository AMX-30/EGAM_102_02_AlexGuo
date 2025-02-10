using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip musicClip;    

    private static AudioManager instance;  

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    void Start()
    {
        if (audioSource != null && musicClip != null)
        {
            audioSource.clip = musicClip;  
            audioSource.loop = true;       
            audioSource.Play();            
        }
    }
}