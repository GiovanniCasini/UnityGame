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

    public GameObject alienPlanet;
    public float alienPlanetScale;

    public GameObject human;
    public List<GameObject> humans;
    public int numStartingHumans = 2;
    public GameObject alien;
    public List<GameObject> aliens;
    // public List<List<GameObject> aliensList;
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
    // public GameObject selectedPlanetToHarvest;
    public List<GameObject> selectedPlanetsToHarvest;
    private List<bool> isPlanetSelected;
    public int indexPlanetToHarvest = -1;

    public Button defOrAttButton;
    public Sprite shieldImage;
    public Sprite swordImage;
    private bool defenseOrAttackButton = true;
    public GameObject alienPlanetSelected;
    private bool isAlienPlanetSelected = false;

    public GameObject dottedLine;
    public List<GameObject> dottedLinesHarvestingHumans;
    public List<GameObject> dottedLinesAttackingHumans;
    public List<GameObject> dottedLinesHarvestingAliens;
    public List<GameObject> selectedPlanetsToHarvestAlien;

    public GameObject star;

    public GameObject bg_stars;
    public GameObject bg_starsFolder;

    //private bool buildMode = true;


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

        alienPlanet = Instantiate(alienPlanet, new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), 0f), transform.rotation);
        alienPlanetScale = alienPlanet.transform.localScale.x;

        aliens = new List<GameObject>();
        numStartingAliens = (int)Random.Range(5f, 20f);
        for (int i = 0; i < numStartingAliens; i++)
        {
            // aliens.Add(Instantiate(alien, alienPlanet.transform.position, transform.rotation));
            AddAlien();
            aliens[i].GetComponent<Movement>().radius = alienPlanetScale / 2f;
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

        selectedPlanetsToHarvestAlien = new List<GameObject>();
        dottedLinesHarvestingAliens = new List<GameObject>();

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
            AliensInFormation();
        }
        if (Input.GetKeyDown("g"))
        {
            AliensFree();
        }
        if (Input.GetKeyDown("t"))
        {
            StartAlienAttack();
        }
        if (Input.GetKeyDown("h"))
        {
            AliensHarvesting();
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
                    else
                    {
                        Destroy(dottedLinesAttackingHumans[dottedLinesAttackingHumans.Count - 1]);
                        dottedLinesAttackingHumans.RemoveAt(dottedLinesAttackingHumans.Count - 1);
                        isAlienPlanetSelected = false;
                        defenseOrAttackButton = !defenseOrAttackButton;
                        HumansGoDefend();
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
        aliens.Remove(alien);
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
        foreach (GameObject alien in aliens)
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
        return closest;
    }

    public void AddHuman()
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

    public void UpdatePlanet(int count)
    {
        if (count > 8)
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

    public void AddAlien()
    {
        aliens.Add(Instantiate(alien, alienPlanet.transform.position, transform.rotation));
        var count = aliens.Count;
        aliens[count - 1].GetComponent<Movement>().radius = alienPlanetScale / 2f;
        if (count > 8)
        {
            if (Mathf.Sqrt(count - 1) % 1 == 0)
            {
                alienPlanetScale = 0.5f * Mathf.Pow(find_next_square(count), 0.5f);
                alienPlanet.transform.localScale = new Vector3(alienPlanetScale, alienPlanetScale, 0f);
                for (int i = 0; i < count; i++)
                    aliens[i].GetComponent<Movement>().radius = alienPlanetScale / 2f;
            }
        }
        RecalculateFormationAliens();
    }

    public void AliensInFormation()
    {
        Vector2[] aliensTargetPos = circleFormation.CalculateCircleFormation(aliens, alienPlanet.transform.position, alienPlanetScale / 2f);
        for (int i = 0; i < aliens.Count; i++)
        {
            aliens[i].GetComponent<GoToAttackAlien>().enabled = false;
            AliensStopHarvesting();
            aliens[i].GetComponent<Movement>().enabled = false;
            aliens[i].GetComponent<AlienJob>().SetIsGoingIntoFormation();
            aliens[i].GetComponent<GoInFormationAlien>().enabled = true;
            aliens[i].GetComponent<GoInFormationAlien>().coords = aliensTargetPos[i];
        }
    }

    public void AliensStopHarvesting()
    {
        if (selectedPlanetsToHarvestAlien.Count > 0)
        {
            Destroy(dottedLinesHarvestingAliens[dottedLinesHarvestingAliens.Count - 1]);
            dottedLinesHarvestingAliens.RemoveAt(dottedLinesHarvestingAliens.Count - 1);
            selectedPlanetsToHarvestAlien.RemoveAt(selectedPlanetsToHarvestAlien.Count - 1);
            for (int i = 0; i < aliens.Count; i++)
            {
                if (aliens[i].GetComponent<AlienJob>().harvesting)
                {
                    aliens[i].GetComponent<HarvestResourcesAlien>().stopHarvesting = true;
                    aliens[i].GetComponent<Movement>().enabled = true;
                }
            }
        }
    }

    public void AliensHarvesting()
    {
        if (dottedLinesHarvestingAliens.Count > 0)
        {
            AliensStopHarvesting();
        }
        else
        {
            GameObject nearestPlanet = FindNearestPlanetToHarvestForAliens();
            selectedPlanetsToHarvestAlien.Add(nearestPlanet);
            dottedLinesHarvestingAliens.Add(Instantiate(dottedLine, transform));
            dottedLinesHarvestingAliens[dottedLinesHarvestingAliens.Count - 1].GetComponent<LineRenderer>().SetPosition(0, alienPlanet.transform.position);
            dottedLinesHarvestingAliens[dottedLinesHarvestingAliens.Count - 1].GetComponent<LineRenderer>().SetPosition(1, nearestPlanet.transform.position);
            dottedLinesHarvestingAliens[dottedLinesHarvestingAliens.Count - 1].GetComponent<LineRenderer>().startColor = Color.green;
            dottedLinesHarvestingAliens[dottedLinesHarvestingAliens.Count - 1].GetComponent<LineRenderer>().endColor = Color.green;
            for (int i = 0; i < aliens.Count; i++)
            {
                aliens[i].GetComponent<Movement>().enabled = false;
                aliens[i].GetComponent<GoToAttackAlien>().enabled = false;
                aliens[i].GetComponent<GoInFormationAlien>().enabled = false;
                aliens[i].GetComponent<CombatModeAlien>().StopCombatModeAlien();
                aliens[i].GetComponent<HarvestResourcesAlien>().enabled = true;
                aliens[i].GetComponent<AlienJob>().SetIsHarvesting();
                aliens[i].GetComponent<HarvestResourcesAlien>().SetSetPlanet(nearestPlanet.transform.position);
            }
        }
    }

    public void StartAlienAttack()
    {
        for (int i = 0; i < aliens.Count; i++)
        {
            aliens[i].GetComponent<GoInFormationAlien>().enabled = false;
            AliensStopHarvesting();
            aliens[i].GetComponent<Movement>().enabled = false;
            aliens[i].GetComponent<CombatModeAlien>().StartCombatModeAlien();
            aliens[i].GetComponent<GoToAttackAlien>().enabled = true;
            aliens[i].GetComponent<GoToAttackAlien>().coords = 
                new Vector2(Random.insideUnitCircle.x * (humanPlanetScale / 2f), Random.insideUnitCircle.y * (humanPlanetScale / 2f));
            aliens[i].GetComponent<AlienJob>().SetIsAttacking();
        }
    }

    public void AliensFree()
    {
        for (int i = 0; i < aliens.Count; i++)
        {
            aliens[i].GetComponent<GoInFormationAlien>().enabled = false;
            aliens[i].GetComponent<GoToAttackAlien>().enabled = false;
            aliens[i].GetComponent<CombatModeAlien>().StopCombatModeAlien();
            AliensStopHarvesting();
            aliens[i].GetComponent<Movement>().enabled = true;
            aliens[i].GetComponent<AlienJob>().SetIsFree();
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
                        humans[k].GetComponent<HumanJob>().SetIsHarvesting();
                        humans[k].GetComponent<HarvestResources>().SetPlanet(HarvestingPlanetsDistributor());
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
                            humans[k].GetComponent<GoToAttackHuman>().coords =
                                new Vector2(alienPlanetSelected.transform.position.x + Random.insideUnitCircle.x * (alienPlanetScale / 2f),
                                    alienPlanetSelected.transform.position.y + Random.insideUnitCircle.y * (alienPlanetScale / 2f));
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

    public void RecalculateFormationAliens()
    {
        List<GameObject> entsToPass = new List<GameObject>();
        for (int i = 0; i < aliens.Count; i++)
        {
            if (aliens[i].GetComponent<AlienJob>().inFormation || aliens[i].GetComponent<AlienJob>().goingIntoFormation)
            {
                entsToPass.Add(aliens[i]);
            }
        }
        Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, alienPlanet.transform.position, alienPlanetScale / 2f);
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

        if (selectedPlanetsToHarvestAlien.IndexOf(planetToStopHarvesting) != -1)
        {
            int pos = selectedPlanetsToHarvestAlien.IndexOf(planetToStopHarvesting);
            selectedPlanetsToHarvestAlien.RemoveAt(pos);
            Destroy(dottedLinesHarvestingAliens[pos]);
            dottedLinesHarvestingAliens.RemoveAt(pos);
            for (int i = 0; i < aliens.Count; i++)
            {
                if (aliens[i].GetComponent<AlienJob>().harvesting && aliens[i].GetComponent<HarvestResourcesAlien>().coorPlanet == planetToStopHarvesting.transform.position)
                {
                    aliens[i].GetComponent<HarvestResourcesAlien>().PlanetOutOfResources();
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
                humans[i].GetComponent<HarvestResources>().SetPlanet(HarvestingPlanetsDistributor());
            }
        }
    }

    public void AttackersStartAttacking()
    {
        for (int i = 0; i < humans.Count; i++)
        {
            if (humans[i].GetComponent<HumanJob>().attacking)
            {
                humans[i].GetComponent<Movement>().enabled = false;
                humans[i].GetComponent<GoToAttackHuman>().enabled = true;
                humans[i].GetComponent<GoToAttackHuman>().coords =
                    new Vector2(alienPlanetSelected.transform.position.x + Random.insideUnitCircle.x * (alienPlanetScale / 2f),
                        alienPlanetSelected.transform.position.y + Random.insideUnitCircle.y * (alienPlanetScale / 2f));
            }
        }
    }

    public Vector3 FindNearestPlanetToHarvest()
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
    }

    public GameObject FindNearestPlanetToHarvestForAliens()
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
                        humans[i].GetComponent<GoToAttackHuman>().coords =
                            new Vector2(alienPlanetSelected.transform.position.x + Random.insideUnitCircle.x * (alienPlanetScale / 2f),
                                alienPlanetSelected.transform.position.y + Random.insideUnitCircle.y * (alienPlanetScale / 2f));
                        humans[i].GetComponent<HumanJob>().SetIsAttacking();
                    }
                }
            }
        }     
    }
}
