using UnityEngine;


public class HoldActions : MonoBehaviour
{
    public bool held;

    virtual public void Grab()
    {
        held = true;
    }


    virtual public void LetGo()
    {
        held = false;
    }
}