using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip resetToIdle;

    [SerializeField]
    private AudioClip jump;

    [SerializeField]
    private AudioClip death;

    [SerializeField]
    private AudioClip scoreIncrease;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayResetClip()
    {
        audioSource.PlayOneShot(resetToIdle);
    }

    public void PlayJumpClip()
    {
        audioSource.PlayOneShot(jump);
    }

    public void PlayDeathClip()
    {
        audioSource.PlayOneShot(death);
    }

    public void PlayPointsGainedClip()
    {
        audioSource.PlayOneShot(scoreIncrease);
    }
}
