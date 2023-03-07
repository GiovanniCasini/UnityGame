using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoInFormationAlien : MonoBehaviour
{
    public Vector2 coords;
    public float InFormationVelocity = 1f;

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, coords, InFormationVelocity * Time.deltaTime);
        if (transform.position.x == coords.x && transform.position.y == coords.y)
        {
            GetComponent<AlienJob>().SetIsInFormation();
            GetComponent<CombatModeAlien>().StartCombatModeAlien();
            enabled = false;
        }
    }
}
