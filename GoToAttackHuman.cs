using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAttackHuman : MonoBehaviour
{
    public Vector2 coords;
    public float attackingVelocity = 2f;
    public Manager manager;
    public GameObject target;
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
            coords = new Vector2(targetCoords.x + Random.insideUnitCircle.x * target.transform.localScale.x,
               targetCoords.y + Random.insideUnitCircle.y * target.transform.localScale.x);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        targetCoords = target.transform.position;
        coords = new Vector2(targetCoords.x + Random.insideUnitCircle.x * target.transform.localScale.x,
               targetCoords.y + Random.insideUnitCircle.y * target.transform.localScale.x); ;
    }
}
