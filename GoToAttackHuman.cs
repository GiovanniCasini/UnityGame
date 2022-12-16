using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAttackHuman : MonoBehaviour
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
            coords = new Vector2(manager.alienPlanet.transform.position.x + Random.insideUnitCircle.x * manager.alienPlanetScale,
                manager.alienPlanet.transform.position.y + Random.insideUnitCircle.y * manager.alienPlanetScale);
        }
    }
}
