using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanJob : MonoBehaviour
{
    public bool attacking = false;
    public bool harvesting = false;
    public bool goingIntoFormation = false;
    public bool inFormation = false;
    public bool free = true;

    public void SetIsAttacking()
    {
        attacking = true;
        harvesting = false;
        goingIntoFormation = false;
        inFormation = false;
        free = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
    }

    public void SetIsHarvesting()
    {
        attacking = false;
        harvesting = true;
        goingIntoFormation = false;
        inFormation = false;
        free = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
    }

    public void SetIsGoingIntoFormation()
    {
        attacking = false;
        harvesting = false;
        goingIntoFormation = true;
        inFormation = false;
        free = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
    }

    public void SetIsInFormation()
    {
        attacking = false;
        harvesting = false;
        goingIntoFormation = false;
        inFormation = true;
        free = false;
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
    }

    public void SetIsFree()
    {
        attacking = false;
        harvesting = false;
        goingIntoFormation = false;
        inFormation = false;
        free = true;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
}
