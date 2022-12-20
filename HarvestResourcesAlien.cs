using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestResourcesAlien : MonoBehaviour
{
    public Vector3 coorPlanet = Vector3.zero;
    public bool startHarvesting = false;
    public bool startReturn = false;
    public Vector3 dropPosition;
    //public Vector3 startingPos;
    public bool stopHarvesting = false;
    public float harvestingVelocity = 1f;
    public float returnVelocity = 1f;
    public bool taken = false;
    private Manager manager;
    public bool planetOutOfResources = false;
    public bool waterResource = false;
    public bool mineralResource = false;
    public bool gasResource = false;

    private void Start()
    {
        //startingPos = transform.position;
        manager = FindObjectOfType<Manager>();
        //dropPosition = startingPos;
        startHarvesting = true;
    }

    void Update()
    {
        if (startHarvesting)
            transform.position = Vector2.MoveTowards(transform.position, coorPlanet, harvestingVelocity * Time.deltaTime);
        if (taken)
        {
            startHarvesting = false;
            startReturn = true;
        }
        if (startReturn /*&& taken*/)
        {
            transform.position = Vector2.MoveTowards(transform.position, dropPosition, returnVelocity * Time.deltaTime);
        }
        //else if (startReturn && !taken)
        //    transform.position = Vector2.MoveTowards(transform.position, startingPos, harvestingVelocity * Time.deltaTime);

        if (transform.position == dropPosition && !planetOutOfResources)
        {
            startReturn = false;
            startHarvesting = true;
            if (taken && mineralResource)
            {
                taken = false;
                Destroy(transform.GetChild(0).gameObject);
                mineralResource = false;
            }
            else if (taken && waterResource)
            {
                taken = false;
                Destroy(transform.GetChild(0).gameObject);
                waterResource = false;
            }
            else if (taken && gasResource)
            {
                taken = false;
                Destroy(transform.GetChild(0).gameObject);
                gasResource = false;
            }
            // GetComponent<Collider2D>().enabled = true;
        }
        if (transform.position == dropPosition && planetOutOfResources)
        {
            GetComponent<Movement>().enabled = true;
            GetComponent<AlienJob>().SetIsFree();
            startReturn = false;
            startHarvesting = false;
            if (taken && mineralResource)
            {
                taken = false;
                Destroy(transform.GetChild(0).gameObject);
                mineralResource = false;
            }
            else if (taken && waterResource)
            {
                taken = false;
                Destroy(transform.GetChild(0).gameObject);
                waterResource = false;
            }
            else if (taken && gasResource)
            {
                taken = false;
                Destroy(transform.GetChild(0).gameObject);
                gasResource = false;
            }
            // GetComponent<Collider2D>().enabled = true;
            enabled = false;
        }
        if (stopHarvesting)
        {
            // GetComponent<Collider2D>().enabled = true;
            if (taken)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            startReturn = false;
            startHarvesting = true;
            stopHarvesting = false;
            taken = false;
            enabled = false;
        }

        if (transform.position == dropPosition && planetOutOfResources)
        {
            // GetComponent<Collider2D>().enabled = true;
            GetComponent<Movement>().enabled = true;
            GetComponent<AlienJob>().SetIsFree();
            startReturn = false;
            startHarvesting = false;
            enabled = false;
        }
    }

    public void SetPlanet(Vector3 coords, Vector3 myPlanet)
    {
        dropPosition = myPlanet;
        coorPlanet = coords;
        planetOutOfResources = false;
        if (!startReturn || !taken)
        {
            startReturn = false;
            startHarvesting = true;
        }
    }

    public void PlanetOutOfResources()
    {
        planetOutOfResources = true;
        startHarvesting = false;
        startReturn = true;
    }

    public void ResetHarvester()
    {
        startReturn = false;
        startHarvesting = false;
        stopHarvesting = false;
        taken = false;
        enabled = false;
    }
}
