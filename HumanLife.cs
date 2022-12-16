using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanLife : MonoBehaviour
{
    public GameObject particlesHumanDeath;
    public int life = 10;

    public void hit()
    {
        life -= 1;
        if (life <= 0)
        {
            FindObjectOfType<Manager>().HumanDied(gameObject);
            Instantiate(particlesHumanDeath, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
