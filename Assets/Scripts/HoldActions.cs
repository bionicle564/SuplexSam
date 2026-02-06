using UnityEngine;


public class HoldActions : MonoBehaviour
{
    public bool held;

    public void Grab()
    {
        held = true;
    }


    public void LetGo()
    {
        held = false;
    }
}