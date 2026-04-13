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

    // Things to keep track of (various timers as well as current audio)
    public GameObject currentAudioObject;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void VoiceForce(AudioClip clip)
    {
        // Forces the current Sam audio to cut and plays this instead
    }

    public void VoiceTryGoon(AudioClip clip)
    {
        // Logic goes here
        // If already playing voice sound (either Sam or goon), don't play anything
        // If 
        // If not, play goon voice and reset goon voice timer
    }

    public void VoiceTrySam()
    {
        // Logic goes here
        // 
    }
}
