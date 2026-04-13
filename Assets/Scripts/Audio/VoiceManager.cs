using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    // Make singleton
    private static VoiceManager _instance;

    public static VoiceManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Things to keep track of (various timers as well as current audio(s))
    public GameObject currentSamAudio;
    public GameObject currentGoonAudio;

    public GameObject audioObject;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void VoiceForce(AudioClip clip)
    {
        // Forces the current Sam audio to cut and plays this instead
        // Goon audio is ignored, and allowed to continue
    }

    public void VoiceTryGoon(AudioClip clip)
    {
        // Logic goes here
        // If already playing voice sound (either Sam or goon), don't play anything
        // If goon timer is not depleted, don't play
        // If goon timer is depleted, play goon voice line and reset goon voice timer
    }

    public void VoiceTrySam(AudioClip clip)
    {
        // Logic goes here
        // If already playing voice sound (either Sam or goon), don't play anything
        // If Sam timer is not depleted, don't play
        // If Sam timer is depleted, play goon voice line and reset Sam voice timer
    }
}
