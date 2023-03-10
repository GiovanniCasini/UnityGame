using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoInFormationHuman : MonoBehaviour
{
    public Vector2 coords;
    public float inFormationVelocity = 1f;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, coords, inFormationVelocity * Time.deltaTime);
        if (transform.position.x == coords.x && transform.position.y == coords.y)
        {
            GetComponent<HumanJob>().SetIsInFormation();
            enabled = false;
        }
    }
}
