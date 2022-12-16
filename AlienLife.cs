using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienLife : MonoBehaviour
{
    public GameObject particlesAlienDeath;
    public int life = 1;

    public void hit()
    {
        life -= 1;
        if (life <= 0)
        {
            FindObjectOfType<Manager>().RemoveAlien(gameObject);
            Instantiate(particlesAlienDeath, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
