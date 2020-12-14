using UnityEngine;

public class DoorScrpt : MonoBehaviour
{
    public bool Condition;
    public Animator Door;

    public Doortype DoorType;
    public enum Doortype
    {
        NormalDoor,
        OfficeDoor,
    }

    //[SerializeField] PlayerLocation PlayerLoc;
    //enum PlayerLocation { 
    //    Front,
    //    Back
    //}

    //bool FrontD;
    //bool BackD;
}