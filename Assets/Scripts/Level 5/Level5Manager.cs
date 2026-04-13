using UnityEngine;

public class Level5Manager : MonoBehaviour
{
    // Opening area
    public BarrelDispenser area1Dispenser;
    // Enemy spawn points/combat encounters
    public Level5Encounter warehouseDoor1;
    public Level5Encounter warehouseDoor2;

    // Switch puzzle piece
    public ProxyPickup missingPiece;
    public bool pieceCollected = false;

    // Goon barricade switch
    public GameObject goonSwitch;

    // Hydrant delivery + fire
    //Fire handling?
    //Boat animations (later)

    // Boss fight?
    //Figure this one out later

    void Start()
    {
        
    }

    void Update()
    {
        if (warehouseDoor1.isComplete && warehouseDoor2.isComplete && !area1Dispenser.isOn)
        {
            area1Dispenser.isOn = true;
        }

        if (missingPiece.isPickedUp)
        {
            pieceCollected = true;
            missingPiece.gameObject.SetActive(false);
            goonSwitch.SetActive(true);
        }
    }
}
