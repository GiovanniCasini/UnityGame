using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatModeAlien : MonoBehaviour
{
    Manager manager;
    GameObject target;
    public GameObject bulletPrefab;
    public bool alreadyStarted = false;

    void Awake()
    {
        manager = FindObjectOfType<Manager>();
    }

    IEnumerator CheckForHumans()
    {
        target = manager.GetClosestHuman(transform.position, 5f);
        if (target != null)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<SpriteRenderer>().color = Color.green;
            bullet.GetComponent<BulletMovement>().target = target;
        }
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(CheckForHumans());
    }

    public void StartCombatModeAlien()
    {
        if (!alreadyStarted)
        {
            alreadyStarted = true;
            StartCoroutine(CheckForHumans());
        }
    }

    public void StopCombatModeAlien()
    {
        alreadyStarted = false;
        StopAllCoroutines();
    }
}
