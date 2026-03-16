using UnityEngine;

public class Level5Manager : MonoBehaviour
{
    // Opening area
    public BarrelDispenser area1Dispenser;
    //Enemy spawn points/combat encounters
    public Level5Encounter warehouseDoor1;
    public Level5Encounter warehouseDoor2;

    // One-way hallway
    //Trigger for that maybe (unless it being a solo object is better)
    //Switch puzzle piece
    //Enemy encounter x2

    // Boat jam
    //Barrier bypass
    //Lever/valve puzzle check

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
    }
}
