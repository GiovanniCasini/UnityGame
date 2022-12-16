using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MineralPlanet : MonoBehaviour
{
    public int resources = 1000;
    private int startingResources;
    public TMP_Text resourcesText;
    private Vector3 startingScale;
    public GameObject mineralResource;
    public List<GameObject> mineralResources;
    public bool alreadyChangedImage = false;
    public Sprite halfConsumedMineralPlanet;
    public Sprite consumedMineralPlanet;

    public void Start()
    {
        mineralResources = new List<GameObject>();
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
        if ((resources <= startingResources / 2) && !alreadyChangedImage)
        {
            alreadyChangedImage = true;
            GetComponent<SpriteRenderer>().sprite = halfConsumedMineralPlanet;
        }

        if (resources > 0)
        {
            if (collision.tag == "Human" && collision.GetComponent<HumanJob>().harvesting && !collision.GetComponent<HarvestResources>().taken)
            {
                mineralResources.Add(Instantiate(mineralResource, collision.transform.position, collision.transform.rotation));
                mineralResources[mineralResources.Count - 1].transform.parent = collision.transform;
                collision.GetComponent<HarvestResources>().mineralResource = true;
                collision.GetComponent<HarvestResources>().taken = true;
                resources--;
                resourcesText.text = resources.ToString();
                float newScale = transform.localScale.x - 0.001f;
                transform.localScale = new Vector3(newScale, newScale, 0);
            }
            else if (collision.tag == "Alien" && collision.GetComponent<AlienJob>().harvesting && !collision.GetComponent<HarvestResourcesAlien>().taken)
            {
                mineralResources.Add(Instantiate(mineralResource, collision.transform.position, collision.transform.rotation));
                mineralResources[mineralResources.Count - 1].transform.parent = collision.transform;
                collision.GetComponent<HarvestResourcesAlien>().mineralResource = true; ;
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
            GetComponent<SpriteRenderer>().sprite = consumedMineralPlanet;
            FindObjectOfType<Manager>().EndHarvesting(gameObject);
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }
}