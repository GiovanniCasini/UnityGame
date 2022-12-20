using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IcePlanet : MonoBehaviour
{
    public int resources = 1000;
    private int startingResources;
    public TMP_Text resourcesText;
    private Vector3 startingScale;
    public GameObject iceResource;
    public List<GameObject> iceResources;
    private bool alreadyChangedImage = false;
    public Sprite halfConsumedIcePlanet;
    public Sprite consumedIcePlanet;

    public void Start()
    {
        iceResources = new List<GameObject>();
        resources = (int)Mathf.Round(Random.Range(10, 50) / 10) * 10;
        startingResources = resources;
        resourcesText.text = resources.ToString();
        float s = Mathf.Log(resources, 10);
        transform.localScale = new Vector3(s, s, 0);
        startingScale = transform.localScale;
        startingResources = resources;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if ((resources <= (startingResources / 2)) && !alreadyChangedImage)
        {
            alreadyChangedImage = true;
            GetComponent<SpriteRenderer>().sprite = halfConsumedIcePlanet;
        }

        if (resources > 0)
        {
            if (collision.tag == "Human" && collision.GetComponent<HumanJob>().harvesting && !collision.GetComponent<HarvestResources>().taken)
            {
                iceResources.Add(Instantiate(iceResource, collision.transform.position, collision.transform.rotation));
                iceResources[iceResources.Count - 1].transform.parent = collision.transform;
                collision.GetComponent<HarvestResources>().iceResource = true; ;
                collision.GetComponent<HarvestResources>().taken = true;
                resources--;
                resourcesText.text = resources.ToString();
                float newScale = transform.localScale.x - 0.001f;
                transform.localScale = new Vector3(newScale, newScale, 0);
            }
            else if (collision.tag == "Alien" && collision.GetComponent<AlienJob>().harvesting && !collision.GetComponent<HarvestResourcesAlien>().taken)
            {
                iceResources.Add(Instantiate(iceResource, collision.transform.position, collision.transform.rotation));
                iceResources[iceResources.Count - 1].transform.parent = collision.transform;
                collision.GetComponent<HarvestResourcesAlien>().iceResource = true;
                collision.GetComponent<HarvestResourcesAlien>().taken = true;
                resources--;
                resourcesText.text = resources.ToString();
                float newScale = transform.localScale.x - 0.001f;
                transform.localScale = new Vector3(newScale, newScale, 0);
            }
            else if (collision.tag != "Human" && collision.tag != "Alien" && collision.tag != "Bullet")
            {
                FindObjectOfType<Manager>().planetsToHarvest.Remove(gameObject);
                Destroy(gameObject);
            }
        }

        if (resources == 0)
        {
            GetComponent<SpriteRenderer>().sprite = consumedIcePlanet;
            FindObjectOfType<Manager>().EndHarvesting(gameObject);
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}
