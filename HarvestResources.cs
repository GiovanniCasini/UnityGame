using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestResources : MonoBehaviour
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
    public bool iceResource = false;
    public bool mineralResource = false;
    public bool gasResource = false;

    public int myPlanetId = -1;
    public Vector3 myPlanetCoords;

    private void Start()
    {
        //startingPos = transform.position;
        //dropPosition = startingPos;
        manager = FindObjectOfType<Manager>();
        startHarvesting = true;
        myPlanetId = manager.GetMyPlanetId(gameObject);
        myPlanetCoords = manager.WhereAmIFrom(gameObject);
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
                manager.AddMineralResource();
                mineralResource = false;
            }
            else if (taken && iceResource)
            {
                taken = false;
                Destroy(transform.GetChild(0).gameObject);
                manager.AddIceResource();
                iceResource = false;
            }
            else if (taken && gasResource)
            {
                taken = false;
                Destroy(transform.GetChild(0).gameObject);
                manager.AddGasResource();
                gasResource = false;
            }
        }
        if (transform.position == dropPosition && planetOutOfResources)
        {
            if (manager.selectedPlanetsToHarvestHumanList[myPlanetId].Count > 0)
            {
                SetPlanet(manager.HarvestingPlanetsDistributor(myPlanetId), myPlanetCoords);
                startReturn = false;
                startHarvesting = true;
                planetOutOfResources = false;
            }
            else
            {
                GetComponent<Movement>().enabled = true;
                startReturn = false;
                startHarvesting = false;
                if (taken && mineralResource)
                {
                    taken = false;
                    Destroy(transform.GetChild(0).gameObject);
                    manager.AddMineralResource();
                    mineralResource = false;
                }
                else if (taken && iceResource)
                {
                    taken = false;
                    Destroy(transform.GetChild(0).gameObject);
                    manager.AddIceResource();
                    iceResource = false;
                }
                else if (taken && gasResource)
                {
                    taken = false;
                    Destroy(transform.GetChild(0).gameObject);
                    manager.AddGasResource();
                    gasResource = false;
                }
                enabled = false;
            }
        }
        if (stopHarvesting)
        {
            GetComponent<Movement>().enabled = true;
            GetComponent<HumanJob>().SetIsFree();
            manager.UpdateSlider();
            manager.UpdateHarvestingSlider();
            if (taken)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            startReturn = false;
            startHarvesting = false;
            stopHarvesting = false;
            taken = false;
            enabled = false;
        }
        
        //if (transform.position == startingPos && planetOutOfResources)
        //{
        //    // GetComponent<Collider2D>().enabled = true;
        //    GetComponent<Movement>().enabled = true;
        //    startReturn = false;
        //    startHarvesting = false;
        //    enabled = false;
        //}
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
