using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAttackHuman : MonoBehaviour
{
    public Vector2 coords;
    public float attackingVelocity = 1f;
    public Manager manager;
    public Vector3 target;
    public Vector2 targetCoords;

    private void Start()
    {
        manager = FindObjectOfType<Manager>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, coords, attackingVelocity * Time.deltaTime);
        if (transform.position.x == coords.x && transform.position.y == coords.y)
        {
            coords = new Vector2(targetCoords.x + Random.insideUnitCircle.x * 3, targetCoords.y + Random.insideUnitCircle.y * 3);
        }
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
        targetCoords = target;
        coords = new Vector2(targetCoords.x + Random.insideUnitCircle.x * 3, targetCoords.y + Random.insideUnitCircle.y * 3); ;
    }
}
