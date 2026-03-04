using UnityEngine;

public class StickyPlatform : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    Transform platform;

    void Awake()
    {
        platform = this.transform;
    }

    void FixedUpdate()
    {
        //platform.Translate(0.01f, 0, 0);
        //platform.Translate(-0.01f, 0, 0);
        // ^ This might genuinely be the dumbest thing I've ever seen/done in my life
        // This code causes the platform to like... jitter?
        // It jitters back and forth in the same frame, allowing the gameobject to affect the player controller.
        // Without this, the controller is not affected by the platform.
        // UPDATE: This workaround solved the issue before but no longer! Only rotation is affected, not position
        // ???
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            other.gameObject.transform.parent = platform;
            //GameObject.FindGameObjectWithTag("CameraHolder").transform.parent = platform;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            other.gameObject.transform.Rotate(0, 360, 0);
            // Stupid workaround 2 Electric Boogaloo
            // I am collecting them all like pokemon
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            other.gameObject.transform.parent = null;
            //GameObject.FindGameObjectWithTag("CameraHolder").transform.parent = null;
        }
    }
}
