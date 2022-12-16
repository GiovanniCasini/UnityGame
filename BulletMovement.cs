using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public GameObject target;
    public float bulletVelocity = 4f;
    public Vector3 targetPos;
    public string targetTag;

    private void Start()
    {
        targetPos = target.transform.position;
        targetTag = target.tag;
        StartCoroutine(SelfDestruct());
    }

    void Update()
    {
        if (target != null)
        {
            targetPos = target.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, bulletVelocity * Time.deltaTime);
            transform.right = targetPos - transform.position;
        }
        else
            transform.Translate(Vector2.right * bulletVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Alien" && targetTag == "Alien")
        {
            collision.GetComponent<AlienLife>().hit();
            Destroy(gameObject);
        }
        else if (collision.tag == "Human" && targetTag == "Human")
        {
            collision.GetComponent<HumanLife>().hit();
            Destroy(gameObject);
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
