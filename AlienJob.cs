using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienJob : MonoBehaviour
{
    public bool attacking = false;
    public bool goingIntoFormation = false;
    public bool inFormation = false;
    public bool free = true;
    public bool harvesting = false;

    public void SetIsAttacking()
    {
        harvesting = false;
        attacking = true;
        goingIntoFormation = false;
        inFormation = false;
        free = false;
    }

    public void SetIsGoingIntoFormation()
    {
        harvesting = false;
        attacking = false;
        goingIntoFormation = true;
        inFormation = false;
        free = false;
    }

    public void SetIsInFormation()
    {
        harvesting = false;
        attacking = false;
        goingIntoFormation = false;
        inFormation = true;
        free = false;
    }

    public void SetIsFree()
    {
        harvesting = false;
        attacking = false;
        goingIntoFormation = false;
        inFormation = false;
        free = true;
    }

    public void SetIsHarvesting()
    {
        harvesting = true;
        attacking = false;
        goingIntoFormation = false;
        inFormation = false;
        free = false;
    }
}
