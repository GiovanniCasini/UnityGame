using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject humanPlanet;
    public float humanPlanetScale;

    public GameObject mineralResource;
    public List<GameObject> mineralResources;

    public GameObject waterResource;
    public List<GameObject> waterResources;

    public GameObject gasResource;
    public List<GameObject> gasResources;

    public GameObject alienPlanet;
    public List<GameObject> alienPlanets = new List<GameObject>();
    // public float alienPlanetScale;

    public GameObject human;
    public List<GameObject> humans;
    public int numStartingHumans = 2;
    public GameObject alien;
    public List<GameObject> aliens;
    public List<List<GameObject>> aliensList = new List<List<GameObject>>();
    public int numStartingAliens = 5;
    public CircleFormation circleFormation = new CircleFormation();

    public Slider slider;
    public Slider harvestingSlider;
    public Slider inFormationSlider;
    public Slider mineralResourcesSlider;
    public Slider waterResourcesSlider;
    public Slider gasResourcesSlider;

    public GameObject mineralPlanet;
    public GameObject waterPlanet;
    public GameObject gasPlanet;
    public List<GameObject> planetsToHarvest;
    public List<GameObject> selectedPlanetsToHarvest;
    private List<bool> isPlanetSelected;
    public int indexPlanetToHarvest = -1;

    public Button defOrAttButton;
    public Sprite shieldImage;
    public Sprite swordImage;
    public bool defenseOrAttackButton = true;
    public GameObject alienPlanetSelected;
    private bool isAlienPlanetSelected = false;

    public GameObject dottedLine;
    public List<GameObject> dottedLinesHarvestingHumans;
    public List<GameObject> dottedLinesAttackingHumans;

    public List<List<GameObject>> dottedLinesHarvestingAliensList = new List<List<GameObject>>();
    public List<GameObject> dottedLinesHarvestingAliens;
    public List<List<GameObject>> selectedPlanetsToHarvestAlienList = new List<List<GameObject>>();
    public List<GameObject> selectedPlanetsToHarvestAlien;

    public GameObject star;

    public GameObject bg_stars;
    public GameObject bg_starsFolder;

    void Start()
    {
        humans = new List<GameObject>();
        for (int i = 0; i < numStartingHumans; i++)
            humans.Add(Instantiate(human));

        humanPlanet = Instantiate(humanPlanet);
        humanPlanetScale = 0.5f * Mathf.Pow(find_next_square(humans.Count), 0.5f);
        humanPlanet.transform.localScale = new Vector3(humanPlanetScale, humanPlanetScale, 0f);
        for (int i = 0; i < humans.Count; i++)
            humans[i].GetComponent<Movement>().radius = humanPlanetScale / 2f;

        mineralResources = new List<GameObject>();
        waterResources = new List<GameObject>();
        gasResources = new List<GameObject>();

        for (int j = 0; j < 3; j++)
        {
            aliens = new List<GameObject>();
            aliensList.Add(aliens);
            selectedPlanetsToHarvestAlien = new List<GameObject>();
            dottedLinesHarvestingAliens = new List<GameObject>();
            alienPlanets.Add(Instantiate(alienPlanet, new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), 0f), transform.rotation));
            numStartingAliens = (int)Random.Range(5f, 15f);
            for (int i = 0; i < numStartingAliens; i++)
            {
                AddAlien(alienPlanets[j]);
                // aliens[i].GetComponent<Movement>().radius = alienPlanetScale / 2f;
            }
            selectedPlanetsToHarvestAlienList.Add(selectedPlanetsToHarvestAlien);
            dottedLinesHarvestingAliensList.Add(dottedLinesHarvestingAliens);
        }

        slider.maxValue = humans.Count;
        slider.value = humans.Count;
        harvestingSlider.maxValue = humans.Count;
        inFormationSlider.maxValue = humans.Count;

        for (int i = 0; i < 4; i++)
        {
            planetsToHarvest.Add(Instantiate(waterPlanet, new Vector3(Random.Range(-70f, 70f), Random.Range(-70f, 70f), 0f), transform.rotation));          
        }
        for (int i = 0; i < 10; i++)
        {
            planetsToHarvest.Add(Instantiate(mineralPlanet, new Vector3(Random.Range(-70f, 70f), Random.Range(-70f, 70f), 0f), transform.rotation));
        }
        for (int i = 0; i < 6; i++)
        {
            planetsToHarvest.Add(Instantiate(gasPlanet, new Vector3(Random.Range(-70f, 70f), Random.Range(-70f, 70f), 0f), transform.rotation));
        }

        selectedPlanetsToHarvest = new List<GameObject>();
        isPlanetSelected = new List<bool>();

        dottedLinesHarvestingHumans = new List<GameObject>();
        dottedLinesAttackingHumans = new List<GameObject>();

        Instantiate(star, new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f), transform.rotation);
        Instantiate(star, new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f), transform.rotation);

        for (int i = 0; i < Random.Range(1000, 1500); i++)
        {
            bg_stars = Instantiate(bg_stars, new Vector2(Random.Range(-70f, 70f), Random.Range(-70f, 70f)), transform.rotation);
            float starScale = Random.Range(0.02f, 0.08f);
            bg_stars.transform.localScale = new Vector3(starScale, starScale, starScale);
            bg_stars.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(0f, 0.8f));
            bg_stars.transform.parent = bg_starsFolder.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("n"))
        {
            AddHuman();
        }
        if (Input.GetKeyDown("m"))
        {
            AddAlien();
        }
        if (Input.GetKeyDown("f"))
        {
            for (int i = 0; i < aliensList.Count; i++)
            {
                AliensInFormation(i);
            }
        }
        if (Input.GetKeyDown("g"))
        {
            for (int i = 0; i < aliensList.Count; i++)
            {
                AliensFree(i);
            }
        }
        if (Input.GetKeyDown("t"))
        {
            for (int i = 0; i < aliensList.Count; i++)
            {
                StartAlienAttack(i);
            }
        }
        if (Input.GetKeyDown("h"))
        {
            for (int i = 0; i < aliensList.Count; i++)
            {
                AliensHarvesting(i);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 raycastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "MineralPlanet" || hit.collider.gameObject.tag == "WaterPlanet" || hit.collider.gameObject.tag == "GasPlanet")
                {
                    if (selectedPlanetsToHarvest.IndexOf(hit.collider.gameObject) == -1)
                    {
                        if (selectedPlanetsToHarvest.Count < 3)
                        {
                            selectedPlanetsToHarvest.Add(hit.collider.gameObject);
                            isPlanetSelected.Add(true);
                            HarvestersStartHarvesting();
                            dottedLinesHarvestingHumans.Add(Instantiate(dottedLine, transform));
                            dottedLinesHarvestingHumans[dottedLinesHarvestingHumans.Count - 1].GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                            dottedLinesHarvestingHumans[dottedLinesHarvestingHumans.Count - 1].GetComponent<LineRenderer>().SetPosition(1, selectedPlanetsToHarvest[selectedPlanetsToHarvest.Count - 1].transform.position);
                        }
                    }
                    else
                    {
                        int pos = selectedPlanetsToHarvest.IndexOf(hit.collider.gameObject);
                        Destroy(dottedLinesHarvestingHumans[pos]);
                        dottedLinesHarvestingHumans.RemoveAt(pos);
                        isPlanetSelected.RemoveAt(pos);
                        HarvestingAborted(selectedPlanetsToHarvest[pos]);
                        selectedPlanetsToHarvest.RemoveAt(pos);
                    }
                }
                else if (hit.collider.gameObject.tag == "AlienPlanet")
                {
                    if (!isAlienPlanetSelected)
                    {
                        alienPlanetSelected = hit.collider.gameObject;
                        isAlienPlanetSelected = true;
                        AttackersStartAttacking();
                        dottedLinesAttackingHumans.Add(Instantiate(dottedLine, transform));
                        dottedLinesAttackingHumans[dottedLinesAttackingHumans.Count - 1].GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                        dottedLinesAttackingHumans[dottedLinesAttackingHumans.Count - 1].GetComponent<LineRenderer>().SetPosition(1, alienPlanetSelected.transform.position);
                    }
                    else if (alienPlanetSelected == hit.collider.gameObject)
                    {
                        Destroy(dottedLinesAttackingHumans[dottedLinesAttackingHumans.Count - 1]);
                        dottedLinesAttackingHumans.RemoveAt(dottedLinesAttackingHumans.Count - 1);
                        isAlienPlanetSelected = false;
                        // defenseOrAttackButton = !defenseOrAttackButton;
                        HumansGoDefend();
                    }
                    else
                    {
                        Destroy(dottedLinesAttackingHumans[dottedLinesAttackingHumans.Count - 1]);
                        dottedLinesAttackingHumans.RemoveAt(dottedLinesAttackingHumans.Count - 1);

                        alienPlanetSelected = hit.collider.gameObject;
                        isAlienPlanetSelected = true;
                        AttackersStartAttacking();
                        dottedLinesAttackingHumans.Add(Instantiate(dottedLine, transform));
                        dottedLinesAttackingHumans[dottedLinesAttackingHumans.Count - 1].GetComponent<LineRenderer>().SetPosition(0, Vector3.zero);
                        dottedLinesAttackingHumans[dottedLinesAttackingHumans.Count - 1].GetComponent<LineRenderer>().SetPosition(1, alienPlanetSelected.transform.position);
                    }
                }
            }
        }
    }

    public void HumanDied(GameObject human)
    {
        RemoveHuman(human);
        int count = humans.Count;
        slider.maxValue = count;
        harvestingSlider.maxValue = count;
        inFormationSlider.maxValue = count;
        UpdateSlider();
        UpdateHarvestingSlider();
        UpdateInFormationSlider();
    }

    public void RemoveHuman(GameObject human)
    {
        humans.Remove(human);
    }

    public void RemoveAlien(GameObject alien)
    {
        for (int i = 0; i < aliensList.Count; i++)
        {
            if (aliensList[i].IndexOf(alien) != -1)
            {
                aliensList[i].Remove(alien);
                break;
            }
        }
    }

    public GameObject GetClosestHuman(Vector3 pos, float maxRange)
    {
        GameObject closest = null;
        foreach (GameObject human in humans)
        {
            if (Vector3.Distance(pos, human.transform.position) <= maxRange)
            {
                if (closest == null)
                    closest = human;
                else
                {
                    if (Vector3.Distance(pos, human.transform.position) < Vector3.Distance(pos, closest.transform.position))
                    {
                        closest = human;
                    }
                }
            }
        }
        return closest;
    }

    public GameObject GetClosestAlien(Vector3 pos, float maxRange)
    {
        GameObject closest = null;
        for (int i = 0; i < aliensList.Count; i++)
        {
            foreach (GameObject alien in aliensList[i])
            {
                if (Vector3.Distance(pos, alien.transform.position) <= maxRange)
                {
                    if (closest == null)
                        closest = alien;
                    else
                    {
                        if (Vector3.Distance(pos, alien.transform.position) < Vector3.Distance(pos, closest.transform.position))
                        {
                            closest = alien;
                        }
                    }
                }
            }
        }
        return closest;
    }

    public void AddHuman()
    {
        if (humans.Count < 50)
        {
            humans.Add(Instantiate(human));
            int count = humans.Count;
            humans[count - 1].GetComponent<Movement>().radius = humanPlanetScale / 2f;
            slider.maxValue = count;
            UpdateSlider();
            harvestingSlider.maxValue = count;
            inFormationSlider.maxValue = count;
            UpdatePlanet(count);
        }  
    }

    public void UpdatePlanet(int count)
    {
        if (count > 8 && count < 38)
        {
            if (Mathf.Sqrt(count - 1) % 1 == 0)
            {
                humanPlanetScale = 0.5f * Mathf.Pow(find_next_square(count), 0.5f);
                humanPlanet.transform.localScale = new Vector3(humanPlanetScale, humanPlanetScale, 0f);
                RecalculateFormation();
                for (int i = 0; i < count; i++)
                    humans[i].GetComponent<Movement>().radius = humanPlanetScale / 2f;
            }
        } 
    }

    public void AddAlien(GameObject alienPlanet)
    {
        int index = alienPlanets.IndexOf(alienPlanet);
        if (aliensList[index].Count < 50)
        {
            aliensList[index].Add(Instantiate(alien, alienPlanet.transform.position, transform.rotation));
            var count = aliensList[index].Count;
            float alienPlanetScale = alienPlanet.transform.localScale.x;
            aliensList[index][count - 1].GetComponent<Movement>().radius = alienPlanetScale / 2f;
            if (count > 8 && count < 38)
            {
                if (Mathf.Sqrt(count - 1) % 1 == 0)
                {
                    alienPlanetScale = 0.5f * Mathf.Pow(find_next_square(count), 0.5f);
                    alienPlanets[index].transform.localScale = new Vector3(alienPlanetScale, alienPlanetScale, 0f);
                    for (int i = 0; i < count; i++)
                        aliensList[index][i].GetComponent<Movement>().radius = alienPlanetScale / 2f;
                }
            }
            RecalculateFormationAliens(index);
        } 
    }

    public void AddAlien()
    {
        for (int i = 0; i < alienPlanets.Count; i++)
        {
            if (aliensList[i].Count < 50)
            {
                aliensList[i].Add(Instantiate(alien, alienPlanets[i].transform.position, transform.rotation));
                var count = aliensList[i].Count;
                float alienPlanetScale = alienPlanets[i].transform.localScale.x;
                aliensList[i][count - 1].GetComponent<Movement>().radius = alienPlanetScale / 2f;
                if (count > 8 && count < 38)
                {
                    if (Mathf.Sqrt(count - 1) % 1 == 0)
                    {
                        alienPlanetScale = 0.5f * Mathf.Pow(find_next_square(count), 0.5f);
                        alienPlanets[i].transform.localScale = new Vector3(alienPlanetScale, alienPlanetScale, 0f);
                        for (int j = 0; j < count; j++)
                            aliensList[i][j].GetComponent<Movement>().radius = alienPlanetScale / 2f;
                    }
                }
                RecalculateFormationAliens(i);
            }
        }
    }

    public void AliensInFormation(int index)
    {
        Vector2[] aliensTargetPos = circleFormation.CalculateCircleFormation(aliensList[index], alienPlanets[index].transform.position, alienPlanets[index].transform.localScale.x / 2f);
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            aliensList[index][i].GetComponent<GoToAttackAlien>().enabled = false;
            AliensStopHarvesting(index);
            aliensList[index][i].GetComponent<Movement>().enabled = false;
            aliensList[index][i].GetComponent<AlienJob>().SetIsGoingIntoFormation();
            aliensList[index][i].GetComponent<GoInFormationAlien>().enabled = true;
            aliensList[index][i].GetComponent<GoInFormationAlien>().coords = aliensTargetPos[i];
        }
    }

    public void AliensStopHarvesting(int index)
    {
        if (selectedPlanetsToHarvestAlienList[index].Count > 0)
        {
            for (int i = 0; i < dottedLinesHarvestingAliensList[index].Count; i++)
            {
                Destroy(dottedLinesHarvestingAliensList[index][i]);
            }
            dottedLinesHarvestingAliensList[index] = new List<GameObject>();
            selectedPlanetsToHarvestAlienList[index] = new List<GameObject>();
        }
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            if (aliensList[index][i].GetComponent<AlienJob>().harvesting)
            {
                aliensList[index][i].GetComponent<HarvestResourcesAlien>().stopHarvesting = true;
                aliensList[index][i].GetComponent<Movement>().enabled = true;
            }
        }
    }

    public void AliensHarvesting(int index)
    {
        if (dottedLinesHarvestingAliensList[index].Count > 0)
        {
            AliensStopHarvesting(index);
        }
        else
        {
            GameObject nearestPlanet = FindNearestPlanetToHarvestForAliens(alienPlanets[index]);
            selectedPlanetsToHarvestAlienList[index].Add(nearestPlanet);
            dottedLinesHarvestingAliensList[index].Add(Instantiate(dottedLine, transform));
            dottedLinesHarvestingAliensList[index][dottedLinesHarvestingAliensList[index].Count - 1].GetComponent<LineRenderer>().SetPosition(0, alienPlanets[index].transform.position);
            dottedLinesHarvestingAliensList[index][dottedLinesHarvestingAliensList[index].Count - 1].GetComponent<LineRenderer>().SetPosition(1, nearestPlanet.transform.position);
            dottedLinesHarvestingAliensList[index][dottedLinesHarvestingAliensList[index].Count - 1].GetComponent<LineRenderer>().startColor = Color.green;
            dottedLinesHarvestingAliensList[index][dottedLinesHarvestingAliensList[index].Count - 1].GetComponent<LineRenderer>().endColor = Color.green;
            for (int i = 0; i < aliensList[index].Count; i++)
            {
                aliensList[index][i].GetComponent<Movement>().enabled = false;
                aliensList[index][i].GetComponent<GoToAttackAlien>().enabled = false;
                aliensList[index][i].GetComponent<GoInFormationAlien>().enabled = false;
                aliensList[index][i].GetComponent<CombatModeAlien>().StopCombatModeAlien();
                aliensList[index][i].GetComponent<HarvestResourcesAlien>().enabled = true;
                aliensList[index][i].GetComponent<HarvestResourcesAlien>().SetPlanet(nearestPlanet.transform.position,
                    new Vector3(alienPlanets[index].transform.position.x + Random.insideUnitCircle.x * (alienPlanets[index].transform.localScale.x / 2f),
                    alienPlanets[index].transform.position.y + Random.insideUnitCircle.y * (alienPlanets[index].transform.localScale.x / 2f), 0));
                aliensList[index][i].GetComponent<AlienJob>().SetIsHarvesting();
            }
        }
    }

    public void StartAlienAttack(int index)
    {
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            aliensList[index][i].GetComponent<GoInFormationAlien>().enabled = false;
            AliensStopHarvesting(index);
            aliensList[index][i].GetComponent<Movement>().enabled = false;
            aliensList[index][i].GetComponent<CombatModeAlien>().StartCombatModeAlien();
            aliensList[index][i].GetComponent<GoToAttackAlien>().enabled = true;
            aliensList[index][i].GetComponent<GoToAttackAlien>().coords = 
                new Vector2(Random.insideUnitCircle.x * (humanPlanetScale / 2f), Random.insideUnitCircle.y * (humanPlanetScale / 2f));
            aliensList[index][i].GetComponent<AlienJob>().SetIsAttacking();
        }
    }

    public void AliensFree(int index)
    {
        AliensStopHarvesting(index);
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            aliensList[index][i].GetComponent<GoInFormationAlien>().enabled = false;
            aliensList[index][i].GetComponent<GoToAttackAlien>().enabled = false;
            aliensList[index][i].GetComponent<CombatModeAlien>().StopCombatModeAlien();
            
            aliensList[index][i].GetComponent<Movement>().enabled = true;
            aliensList[index][i].GetComponent<AlienJob>().SetIsFree();
        }
    }

    private int find_next_square(int sq)
    {
        if (sq > 9)
        {
            int x = (int)Mathf.Pow(sq, 0.5f);
            return (int)Mathf.Pow(x + 1, 2);
        }
        else
        {
            return 9;
        }
        
    }

    public int getNumHumans()
    {
        return humans.Count;
    }

    public int getFreeHumans()
    {
        int numFree = 0;
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().free)
                numFree++;
        }
        return numFree;
    }

    public int getHarvestingHumans()
    {
        int numHarvestingHumans = 0;
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().harvesting)
                numHarvestingHumans++;
        }
        return numHarvestingHumans;
    }

    public int getInFormationOrGoingHumans()
    {
        int numInFormationOrGoing = 0;
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().inFormation || humans[i].GetComponent<HumanJob>().goingIntoFormation)
                numInFormationOrGoing++;
        }
        return numInFormationOrGoing;
    }

    public int getInFormationOrGoingOrAttackingHumans()
    {
        int numInFormationOrGoingOrAttacking = 0;
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().inFormation || humans[i].GetComponent<HumanJob>().goingIntoFormation
                || humans[i].GetComponent<HumanJob>().attacking)
                numInFormationOrGoingOrAttacking++;
        }
        return numInFormationOrGoingOrAttacking;
    }

    public void UpdateSlider()
    {
        slider.value = getFreeHumans();
    }

    public void UpdateHarvestingSlider()
    {
        harvestingSlider.value = getHarvestingHumans();
    }

    public void UpdateInFormationSlider()
    {
        inFormationSlider.value = getInFormationOrGoingOrAttackingHumans();
    }

    public Vector3 HarvestingPlanetsDistributor()
    {
        indexPlanetToHarvest += 1;
        if (indexPlanetToHarvest < selectedPlanetsToHarvest.Count)
        {
            return selectedPlanetsToHarvest[indexPlanetToHarvest].transform.position;
        }
        else
        {
            indexPlanetToHarvest = 0;
            return selectedPlanetsToHarvest[0].transform.position;
        }
    }

    public void AddHarvesters(int num)
    {
        int numHarvesting = getHarvestingHumans();
        for (int i = 0; i < num - numHarvesting; i++)
        {
            bool found = false;
            int k = 0;
            while (!found)
            {
                if (selectedPlanetsToHarvest.Count > 0)
                {
                    if (humans[k].GetComponent<HumanJob>().free)
                    {
                        found = true;
                        humans[k].GetComponent<Movement>().enabled = false;
                        humans[k].GetComponent<HarvestResources>().enabled = true;
                        humans[k].GetComponent<HarvestResources>().SetPlanet(HarvestingPlanetsDistributor(),
                            new Vector3(humanPlanet.transform.position.x + Random.insideUnitCircle.x * (humanPlanetScale / 2f),
                            humanPlanet.transform.position.y + Random.insideUnitCircle.y * (humanPlanetScale / 2f), 0));
                        humans[k].GetComponent<HumanJob>().SetIsHarvesting();
                        UpdateSlider();
                    }
                    else
                    {
                        k++;
                    }
                }
                else
                {
                    if (humans[k].GetComponent<HumanJob>().free)
                    {
                        found = true;
                        humans[k].GetComponent<HumanJob>().SetIsHarvesting();
                        UpdateSlider();
                    }
                    else
                    {
                        k++;
                    }
                }
            }
        } 
    }

    public void RemoveHarvesters(int num)
    {
        int numHarvesting = getHarvestingHumans();
        int len = numHarvesting - num;
        for (int i = 0; i < len; i++)
        {
            bool found = false;
            int k = humans.Count - 1;
            while (!found)
            {
                if (selectedPlanetsToHarvest.Count > 0)
                {
                    if (humans[k].GetComponent<HumanJob>().harvesting)
                    {
                        found = true;
                        humans[k].GetComponent<HarvestResources>().stopHarvesting = true;
                    }
                    else
                    {
                        k--;
                    }
                }
                else
                {
                    if (humans[k].GetComponent<HumanJob>().harvesting)
                    {
                        found = true;
                        humans[k].GetComponent<HumanJob>().SetIsFree();
                        humans[k].GetComponent<HarvestResources>().stopHarvesting = true;
                        UpdateSlider();
                    }
                    else
                    {
                        k--;
                    }
                }
                
            }
        }
    }

    public void AddInFormation(int num)
    {
        if (defenseOrAttackButton)
        {
            List<GameObject> entsToPass = new List<GameObject>();
            for (int i = 0; i < humans.Count; i++)
            {
                if (humans[i].GetComponent<HumanJob>().inFormation || humans[i].GetComponent<HumanJob>().goingIntoFormation)
                {
                    entsToPass.Add(humans[i]);
                }
            }
            int numFG = getInFormationOrGoingHumans();
            for (int i = 0; i < num - numFG; i++)
            {
                bool found = false;
                int k = 0;
                while (!found)
                {
                    if (humans[k].GetComponent<HumanJob>().free)
                    {
                        humans[k].GetComponent<HumanJob>().free = false;
                        found = true;
                        entsToPass.Add(humans[k]);
                        humans[k].GetComponent<Movement>().enabled = false;
                        humans[k].GetComponent<HarvestResources>().ResetHarvester();
                        UpdateSlider();
                    }
                    else
                    {
                        k++;
                    }
                }
            }
            Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, humanPlanet.transform.position, humanPlanetScale / 2f);
            for (int i = 0; i < entsToPass.Count; i++)
            {
                entsToPass[i].GetComponent<HumanJob>().SetIsGoingIntoFormation();
                entsToPass[i].GetComponent<CombatModeHuman>().StartCombatModeHuman();
                entsToPass[i].GetComponent<GoInFormationHuman>().enabled = true;
                entsToPass[i].GetComponent<GoInFormationHuman>().coords = humansTargetPos[i];
            }
        }
        else
        {
            int numFGA = getInFormationOrGoingOrAttackingHumans();
            for (int i = 0; i < num - numFGA; i++)
            {
                bool found = false;
                int k = 0;
                while (!found)
                {
                    if (isAlienPlanetSelected)
                    {
                        if (humans[k].GetComponent<HumanJob>().free)
                        {
                            found = true;
                            humans[k].GetComponent<Movement>().enabled = false;
                            humans[k].GetComponent<GoToAttackHuman>().enabled = true;
                            humans[k].GetComponent<GoToAttackHuman>().SetTarget(alienPlanetSelected);
                                /*new Vector2(alienPlanetSelected.transform.position.x + Random.insideUnitCircle.x * (alienPlanetSelected.transform.localScale.x / 2f),
                                    alienPlanetSelected.transform.position.y + Random.insideUnitCircle.y * (alienPlanetSelected.transform.localScale.x / 2f));*/
                            humans[k].GetComponent<HumanJob>().SetIsAttacking();
                            humans[k].GetComponent<CombatModeHuman>().StartCombatModeHuman();
                            UpdateSlider();
                        }
                        else
                        {
                            k++;
                        }
                    }
                    else
                    {
                        if (humans[k].GetComponent<HumanJob>().free)
                        {
                            found = true;
                            humans[k].GetComponent<HumanJob>().SetIsAttacking();
                            humans[k].GetComponent<CombatModeHuman>().StartCombatModeHuman();
                            UpdateSlider();
                        }
                        else
                        {
                            k++;
                        }
                    }
                }
            }
        }
    }

    public void RemoveInFormation(int num)
    {
        int numInFormationOrGoing = getInFormationOrGoingOrAttackingHumans();
        int len = numInFormationOrGoing - num;
        for (int i = 0; i < len; i++)
        {
            bool found = false;
            int k = humans.Count - 1;
            while (!found)
            {
                if (humans[k].GetComponent<HumanJob>().inFormation || humans[k].GetComponent<HumanJob>().goingIntoFormation
                    || humans[k].GetComponent<HumanJob>().attacking)
                {
                    found = true;
                    humans[k].GetComponent<GoInFormationHuman>().enabled = false;
                    humans[k].GetComponent<GoToAttackHuman>().enabled = false;
                    humans[k].GetComponent<CombatModeHuman>().StopCombatModeHuman();
                    humans[k].GetComponent<Movement>().enabled = true;
                    humans[k].GetComponent<HumanJob>().SetIsFree();
                    RecalculateFormation();
                    UpdateSlider();
                }
                else
                {
                    k--;
                }
            }
        }
    }

    public void RecalculateFormation()
    {
        List<GameObject> entsToPass = new List<GameObject>();
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().inFormation || humans[i].GetComponent<HumanJob>().goingIntoFormation)
            {
                entsToPass.Add(humans[i]);
            }
        }
        Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, humanPlanet.transform.position, humanPlanetScale / 2f);
        for (int i = 0; i < entsToPass.Count; i++)
        {
            entsToPass[i].GetComponent<HumanJob>().SetIsGoingIntoFormation();
            entsToPass[i].GetComponent<GoInFormationHuman>().enabled = true;
            entsToPass[i].GetComponent<GoInFormationHuman>().coords = humansTargetPos[i];
        }
    }

    public void RecalculateFormationAliens(int index)
    {
        List<GameObject> entsToPass = new List<GameObject>();
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            if (aliensList[index][i].GetComponent<AlienJob>().inFormation || aliensList[index][i].GetComponent<AlienJob>().goingIntoFormation)
            {
                entsToPass.Add(aliensList[index][i]);
            }
        }
        Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, alienPlanets[index].transform.position, alienPlanets[index].transform.localScale.x / 2f);
        for (int i = 0; i < entsToPass.Count; i++)
        {
            entsToPass[i].GetComponent<AlienJob>().SetIsGoingIntoFormation();
            entsToPass[i].GetComponent<GoInFormationAlien>().enabled = true;
            entsToPass[i].GetComponent<GoInFormationAlien>().coords = humansTargetPos[i];
        }
    }

    public void HarvestingAborted(GameObject planetToStopHarvesting)
    {
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().harvesting && humans[i].GetComponent<HarvestResources>().coorPlanet == planetToStopHarvesting.transform.position)
            {
                humans[i].GetComponent<HarvestResources>().PlanetOutOfResources();
            }
        }
    }

    public void EndHarvesting(GameObject planetToStopHarvesting)
    {
        if (selectedPlanetsToHarvest.IndexOf(planetToStopHarvesting) != -1)
        {
            int pos = selectedPlanetsToHarvest.IndexOf(planetToStopHarvesting);
            isPlanetSelected.RemoveAt(pos);
            selectedPlanetsToHarvest.RemoveAt(pos);
            Destroy(dottedLinesHarvestingHumans[pos]);
            dottedLinesHarvestingHumans.RemoveAt(pos);
            for (int i = 0; i < humans.Count; i++)
            {
                if (humans[i].GetComponent<HumanJob>().harvesting && humans[i].GetComponent<HarvestResources>().coorPlanet == planetToStopHarvesting.transform.position)
                {
                    humans[i].GetComponent<HarvestResources>().PlanetOutOfResources();
                }
            }
        }

        for (int j = 0; j < selectedPlanetsToHarvestAlienList.Count; j++)
        {
            if (selectedPlanetsToHarvestAlienList[j].IndexOf(planetToStopHarvesting) != -1)
            {
                int pos = selectedPlanetsToHarvestAlienList[j].IndexOf(planetToStopHarvesting);
                selectedPlanetsToHarvestAlienList[j].RemoveAt(pos);
                Destroy(dottedLinesHarvestingAliensList[j][pos]);
                dottedLinesHarvestingAliensList[j].RemoveAt(pos);
                for (int i = 0; i < aliensList[j].Count; i++)
                {
                    if (aliensList[j][i].GetComponent<AlienJob>().harvesting && aliensList[j][i].GetComponent<HarvestResourcesAlien>().coorPlanet == planetToStopHarvesting.transform.position)
                    {
                        aliensList[j][i].GetComponent<HarvestResourcesAlien>().PlanetOutOfResources();
                    }
                }
            }
        }
    }

    public void HarvestersStartHarvesting()
    {
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().harvesting)
            {
                humans[i].GetComponent<Movement>().enabled = false;
                humans[i].GetComponent<HarvestResources>().enabled = true;
                humans[i].GetComponent<HarvestResources>().SetPlanet(HarvestingPlanetsDistributor(),
                    new Vector3(humanPlanet.transform.position.x + Random.insideUnitCircle.x * (humanPlanetScale / 2f),
                    humanPlanet.transform.position.y + Random.insideUnitCircle.y * (humanPlanetScale / 2f), 0));
            }
        }
    }

    public void AttackersStartAttacking()    
    {
        for (int i = 0; i<humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().attacking)
            {
                humans[i].GetComponent<Movement>().enabled = false;
                humans[i].GetComponent<GoToAttackHuman>().enabled = true;
                humans[i].GetComponent<GoToAttackHuman>().SetTarget(alienPlanetSelected);
                    /*new Vector2(alienPlanetSelected.transform.position.x + Random.insideUnitCircle.x* (alienPlanetSelected.transform.localScale.x / 2f),
                        alienPlanetSelected.transform.position.y + Random.insideUnitCircle.y* (alienPlanetSelected.transform.localScale.x / 2f));*/
            }
        }
    }

    /* public Vector3 FindNearestPlanetToHarvest()
    {
        float minDistance = float.MaxValue;
        float dis;
        int count = 0;
        for (int i = 0; i < planetsToHarvest.Count; i++)
        {
            if (planetsToHarvest[i].activeSelf)
            {
                dis = Vector3.Distance(humanPlanet.transform.position, planetsToHarvest[i].transform.position);
                if (dis < minDistance)
                {
                    minDistance = dis;
                    count = i;
                }
            }
        }
        return planetsToHarvest[count].transform.position;
    }*/

    public GameObject FindNearestPlanetToHarvestForAliens(GameObject alienPlanet)
    {
        float minDistance = float.MaxValue;
        float dis;
        int count = 0;
        for (int i = 0; i < planetsToHarvest.Count; i++)
        {
            if (planetsToHarvest[i].GetComponent<CircleCollider2D>().enabled)
            {
                dis = Vector3.Distance(alienPlanet.transform.position, planetsToHarvest[i].transform.position);
                if (dis < minDistance)
                {
                    minDistance = dis;
                    count = i;
                }
            }
        }
        return planetsToHarvest[count];
    }

    public void AddMineralResource()
    {
        mineralResourcesSlider.value++;
    }

    public void AddWaterResource()
    {
        waterResourcesSlider.value++;
    }

    public void AddGasResource()
    {
        gasResourcesSlider.value++;
    }

    public void HumansGoDefend()
    {
        defOrAttButton.GetComponent<Image>().sprite = shieldImage;
        List<GameObject> entsToPass = new List<GameObject>();
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().attacking ||
                humans[i].GetComponent<HumanJob>().inFormation || humans[i].GetComponent<HumanJob>().goingIntoFormation)
            {
                entsToPass.Add(humans[i]);
            }
        }
        Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, humanPlanet.transform.position, humanPlanetScale / 2f);
        for (int i = 0; i < entsToPass.Count; i++)
        {
            entsToPass[i].GetComponent<GoToAttackHuman>().enabled = false;
            entsToPass[i].GetComponent<HumanJob>().SetIsGoingIntoFormation();
            entsToPass[i].GetComponent<Movement>().enabled = false;
            entsToPass[i].GetComponent<GoInFormationHuman>().enabled = true;
            entsToPass[i].GetComponent<GoInFormationHuman>().coords = humansTargetPos[i];
        }
    }

    public void DefenseOrAttackButton()
    {
        // true = defense, false = attack
        if (isAlienPlanetSelected)
        {
            defenseOrAttackButton = !defenseOrAttackButton;
            if (defenseOrAttackButton)
            {
                Destroy(dottedLinesAttackingHumans[dottedLinesAttackingHumans.Count - 1]);
                dottedLinesAttackingHumans.RemoveAt(dottedLinesAttackingHumans.Count - 1);
                isAlienPlanetSelected = false;
                HumansGoDefend();
            }
            else
            {
                defOrAttButton.GetComponent<Image>().sprite = swordImage;
                for (int i = 0; i < humans.Count; i++)
                {
                    if (humans[i].GetComponent<HumanJob>().inFormation || humans[i].GetComponent<HumanJob>().goingIntoFormation)
                    {
                        humans[i].GetComponent<GoInFormationHuman>().enabled = false;
                        humans[i].GetComponent<GoToAttackHuman>().enabled = true;
                        humans[i].GetComponent<GoToAttackHuman>().SetTarget(alienPlanetSelected);
                            /*new Vector2(alienPlanetSelected.transform.position.x + Random.insideUnitCircle.x * (alienPlanetSelected.transform.localScale.x / 2f),
                                alienPlanetSelected.transform.position.y + Random.insideUnitCircle.y * (alienPlanetSelected.transform.localScale.x / 2f));*/
                        humans[i].GetComponent<HumanJob>().SetIsAttacking();
                    }
                }
            }
        }     
    }
}
