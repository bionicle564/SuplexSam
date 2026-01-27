using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    private bool stunned;

    public void SetStunned(bool value)
    {
        stunned = value;
    }

    public bool IsStunned()
    {
        return stunned;
    }
}