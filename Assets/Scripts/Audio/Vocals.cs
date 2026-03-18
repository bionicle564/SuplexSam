using UnityEngine;

public class Vocals : MonoBehaviour
{
    // Potentially switch up if we want more than just Sam to have subtitles
    private AudioSource source;

    public static Vocals instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void Say(AudioObject obj)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }

        source.PlayOneShot(obj.clip);

        SubtitleUI.instance.SetSubtitle($"{obj.subtitle}");
    }
}
