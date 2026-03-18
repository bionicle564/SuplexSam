using UnityEngine;

public class Vocals : MonoBehaviour
{
    // Potentially switch up if we want more than just Sam to have subtitles
    [SerializeField] AudioSource source;

    public static Vocals instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
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

        SubtitleUI.instance.SetSubtitle(obj.subtitle, obj.clip.length);
    }
}
