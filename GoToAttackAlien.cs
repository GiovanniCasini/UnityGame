using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAttackAlien : MonoBehaviour
{
    public Vector2 coords;
    public float attackingVelocity = 2f;
    public Manager manager;

    private void Start()
    {
        manager = FindObjectOfType<Manager>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, coords, attackingVelocity * Time.deltaTime);
        if (transform.position.x == coords.x && transform.position.y == coords.y)
        {
            coords = new Vector2(Random.insideUnitCircle.x * manager.humanPlanets[0].transform.localScale.x, 
                Random.insideUnitCircle.y * manager.humanPlanets[0].transform.localScale.x);
        }
    }
}
