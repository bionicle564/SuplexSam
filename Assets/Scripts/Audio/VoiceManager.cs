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

    public GameObject audioObjectPrefab;

    float samTimer = 0f;
    [SerializeField] float samTimerMax = 25f;
    float goonTimer = 0f;
    [SerializeField] float goonTimerMax = 25f;

    void Start()
    {
        
    }

    void Update()
    {
        if (samTimer > 0)
        {
            samTimer -= Time.deltaTime;
        }
        if (goonTimer > 0)
        {
            goonTimer -= Time.deltaTime;
        }
    }

    public void VoiceForceSam(AudioClip clip, Transform point, float volume, float spatialBlend)
    {
        // Forces the current Sam audio to cut and plays this instead
        // Goon audio is ignored, and is allowed to continue

        if (currentSamAudio != null)
        {
            Destroy(currentSamAudio);

            currentSamAudio = Instantiate(audioObjectPrefab, point.position, point.rotation);
            currentSamAudio.GetComponent<AudioSource>().volume = volume;
            currentSamAudio.GetComponent<AudioSource>().spatialBlend = spatialBlend;

            samTimer = samTimerMax + clip.length;
        }
    }

    public void VoiceTrySam(AudioClip clip, Transform point, float volume, float spatialBlend)
    {
        // Logic goes here
        // If already playing voice sound (either Sam or goon), don't play anything
        if (currentSamAudio != null)
        {
            return;
        }
        if (currentGoonAudio != null)
        {
            return;
        }
        // If Sam timer is not depleted, don't play (Probably unnecessary)
        if (samTimer > 0f)
        {
            return;
        }
        // If Sam timer is depleted, play goon voice line and reset Sam voice timer
        if (samTimer <= 0f)
        {
            currentSamAudio = Instantiate(audioObjectPrefab, point.position, point.rotation);
            //currentSamAudio.GetComponent<AudioSource>().clip = clip;
            currentSamAudio.GetComponent<AudioSource>().PlayOneShot(clip);
            currentSamAudio.GetComponent<AudioSource>().volume = volume;
            currentSamAudio.GetComponent<AudioSource>().spatialBlend = spatialBlend;
            currentSamAudio.GetComponent<KillAfterTime>().lifetime = clip.length;

            samTimer = samTimerMax + clip.length;
        }
    }

    public void VoiceTrySamHurt(AudioClip clip, Transform point, float volume, float spatialBlend)
    {
        // Logic goes here
        // If already playing voice sound (either Sam or goon), don't play anything
        if (currentSamAudio != null)
        {
            return;
        }
        if (currentGoonAudio != null)
        {
            return;
        }
        // If Sam timer is not depleted, don't play (Probably unnecessary)
        if (samTimer > 0f)
        {
            return;
        }
        // If Sam timer is depleted, play goon voice line and reset Sam voice timer
        if (samTimer <= 0f)
        {
            currentSamAudio = Instantiate(audioObjectPrefab, point.position, point.rotation);
            //currentSamAudio.GetComponent<AudioSource>().clip = clip;
            currentSamAudio.GetComponent<AudioSource>().PlayOneShot(clip);
            currentSamAudio.GetComponent<AudioSource>().volume = volume;
            currentSamAudio.GetComponent<AudioSource>().spatialBlend = spatialBlend;
            currentSamAudio.GetComponent<KillAfterTime>().lifetime = clip.length;

            samTimer = samTimerMax / 4 + clip.length;
        }
    }

    public void VoiceTryGoon(AudioClip clip, Transform point, float volume, float spatialBlend)
    {
        // Logic goes here
        // If already playing voice sound (either Sam or goon), don't play anything
        if (currentSamAudio != null)
        {
            return;
        }
        if (currentGoonAudio != null)
        {
            return;
        }
        // If goon timer is not depleted, don't play (Probably unnecessary)
        if (goonTimer > 0f)
        {
            return;
        }
        // If goon timer is depleted, play goon voice line and reset goon voice timer
        if (goonTimer <= 0f)
        {
            currentGoonAudio = Instantiate(audioObjectPrefab, point.position, point.rotation);
            //currentGoonAudio.GetComponent<AudioSource>().clip = clip;
            currentGoonAudio.GetComponent<AudioSource>().PlayOneShot(clip);
            currentGoonAudio.GetComponent<AudioSource>().volume = volume;
            currentGoonAudio.GetComponent<AudioSource>().spatialBlend = spatialBlend;
            currentGoonAudio.GetComponent<KillAfterTime>().lifetime = clip.length;

            goonTimer = goonTimerMax + clip.length;
        }
    }
}
