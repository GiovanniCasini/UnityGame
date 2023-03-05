using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatModeHuman : MonoBehaviour
{
    Manager manager;
    GameObject target;
    public GameObject bulletPrefab;
    public bool alreadyStarted = false;

    void Awake()
    {
        manager = FindObjectOfType<Manager>();
    }

    IEnumerator CheckForAliens()
    {
        target = manager.GetClosestAlien(transform.position, 6f);
        if (target != null)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<SpriteRenderer>().color = Color.red;
            bullet.GetComponent<BulletMovement>().target = target;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(CheckForAliens());
    }

    public void StartCombatModeHuman()
    {
        if (!alreadyStarted)
        {
            alreadyStarted = true;
            StartCoroutine(CheckForAliens());
        }
    }

    public void StopCombatModeHuman()
    {
        alreadyStarted = false;
        StopAllCoroutines();
    }
}
