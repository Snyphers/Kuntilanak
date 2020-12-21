using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HantuCordination : MonoBehaviour
{
    [SerializeField] GameObject Budak;
    [SerializeField] GameObject Hantu;
    [SerializeField] GameObject Currentplace;

    [SerializeField] Transform[] Basement;
    [SerializeField] Transform[] Floor1;
    [SerializeField] Transform[] Floor2;

    [SerializeField] Transform Level2Post;

    public bool BasemenetBool;
    public bool Floor1Bool;
    public bool Floor2Bool;
    public bool OfficeDoor;
    public bool OnStairs;

    void Start()
    {
        Currentplace = Instantiate(Hantu, Floor1[1]);
    }

    public void InstaBasement()
    {
        Currentplace = Instantiate(Hantu, Basement[(Random.Range(0, 2))]);
    }

    public void InstaFloor1()
    {
        Currentplace = Instantiate(Hantu, Floor1[(Random.Range(0, 2))]);
    }

    public void InstaFloor2()
    {
        Currentplace = Instantiate(Hantu, Floor2[(Random.Range(0, 2))]);
    }

    public void DesrtroyThekid()
    {
        Destroy(Budak);
    }

    public IEnumerator RespawnHantu()
    {
        yield return new WaitForSeconds(1.5f);
        HantuKejar();
    }

    public void HantuKejar()
    {
        Currentplace = Instantiate(Hantu, Level2Post);
    }

    public void DestroyHantu()
    {
        Destroy(Currentplace);
    }
}