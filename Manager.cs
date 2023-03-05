using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    // HUMANS

    public GameObject human;
    public List<GameObject> humans;
    public List<List<GameObject>> humansList = new List<List<GameObject>>(); // list of human civilizations (one planet = one civilization)
    public int numStartingHumans = 2;
    public GameObject humanPlanet;
    public List<GameObject> humanPlanets = new List<GameObject>();
    public GameObject selectedHumanPlanet;
    public List<bool> conqueringHumanPlanets = new List<bool>();

    // HUMANS HARVEST

    public List<List<GameObject>> dottedLinesHarvestingHumanList = new List<List<GameObject>>();
    public List<List<GameObject>> selectedPlanetsToHarvestHumanList = new List<List<GameObject>>(); // one list for each planet, each list contains a list of selected harvesting planets
    public List<GameObject> humanHarvesters = new List<GameObject>();
    public List<int> indexPlanetToHarvest = new List<int>();

    // HUMANS ATTACK

    public Sprite shieldImage;
    public Sprite swordImage;
    public Button defOrAttButton;
    public List<GameObject> dottedLinesAttackingHumans = new List<GameObject>();
    public List<GameObject> selectedPlanetToAttack = new List<GameObject>();
    public List<GameObject> humanDefendersOrAttackers = new List<GameObject>();

    // ALIENS

    public GameObject alien;
    public List<GameObject> aliens;
    public List<List<GameObject>> aliensList = new List<List<GameObject>>(); // list of alien civilizations (one planet = one civilization)
    public int numStartingAliens = 5;
    public GameObject alienPlanet;
    public List<GameObject> alienPlanets = new List<GameObject>(); // list of alien civilizations' planet, one for each alien civilization for now
    public List<bool> conqueringAlienPlanets = new List<bool>();
    public List<bool> aliensAttacking = new List<bool>();
    public List<bool> aliensInFormation = new List<bool>();
    public int indexLastHumanPlanetAttacked;
    public int indexLastResoursePlanetAttackedAliens;

    public List<List<GameObject>> dottedLinesHarvestingAliensList = new List<List<GameObject>>();
    public List<List<GameObject>> selectedPlanetsToHarvestAlienList = new List<List<GameObject>>();

    // PLANETS

    public GameObject mineralPlanet;
    public GameObject icePlanet;
    public GameObject gasPlanet;
    public List<GameObject> planetsToHarvest = new List<GameObject>(); // planets ready to be harvested
    public List<bool> emptyResourcePlanets = new List<bool>();
    public List<bool> conqueringResourcePlanets = new List<bool>();

    // SLIDERS

    public Slider slider;
    public Slider harvestingSlider;
    public Slider inFormationSlider;

    public Slider mineralResourcesSlider;
    public Slider iceResourcesSlider;
    public Slider gasResourcesSlider;

    public bool updatingLocalSlider = false;
    public bool updatingGlobalSlider = false;

    public Button closeSlidersButton;
    public Button openSlidersButton;
    public bool openCloseSliders = true; // true=open, false=close

    // STUFF

    public CircleFormation circleFormation = new CircleFormation();
    public GameObject dottedLine;
    public GameObject star;
    public GameObject bg_stars;
    public GameObject bg_starsFolder;
    public GameObject selector;
    public List<GameObject> selectors = new List<GameObject>();

    public GameObject planetPanel;
    public GameObject statsPanel;
    public TMP_Text planetaryNameTM;
    private string planetaryName;
    public TMP_Text populationTM;
    public TMP_Text spawnRateTM;
    public TMP_Text rateOfFireTM;
    public TMP_Text speedTM;

    public bool pass = false; // used in slider scripts

    void Start()
    {
        // HUMANS

        humans = new List<GameObject>();
        humanPlanets.Add(Instantiate(humanPlanet));
        selectedPlanetsToHarvestHumanList.Add(new List<GameObject>());

        dottedLinesHarvestingHumanList.Add(new List<GameObject>());
        indexPlanetToHarvest.Add(-1);
        selectedPlanetToAttack.Add(null);
        dottedLinesAttackingHumans.Add(null);
        conqueringHumanPlanets.Add(false);

        for (int i = 0; i < numStartingHumans; i++)
            humans.Add(Instantiate(human, humanPlanets[0].transform.position, transform.rotation));
        humansList.Add(humans);
        float humanPlanetScale = 0.5f * Mathf.Pow(FindNextSquare(humansList[0].Count), 0.5f);
        humanPlanets[0].transform.localScale = new Vector3(humanPlanetScale, humanPlanetScale, 0f);
        for (int i = 0; i < humansList[0].Count; i++)
            humansList[0][i].GetComponent<Movement>().radius = humanPlanetScale / 2f;

        for (int k = 1; k < 2; k++)
        {
            humans = new List<GameObject>();
            humanPlanets.Add(Instantiate(humanPlanet, new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), 0f), transform.rotation));
            selectedPlanetsToHarvestHumanList.Add(new List<GameObject>());
            dottedLinesHarvestingHumanList.Add(new List<GameObject>());
            indexPlanetToHarvest.Add(-1);
            selectedPlanetToAttack.Add(null);
            dottedLinesAttackingHumans.Add(null);
            conqueringHumanPlanets.Add(false);
            numStartingHumans = Random.Range(20, 30);
            for (int i = 0; i < numStartingHumans; i++)
                humans.Add(Instantiate(human, humanPlanets[k].transform.position, transform.rotation));
            humansList.Add(humans);
            float humanPlanetScale2 = 0.5f * Mathf.Pow(FindNextSquare(humansList[k].Count), 0.5f);
            humanPlanets[k].transform.localScale = new Vector3(humanPlanetScale2, humanPlanetScale2, 0f);
            for (int i = 0; i < humansList[k].Count; i++)
                humansList[k][i].GetComponent<Movement>().radius = humanPlanetScale2 / 2f;
        }

        // ALIENS

        for (int j = 0; j < 3; j++) // one cycle for each alien civilization
        {
            aliens = new List<GameObject>();
            aliensList.Add(aliens);
            alienPlanets.Add(Instantiate(alienPlanet, new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), 0f), transform.rotation));
            conqueringAlienPlanets.Add(false);
            aliensAttacking.Add(false);
            aliensInFormation.Add(false);
            numStartingAliens = Random.Range(5, 15);
            for (int i = 0; i < numStartingAliens; i++)
            {
                AddAlien(alienPlanets[j]);
            }
            selectedPlanetsToHarvestAlienList.Add(new List<GameObject>());
            dottedLinesHarvestingAliensList.Add(new List<GameObject>());
        }

        // PLANETS AND ADDING THEM TO POSSIBLE PLANETS TO HARVEST FOR HUMANS

        for (int i = 0; i < 4; i++)
        {
            planetsToHarvest.Add(Instantiate(icePlanet, new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f), transform.rotation));
            emptyResourcePlanets.Add(false);
            conqueringResourcePlanets.Add(false);
        }
        for (int i = 0; i < 10; i++)
        {
            planetsToHarvest.Add(Instantiate(mineralPlanet, new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f), transform.rotation));
            emptyResourcePlanets.Add(false);
            conqueringResourcePlanets.Add(false);
        }
        for (int i = 0; i < 6; i++)
        {
            planetsToHarvest.Add(Instantiate(gasPlanet, new Vector3(Random.Range(-50f, 50f), Random.Range(-50f, 50f), 0f), transform.rotation));
            emptyResourcePlanets.Add(false);
            conqueringResourcePlanets.Add(false);
        }

        // SLIDERS

        int totHumans = GetNumHumans();
        slider.maxValue = totHumans;
        slider.value = totHumans;
        harvestingSlider.maxValue = totHumans;
        inFormationSlider.maxValue = totHumans;

        openSlidersButton.gameObject.SetActive(false);
        closeSlidersButton.gameObject.SetActive(true);

        // STARS AND BACKGROUND STARS

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

        planetaryName = "My Planetary";
        OpenSliders();

        /////////////////////

        Invoke("StartAlienHarvesting", 3f);
        //InvokeRepeating("StartAlienAttack", 60f, 60f);
    }

    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            Invoke("StartAlienConquestResourcePlanet", 1f);
        }
        if (Input.GetKeyDown("n"))
        {
            for (int i = 0; i < humansList.Count; i++)
            {
                AddHuman(i);
            }
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
                AlienAttack(i);
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
                if (hit.collider.gameObject.tag == "HumanPlanet")
                {
                    int index = humanPlanets.IndexOf(selectedHumanPlanet);
                    if (selectedHumanPlanet == null) // if there is no human planet selected
                    {
                        selectedHumanPlanet = hit.collider.gameObject;
                        selectors.Add(Instantiate(selector, hit.collider.gameObject.transform));
                        UpdateSlider(humanPlanets.IndexOf(selectedHumanPlanet));
                        updatingLocalSlider = true;
                        UpdateHarvestingSlider(humanPlanets.IndexOf(selectedHumanPlanet));
                        UpdateInFormationSlider(humanPlanets.IndexOf(selectedHumanPlanet));
                        updatingLocalSlider = false;
                        if (planetPanel.activeSelf)
                        {
                            index = humanPlanets.IndexOf(selectedHumanPlanet);
                            planetaryNameTM.text = "Planet " + index;
                            populationTM.text = GetNumHumans(index).ToString();
                            planetPanel.transform.GetChild(3).gameObject.SetActive(true);
                            spawnRateTM.text = "1";
                            rateOfFireTM.text = "1";
                            speedTM.text = "1";
                        }
                    }
                    else if (hit.collider.gameObject == selectedHumanPlanet) // if clicked on the human planet already selected, deselect it
                    {
                        Destroy(selectors[0]);
                        selectors.RemoveAt(0);
                        selectedHumanPlanet = null;
                        int totHumans = GetNumHumans();
                        slider.maxValue = totHumans;
                        harvestingSlider.maxValue = totHumans;
                        inFormationSlider.maxValue = totHumans;
                        UpdateSlider();
                        updatingGlobalSlider = true;
                        UpdateHarvestingSlider();
                        UpdateInFormationSlider();
                        updatingGlobalSlider = false;
                        if (planetPanel.activeSelf)
                        {
                            planetaryNameTM.text = planetaryName;
                            populationTM.text = GetNumHumans().ToString();
                            planetPanel.transform.GetChild(3).gameObject.SetActive(false);
                        }
                    }
                    else if (selectedHumanPlanet != null && hit.collider.gameObject != selectedHumanPlanet
                        && selectedPlanetToAttack[index] == null) // if there is a human planet selected and click on a different human planet and there is no alien planet selected, go defend that human planet selected
                    {
                        // MoveHumans(humanPlanets.IndexOf(selectedHumanPlanet), humanPlanets.IndexOf(hit.collider.gameObject), 1);
                        selectedPlanetToAttack[index] = hit.collider.gameObject; // in this case it is not an alien planet to attack but an human planet to defend
                        AttackersStartAttacking(index);
                        dottedLinesAttackingHumans[index] = Instantiate(dottedLine, transform);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(0, selectedHumanPlanet.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(1, hit.collider.gameObject.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().startColor = Color.red;
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().endColor = Color.red;
                    }
                    else if (selectedHumanPlanet != null && hit.collider.gameObject != selectedHumanPlanet
                        && selectedPlanetToAttack[index] == hit.collider.gameObject) // if there is a human planet selected and click on a different human planet already selected to defend, deselect and interrupt
                    {
                        Destroy(dottedLinesAttackingHumans[index]);
                        dottedLinesAttackingHumans[index] = null;
                        selectedPlanetToAttack[index] = null;
                        HumansGoDefend(index);
                    }
                    else if (selectedHumanPlanet != null && hit.collider.gameObject != selectedHumanPlanet
                        && selectedPlanetToAttack[index] != hit.collider.gameObject) // if there is a human planet selected and click on a different human planet and there is a different alien/human planet selected,
                                                                                     // interrupt and defend the new human selected planet
                    {
                        if (selectedPlanetToAttack[index].tag == "AlienPlanet")
                        {
                            int count = 0;
                            for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                            {
                                if (selectedPlanetToAttack[i] == selectedPlanetToAttack[index])
                                {
                                    count++;
                                }
                            }
                            if (count == 1) // check if there is only one human planet attacking the alien planet, if so interrupt conquest
                            {
                                if (aliensInFormation[alienPlanets.IndexOf(selectedPlanetToAttack[index])])
                                {
                                    AliensFree(alienPlanets.IndexOf(selectedPlanetToAttack[index]));
                                }
                                for (int i = 0; i < aliensList.Count; i++)
                                {
                                    if (aliensAttacking[i])
                                    {
                                        for (int j = 0; j < aliensList[i].Count; j++)
                                        {
                                            if (aliensList[i][j].GetComponent<GoToAttackAlien>().target == selectedPlanetToAttack[index].transform.position)
                                            {
                                                AlienRetreat();
                                                break;
                                            }
                                        }
                                    }
                                }
                                conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[index])] = false;
                            }
                        }

                        Destroy(dottedLinesAttackingHumans[index]);
                        dottedLinesAttackingHumans[index] = null;

                        selectedPlanetToAttack[index] = hit.collider.gameObject; // in this case it is not an alien planet to attack but an human planet to defend
                        AttackersStartAttacking(index);
                        dottedLinesAttackingHumans[index] = Instantiate(dottedLine, transform);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(0, selectedHumanPlanet.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(1, hit.collider.gameObject.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().startColor = Color.red;
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().endColor = Color.red;
                    }
                }
                else if ((hit.collider.gameObject.tag == "MineralPlanet" || hit.collider.gameObject.tag == "IcePlanet" || hit.collider.gameObject.tag == "GasPlanet")
                    && selectedHumanPlanet != null)
                {
                    int index = humanPlanets.IndexOf(selectedHumanPlanet);
                    if (selectedPlanetsToHarvestHumanList[index].IndexOf(hit.collider.gameObject) == -1) // if clicked on a planet not already selected
                    {
                        if (selectedPlanetsToHarvestHumanList[index].Count < 2) // max number of harvesting planets for humans
                        {
                            selectedPlanetsToHarvestHumanList[index].Add(hit.collider.gameObject);
                            HarvestersStartHarvesting(index);
                            dottedLinesHarvestingHumanList[index].Add(Instantiate(dottedLine, transform));
                            dottedLinesHarvestingHumanList[index][dottedLinesHarvestingHumanList[index].Count - 1].GetComponent<LineRenderer>().
                                SetPosition(0, selectedHumanPlanet.transform.position);
                            dottedLinesHarvestingHumanList[index][dottedLinesHarvestingHumanList[index].Count - 1].GetComponent<LineRenderer>()
                                .SetPosition(1, hit.collider.gameObject.transform.position);
                            dottedLinesHarvestingHumanList[index][dottedLinesHarvestingHumanList[index].Count - 1].GetComponent<LineRenderer>().startColor = Color.yellow;
                            dottedLinesHarvestingHumanList[index][dottedLinesHarvestingHumanList[index].Count - 1].GetComponent<LineRenderer>().endColor = Color.yellow;
                        }
                    }
                    else // if clicked on a planet already selected, deselect it
                    {
                        int pos = selectedPlanetsToHarvestHumanList[index].IndexOf(hit.collider.gameObject);
                        Destroy(dottedLinesHarvestingHumanList[index][pos]);
                        dottedLinesHarvestingHumanList[index].RemoveAt(pos);
                        HarvestingAborted(selectedPlanetsToHarvestHumanList[index][pos]);
                        selectedPlanetsToHarvestHumanList[index].RemoveAt(pos);
                    }
                }
                else if (hit.collider.gameObject.tag == "AlienPlanet" && selectedHumanPlanet != null)
                {
                    int index = humanPlanets.IndexOf(selectedHumanPlanet);
                    if (selectedPlanetToAttack[index] == null) // if there isnt an alien planet selected
                    {
                        selectedPlanetToAttack[index] = hit.collider.gameObject;
                        AttackersStartAttacking(index);
                        if (!conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[index])])
                        {
                            conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[index])] = true;
                            StartCoroutine(ConqueringAlienPlanet(alienPlanets.IndexOf(selectedPlanetToAttack[index])));
                        }
                        Destroy(dottedLinesAttackingHumans[index]);
                        dottedLinesAttackingHumans[index] = Instantiate(dottedLine, transform);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(0, selectedHumanPlanet.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(1, hit.collider.gameObject.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().startColor = Color.red;
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().endColor = Color.red;
                    }
                    else if (selectedPlanetToAttack[index] == hit.collider.gameObject) // if clicked on an alien planet already selected, deselect it and interrupt attack
                    {
                        int count = 0;
                        for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                        {
                            if (selectedPlanetToAttack[i] == selectedPlanetToAttack[index])
                            {
                                count++;
                            }
                        }
                        if (count == 1) // check if there is only one human planet attacking the alien planet, if so interrupt conquest
                        {
                            if (aliensInFormation[alienPlanets.IndexOf(selectedPlanetToAttack[index])])
                            {
                                AliensFree(alienPlanets.IndexOf(selectedPlanetToAttack[index]));
                            }
                            for (int i = 0; i < aliensList.Count; i++)
                            {
                                if (aliensAttacking[i])
                                {
                                    for (int j = 0; j < aliensList[i].Count; j++)
                                    {
                                        if (aliensList[i][j].GetComponent<GoToAttackAlien>().target == hit.collider.gameObject.transform.position)
                                        {
                                            AlienRetreat();
                                            break;
                                        }
                                    }
                                }
                            }
                            conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[index])] = false;
                        }

                        Destroy(dottedLinesAttackingHumans[index]);
                        dottedLinesAttackingHumans[index] = null;
                        selectedPlanetToAttack[index] = null;
                        HumansGoDefend(index);
                    }
                    else // if clicked on an alien planet but there is another planet already selected, interrupt attack and attack the new alien planet
                    {
                        Destroy(dottedLinesAttackingHumans[index]);
                        dottedLinesAttackingHumans[index] = null;
                        if (selectedPlanetToAttack[index].tag == "AlienPlanet")
                        {
                            int count = 0;
                            for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                            {
                                if (selectedPlanetToAttack[i] == selectedPlanetToAttack[index])
                                {
                                    count++;
                                }
                            }
                            if (count == 1) // check if there is only one human planet attacking the alien planet, if so interrupt conquest
                            {
                                if (aliensInFormation[alienPlanets.IndexOf(selectedPlanetToAttack[index])])
                                {
                                    AliensFree(alienPlanets.IndexOf(selectedPlanetToAttack[index]));
                                }
                                for (int i = 0; i < aliensList.Count; i++)
                                {
                                    if (aliensAttacking[i])
                                    {
                                        for (int j = 0; j < aliensList[i].Count; j++)
                                        {
                                            if (aliensList[i][j].GetComponent<GoToAttackAlien>().target == selectedPlanetToAttack[index].transform.position)
                                            {
                                                AlienRetreat();
                                                break;
                                            }
                                        }
                                    }
                                }
                                conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[index])] = false;
                            }
                        }
                        else if (selectedPlanetToAttack[index].tag == "EmptyResourcePlanet")
                        {
                            int countH = 0;
                            int countA = 0;
                            for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                            {
                                if (selectedPlanetToAttack[i] == selectedPlanetToAttack[index])
                                {
                                    countH++;
                                }
                            }
                            for (int m = 0; m < aliensList.Count; m++)
                            {
                                for (int k = 0; k < aliensList[m].Count; k++)
                                {
                                    if (aliensList[m][k].GetComponent<AlienJob>().attacking && aliensList[m][k].GetComponent<GoToAttackAlien>().target == hit.collider.gameObject.transform.position)
                                    {
                                        countA++;
                                        break;
                                    }
                                }
                            }
                            if (countH == 1 && countA == 0) // check if there is only one human planet and no alien planets attacking the resource planet, if so interrupt conquest
                            {
                                conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[index])] = false;
                            }
                        }

                        selectedPlanetToAttack[index] = hit.collider.gameObject;
                        AttackersStartAttacking(index);
                        if (!conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[index])])
                        {
                            conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[index])] = true;
                            StartCoroutine(ConqueringAlienPlanet(alienPlanets.IndexOf(selectedPlanetToAttack[index])));
                        }
                        dottedLinesAttackingHumans[index] = Instantiate(dottedLine, transform);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(0, selectedHumanPlanet.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(1, hit.collider.gameObject.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().startColor = Color.red;
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().endColor = Color.red;
                    }
                }
                else if (hit.collider.gameObject.tag == "EmptyResourcePlanet" && selectedHumanPlanet != null)
                {
                    int index = humanPlanets.IndexOf(selectedHumanPlanet);
                    if (selectedPlanetToAttack[index] == null) // if there isnt a planet selected to attack
                    {
                        selectedPlanetToAttack[index] = hit.collider.gameObject;
                        AttackersStartAttacking(index);
                        if (!conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[index])])
                        {
                            conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[index])] = true;
                            StartCoroutine(ConqueringResourcePlanet(planetsToHarvest.IndexOf(selectedPlanetToAttack[index])));
                        }
                        Destroy(dottedLinesAttackingHumans[index]);
                        dottedLinesAttackingHumans[index] = Instantiate(dottedLine, transform);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(0, selectedHumanPlanet.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(1, hit.collider.gameObject.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().startColor = Color.red;
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().endColor = Color.red;
                    }
                    else if (selectedPlanetToAttack[index] == hit.collider.gameObject) // if clicked on a planet to attack already selected, deselect it and interrupt attack
                    {
                        int countH = 0;
                        int countA = 0;
                        for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                        {
                            if (selectedPlanetToAttack[i] == selectedPlanetToAttack[index])
                            {
                                countH++;
                            }
                        }
                        for (int m = 0; m < aliensList.Count; m++)
                        {
                            for (int k = 0; k < aliensList[m].Count; k++)
                            {
                                if (aliensList[m][k].GetComponent<AlienJob>().attacking && aliensList[m][k].GetComponent<GoToAttackAlien>().target == hit.collider.gameObject.transform.position)
                                {
                                    countA++;
                                    break;
                                }
                            }
                        }
                        if (countH == 1 && countA == 0) // check if there is only one human planet and no alien planets attacking the resource planet, if so interrupt conquest
                        {
                            conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[index])] = false;
                        }

                        Destroy(dottedLinesAttackingHumans[index]);
                        dottedLinesAttackingHumans[index] = null;
                        selectedPlanetToAttack[index] = null;
                        HumansGoDefend(index);
                    }
                    else // if clicked on a resource planet to attack but there is another planet already selected, interrupt attack and attack the new planet
                    {
                        Destroy(dottedLinesAttackingHumans[index]);
                        dottedLinesAttackingHumans[index] = null;
                        if (selectedPlanetToAttack[index].tag == "EmptyResourcePlanet")
                        {
                            int countH = 0;
                            int countA = 0;
                            for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                            {
                                if (selectedPlanetToAttack[i] == selectedPlanetToAttack[index])
                                {
                                    countH++;
                                }
                            }
                            for (int m = 0; m < aliensList.Count; m++)
                            {
                                for (int k = 0; k < aliensList[m].Count; k++)
                                {
                                    if (aliensList[m][k].GetComponent<AlienJob>().attacking && aliensList[m][k].GetComponent<GoToAttackAlien>().target == hit.collider.gameObject.transform.position)
                                    {
                                        countA++;
                                        break;
                                    }
                                }
                            }
                            if (countH == 1 && countA == 0) // check if there is only one human planet and no alien planets attacking the resource planet, if so interrupt conquest
                            {
                                conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[index])] = false;
                            }
                        }
                        else if (selectedPlanetToAttack[index].tag == "AlienPlanet")
                        {
                            int count = 0;
                            for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                            {
                                if (selectedPlanetToAttack[i] == selectedPlanetToAttack[index])
                                {
                                    count++;
                                }
                            }
                            if (count == 1) // check if there is only one human planet attacking the alien planet, if so interrupt conquest
                            {
                                if (aliensInFormation[alienPlanets.IndexOf(selectedPlanetToAttack[index])])
                                {
                                    AliensFree(alienPlanets.IndexOf(selectedPlanetToAttack[index]));
                                }
                                for (int i = 0; i < aliensList.Count; i++)
                                {
                                    if (aliensAttacking[i])
                                    {
                                        for (int j = 0; j < aliensList[i].Count; j++)
                                        {
                                            if (aliensList[i][j].GetComponent<GoToAttackAlien>().target == selectedPlanetToAttack[index].transform.position)
                                            {
                                                AlienRetreat();
                                                break;
                                            }
                                        }
                                    }
                                }
                                conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[index])] = false;
                            }
                        }

                        selectedPlanetToAttack[index] = hit.collider.gameObject;
                        AttackersStartAttacking(index);
                        if (!conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[index])])
                        {
                            conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[index])] = true;
                            StartCoroutine(ConqueringResourcePlanet(planetsToHarvest.IndexOf(selectedPlanetToAttack[index])));
                        }
                        dottedLinesAttackingHumans[index] = Instantiate(dottedLine, transform);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(0, selectedHumanPlanet.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().SetPosition(1, hit.collider.gameObject.transform.position);
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().startColor = Color.red;
                        dottedLinesAttackingHumans[index].GetComponent<LineRenderer>().endColor = Color.red;
                    }
                }
            }
        }
    }


    // HUMANS FUNCTIONS

    public void HumanDied(GameObject human) // removes human from sliders and humans list
    {
        for (int i = 0; i < humansList.Count; i++)
        {
            if (humansList[i].IndexOf(human) != -1)
            {
                humansList[i].Remove(human);
                Destroy(human);
                if (humansList[i].Count == 0 && selectedPlanetToAttack[i].tag == "AlienPlanet")
                {
                    int count = 0;
                    for (int j = 0; j < selectedPlanetToAttack.Count; j++)
                    {
                        if (selectedPlanetToAttack[j] == selectedPlanetToAttack[i] && humansList[j].Count != 0)
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        AliensFree(alienPlanets.IndexOf(selectedPlanetToAttack[i]));
                    }
                }
                if (selectedHumanPlanet == null)
                {
                    int tot = GetNumHumans();
                    slider.maxValue = tot;
                    harvestingSlider.maxValue = tot;
                    inFormationSlider.maxValue = tot;
                    pass = true;
                    UpdateSlider();
                    UpdateHarvestingSlider();
                    UpdateInFormationSlider();
                    populationTM.text = GetNumHumans().ToString();
                }
                else
                {
                    int index = humanPlanets.IndexOf(selectedHumanPlanet);
                    int tot = GetNumHumans(index);
                    slider.maxValue = tot;
                    harvestingSlider.maxValue = tot;
                    inFormationSlider.maxValue = tot;
                    UpdateSlider(index);
                    UpdateHarvestingSlider(index);
                    UpdateInFormationSlider(index);
                    populationTM.text = GetNumHumans(index).ToString();
                }
                break;
            }
        }
        if (humanHarvesters.IndexOf(human) != -1)
        {
            humanHarvesters.Remove(human);
        }
        if (humanDefendersOrAttackers.IndexOf(human) != -1)
        {
            humanDefendersOrAttackers.Remove(human);
        }
    }

    IEnumerator ConqueringHumanPlanet(int index)
    {
        GameObject humanPlanet = humanPlanets[index];
        while (true)
        {
            Debug.Log("H");
            int i = humanPlanets.IndexOf(humanPlanet);
            if (!conqueringHumanPlanets[i])
            {
                humanPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount = 0f;
                yield break;
            }
            int countAliens = 0;
            int countHumans = 0;
            for (int j = 0; j < aliensList.Count; j++)
            {
                for (int k = 0; k < aliensList[j].Count; k++)
                {
                    if (aliensList[j][k].GetComponent<AlienJob>().attacking && Vector3.Distance(humanPlanets[i].transform.position, aliensList[j][k].transform.position) <= 6f
                        && aliensList[j][k].GetComponent<GoToAttackAlien>().target == humanPlanets[i].transform.position)
                    {
                        countAliens++;
                    }
                }
            }
            for (int j = 0; j < humansList.Count; j++)
            {
                for (int k = 0; k < humansList[j].Count; k++)
                {
                    if ((humansList[j][k].GetComponent<HumanJob>().attacking || humansList[j][k].GetComponent<HumanJob>().goingIntoFormation
                        || humansList[j][k].GetComponent<HumanJob>().inFormation) && Vector3.Distance(humanPlanets[i].transform.position, humansList[j][k].transform.position) <= 6f)
                    {
                        countHumans++;
                    }
                }
            }
            int count = countAliens - countHumans;
            humanPlanets[i].transform.GetChild(0).GetComponent<Canvas>().enabled = true;
            humanPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().enabled = true;
            humanPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount += 0.005f * count; // increase green ring

            yield return new WaitForSeconds(1f);

            i = humanPlanets.IndexOf(humanPlanet);
            if (humanPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount >= 1f) // if it is conquered
            {
                for (int j = 0; j < aliensList.Count; j++)
                {
                    for (int k = 0; k < aliensList[j].Count; k++)
                    {
                        if (aliensList[j][k].GetComponent<GoToAttackAlien>().target == humanPlanets[i].transform.position)
                        {
                            AliensFree(j);
                            break;
                        }
                    }
                }
                // New alien planet
                aliens = new List<GameObject>();
                aliensList.Add(aliens);
                alienPlanets.Add(Instantiate(alienPlanet, humanPlanets[i].transform.position, transform.rotation));
                conqueringAlienPlanets.Add(false);
                aliensAttacking.Add(false);
                aliensInFormation.Add(false);
                numStartingAliens = Random.Range(2, 5);
                for (int m = 0; m < numStartingAliens; m++)
                {
                    AddAlien(alienPlanets[alienPlanets.Count - 1]);
                }
                selectedPlanetsToHarvestAlienList.Add(new List<GameObject>());
                dottedLinesHarvestingAliensList.Add(new List<GameObject>());

                // If there are humans that are going to defend an human planet but this one is conquered and becomes an alien planet
                // start the conquest for this new alien planet
                for (int m = 0; m < humansList.Count; m++)
                {
                    if (selectedPlanetToAttack[m] != null)
                    {
                        if (alienPlanets[alienPlanets.Count - 1].transform.position == selectedPlanetToAttack[m].transform.position)
                        {
                            selectedPlanetToAttack[m] = alienPlanets[alienPlanets.Count - 1];
                            if (!conqueringAlienPlanets[alienPlanets.Count - 1])
                            {
                                conqueringAlienPlanets[alienPlanets.Count - 1] = true;
                                StartCoroutine(ConqueringAlienPlanet(alienPlanets.Count - 1));
                            }
                            if (humansList[m].Count > 0)
                            {
                                AliensInFormation(alienPlanets.Count - 1);
                            }
                        }
                    }
                }

                // Destroy human planet
                Destroy(humanPlanets[i]);
                humanPlanets.RemoveAt(i);
                selectedPlanetsToHarvestHumanList.RemoveAt(i);
                for (int l = 0; l < dottedLinesHarvestingHumanList[i].Count; l++)
                {
                    Destroy(dottedLinesHarvestingHumanList[i][l]);
                }
                dottedLinesHarvestingHumanList.RemoveAt(i);
                indexPlanetToHarvest.RemoveAt(i);
                selectedPlanetToAttack.RemoveAt(i);
                Destroy(dottedLinesAttackingHumans[i]);
                dottedLinesAttackingHumans.RemoveAt(i);
                humansList.RemoveAt(i);
                conqueringHumanPlanets.RemoveAt(i);

                yield break;
            }
        }
    }

    IEnumerator ConqueringAlienPlanet(int index)
    {
        GameObject alienPlanet = alienPlanets[index];
        bool startedIncreasingRing = false;
        while (true)
        {
            Debug.Log("A");
            int i = alienPlanets.IndexOf(alienPlanet);
            if (!conqueringAlienPlanets[i])
            {
                alienPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                yield break;
            }
            int countHumans = 0;
            int countAliens = 0;
            for (int j = 0; j < humansList.Count; j++)
            {
                for (int k = 0; k < humansList[j].Count; k++)
                {
                    if (humansList[j][k].GetComponent<HumanJob>().attacking && Vector3.Distance(alienPlanets[i].transform.position, humansList[j][k].transform.position) <= 6f
                        && humansList[j][k].GetComponent<GoToAttackHuman>().target == alienPlanets[i].transform.position)
                    {
                        countHumans++;
                    }
                }
            }
            for (int j = 0; j < aliensList.Count; j++)
            {
                for (int k = 0; k < aliensList[j].Count; k++)
                {
                    if ((aliensList[j][k].GetComponent<AlienJob>().attacking || aliensList[j][k].GetComponent<AlienJob>().goingIntoFormation
                        || aliensList[j][k].GetComponent<AlienJob>().inFormation) && Vector3.Distance(alienPlanets[i].transform.position, aliensList[j][k].transform.position) <= 6f)
                    {
                        countAliens++;
                    }
                }
            }
            int count = countHumans - countAliens;
            alienPlanets[i].transform.GetChild(0).GetComponent<Canvas>().enabled = true;
            alienPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().enabled = true;
            alienPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount += 0.005f * count; // increase white ring

            if (alienPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount > 0 && !startedIncreasingRing)
            {
                startedIncreasingRing = true;
                StartAlienDefence(alienPlanets[i].transform.position);
            }
            else if (alienPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount == 0 && startedIncreasingRing
                && countHumans == 0)
            {
                startedIncreasingRing = false;
                AlienRetreat();
            }

            yield return new WaitForSeconds(1f);

            if (alienPlanet == null)
            {
                yield break;
            }
            i = alienPlanets.IndexOf(alienPlanet);
            if (alienPlanets[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount >= 1f) // if it is conquered
            {
                for (int j = 0; j < humansList.Count; j++)
                {
                    if (selectedPlanetToAttack[j] == alienPlanets[i])
                    {
                        Destroy(dottedLinesAttackingHumans[j]);
                        dottedLinesAttackingHumans[j] = null;
                        selectedPlanetToAttack[j] = null;
                        HumansGoDefend(j);
                    } 
                }

                // New human planet
                humans = new List<GameObject>();
                humanPlanets.Add(Instantiate(humanPlanet, alienPlanets[i].transform.position, transform.rotation));
                selectedPlanetsToHarvestHumanList.Add(new List<GameObject>());
                dottedLinesHarvestingHumanList.Add(new List<GameObject>());
                indexPlanetToHarvest.Add(-1);
                selectedPlanetToAttack.Add(null);
                dottedLinesAttackingHumans.Add(null);
                conqueringHumanPlanets.Add(false);
                numStartingHumans = Random.Range(2, 5);
                for (int m = 0; m < numStartingHumans; m++)
                    humans.Add(Instantiate(human, humanPlanets[humanPlanets.Count - 1].transform.position, transform.rotation));
                humansList.Add(humans);

                if (selectedHumanPlanet == null)
                {
                    int tot = GetNumHumans();
                    slider.maxValue = tot;
                    harvestingSlider.maxValue = tot;
                    inFormationSlider.maxValue = tot;
                    UpdateSlider();
                    UpdateHarvestingSlider();
                    UpdateInFormationSlider();
                }
                else
                {
                    int j = humanPlanets.IndexOf(selectedHumanPlanet);
                    int tot = GetNumHumans(j);
                    slider.maxValue = tot;
                    harvestingSlider.maxValue = tot;
                    inFormationSlider.maxValue = tot;
                    UpdateSlider(j);
                    UpdateHarvestingSlider(j);
                    UpdateInFormationSlider(j);
                }

                float humanPlanetScale2 = 0.5f * Mathf.Pow(FindNextSquare(humansList[humanPlanets.Count - 1].Count), 0.5f);
                humanPlanets[humanPlanets.Count - 1].transform.localScale = new Vector3(humanPlanetScale2, humanPlanetScale2, 0f);
                for (int m = 0; m < humansList[humanPlanets.Count - 1].Count; m++)
                    humansList[humanPlanets.Count - 1][m].GetComponent<Movement>().radius = humanPlanetScale2 / 2f;

                // If there are aliens that are going to defend an alien planet but this one is conquered and becomes an human planet
                // start the conquest for this new human planet
                for (int m = 0; m < aliensList.Count; m++)
                {
                    for (int k = 0; k < aliensList[m].Count; k++)
                    {
                        if (aliensList[m][k].GetComponent<AlienJob>().attacking && aliensList[m][k].GetComponent<GoToAttackAlien>().target == alienPlanets[i].transform.position)
                        {
                            if (!conqueringHumanPlanets[humanPlanets.Count - 1])
                            {
                                conqueringHumanPlanets[humanPlanets.Count - 1] = true;
                                StartCoroutine(ConqueringHumanPlanet(humanPlanets.Count - 1));
                            }
                            indexLastHumanPlanetAttacked = humanPlanets.Count - 1;
                            break;
                        }
                    }
                }

                // Destroy alien planet
                Destroy(alienPlanets[i]);
                alienPlanets.RemoveAt(i);
                aliensList.RemoveAt(i);
                for (int l = 0; l < dottedLinesHarvestingAliensList[i].Count; l++)
                {
                    Destroy(dottedLinesHarvestingAliensList[i][l]);
                }
                dottedLinesHarvestingAliensList.RemoveAt(i);
                selectedPlanetsToHarvestAlienList.RemoveAt(i);
                conqueringAlienPlanets.RemoveAt(i);
                aliensAttacking.RemoveAt(i);
                aliensInFormation.RemoveAt(i);

                yield break;
            }
        }
    }

    IEnumerator ConqueringResourcePlanet(int index)
    {
        GameObject resourcePlanet = planetsToHarvest[index];
        bool itsGreen = false;
        bool itsWhite = false;
        while (true)
        {
            //Debug.Log("R");
            int i = planetsToHarvest.IndexOf(resourcePlanet);
            if (!conqueringResourcePlanets[i])
            {
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount = 0f;
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
                yield break;
            }
            int countAliens = 0;
            int countHumans = 0;
            for (int j = 0; j < aliensList.Count; j++)
            {
                for (int k = 0; k < aliensList[j].Count; k++)
                {
                    if (aliensList[j][k].GetComponent<AlienJob>().attacking && Vector3.Distance(planetsToHarvest[i].transform.position, aliensList[j][k].transform.position) <= 6f
                        && aliensList[j][k].GetComponent<GoToAttackAlien>().target == planetsToHarvest[i].transform.position)
                    {
                        countAliens++;
                    }
                }
            }
            for (int j = 0; j < humansList.Count; j++)
            {
                for (int k = 0; k < humansList[j].Count; k++)
                {
                    if ((humansList[j][k].GetComponent<HumanJob>().attacking || humansList[j][k].GetComponent<HumanJob>().goingIntoFormation
                        || humansList[j][k].GetComponent<HumanJob>().inFormation) && Vector3.Distance(planetsToHarvest[i].transform.position, humansList[j][k].transform.position) <= 6f)
                    {
                        countHumans++;
                    }
                }
            }
            int count = countAliens - countHumans;
            if (count > 0 && !itsGreen && !itsWhite) // more aliens for the first time (no ring)
            {
                itsGreen = true;
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount += 0.005f * count; // increase green ring
            }
            else if (count < 0 && !itsGreen && !itsWhite) // more humans for the first time (no ring)
            {
                itsWhite = true;
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount += 0.005f * (-count); // increase white ring
            }
            else if (count > 0 && itsGreen && !itsWhite) // more aliens and the ring is green
            {
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount += 0.005f * count; // increase green ring
            }
            else if (count < 0 && !itsGreen && itsWhite) // more humans and the ring is white
            {
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount += 0.005f * (-count); // increase white ring
            }
            else if (count < 0 && itsGreen && !itsWhite && planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount > 0) 
                // more humans and the ring is green
            {
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount -= 0.005f * (-count); // decrease green ring
            }
            else if (count > 0 && !itsGreen && itsWhite && planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount > 0)
                // more aliens and the ring is white
            {
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount -= 0.005f * count; // decrease white ring
            }
            else if (count > 0 && !itsGreen && itsWhite && planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount == 0)
                // more aliens and there was white ring but now is 0
            {
                itsGreen = true;
                itsWhite = false;
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount += 0.005f * count; // increase green ring
            }
            else if (count < 0 && itsGreen && !itsWhite && planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount == 0)
                // more humans and there was green ring but now is 0
            {
                itsGreen = false;
                itsWhite = true;
                planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount += 0.005f * (-count); // increase white ring
            }

            yield return new WaitForSeconds(1f);

            i = planetsToHarvest.IndexOf(resourcePlanet);
            if (planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(1).GetComponent<Image>().fillAmount >= 1f) // if it is conquered by humans
            {
                for (int j = 0; j < humansList.Count; j++)
                {
                    if (selectedPlanetToAttack[j] == planetsToHarvest[i])
                    {
                        Destroy(dottedLinesAttackingHumans[j]);
                        dottedLinesAttackingHumans[j] = null;
                        selectedPlanetToAttack[j] = null;
                        HumansGoDefend(j);
                    }
                }

                // New human planet
                humans = new List<GameObject>();
                humanPlanets.Add(Instantiate(humanPlanet, planetsToHarvest[i].transform.position, transform.rotation));
                selectedPlanetsToHarvestHumanList.Add(new List<GameObject>());
                dottedLinesHarvestingHumanList.Add(new List<GameObject>());
                indexPlanetToHarvest.Add(-1);
                selectedPlanetToAttack.Add(null);
                dottedLinesAttackingHumans.Add(null);
                conqueringHumanPlanets.Add(false);
                numStartingHumans = Random.Range(2, 5);
                for (int m = 0; m < numStartingHumans; m++)
                    humans.Add(Instantiate(human, humanPlanets[humanPlanets.Count - 1].transform.position, transform.rotation));
                humansList.Add(humans);

                if (selectedHumanPlanet == null)
                {
                    int tot = GetNumHumans();
                    slider.maxValue = tot;
                    harvestingSlider.maxValue = tot;
                    inFormationSlider.maxValue = tot;
                    UpdateSlider();
                    UpdateHarvestingSlider();
                    UpdateInFormationSlider();
                }
                else
                {
                    int j = humanPlanets.IndexOf(selectedHumanPlanet);
                    int tot = GetNumHumans(j);
                    slider.maxValue = tot;
                    harvestingSlider.maxValue = tot;
                    inFormationSlider.maxValue = tot;
                    UpdateSlider(j);
                    UpdateHarvestingSlider(j);
                    UpdateInFormationSlider(j);
                }

                float humanPlanetScale2 = 0.5f * Mathf.Pow(FindNextSquare(humansList[humanPlanets.Count - 1].Count), 0.5f);
                humanPlanets[humanPlanets.Count - 1].transform.localScale = new Vector3(humanPlanetScale2, humanPlanetScale2, 0f);
                for (int m = 0; m < humansList[humanPlanets.Count - 1].Count; m++)
                    humansList[humanPlanets.Count - 1][m].GetComponent<Movement>().radius = humanPlanetScale2 / 2f;

                // If there are aliens that are going to conquer a resource planet but this one is conquered and becomes an human planet
                // start the conquest for this new human planet
                for (int m = 0; m < aliensList.Count; m++)
                {
                    for (int k = 0; k < aliensList[m].Count; k++)
                    {
                        if (aliensList[m][k].GetComponent<AlienJob>().attacking && aliensList[m][k].GetComponent<GoToAttackAlien>().target == planetsToHarvest[i].transform.position)
                        {
                            if (!conqueringHumanPlanets[humanPlanets.Count - 1])
                            {
                                conqueringHumanPlanets[humanPlanets.Count - 1] = true;
                                StartCoroutine(ConqueringHumanPlanet(humanPlanets.Count - 1));
                            }
                            indexLastHumanPlanetAttacked = humanPlanets.Count - 1;
                            break;
                        }
                    }
                }

                // Destroy resource planet
                Destroy(planetsToHarvest[i]);
                planetsToHarvest.RemoveAt(i);
                emptyResourcePlanets.RemoveAt(i);
                conqueringResourcePlanets.RemoveAt(i);

                yield break;
            }
            else if (planetsToHarvest[i].transform.GetChild(0).GetComponent<Canvas>().transform.GetChild(0).GetComponent<Image>().fillAmount >= 1f) // if it is conquered by aliens
            {
                for (int j = 0; j < aliensList.Count; j++)
                {
                    for (int k = 0; k < aliensList[j].Count; k++)
                    {
                        if (aliensList[j][k].GetComponent<GoToAttackAlien>().target == planetsToHarvest[i].transform.position)
                        {
                            AliensFree(j);
                            break;
                        }
                    }
                }
                // New alien planet
                aliens = new List<GameObject>();
                aliensList.Add(aliens);
                alienPlanets.Add(Instantiate(alienPlanet, planetsToHarvest[i].transform.position, transform.rotation));
                conqueringAlienPlanets.Add(false);
                aliensAttacking.Add(false);
                aliensInFormation.Add(false);
                numStartingAliens = Random.Range(2, 5);
                for (int m = 0; m < numStartingAliens; m++)
                {
                    AddAlien(alienPlanets[alienPlanets.Count - 1]);
                }
                selectedPlanetsToHarvestAlienList.Add(new List<GameObject>());
                dottedLinesHarvestingAliensList.Add(new List<GameObject>());

                // If there are humans that are going to conquer a resource planet but this one is conquered and becomes an alien planet
                // start the conquest for this new alien planet
                for (int m = 0; m < humansList.Count; m++)
                {
                    if (selectedPlanetToAttack[m] != null)
                    {
                        if (alienPlanets[alienPlanets.Count - 1].transform.position == selectedPlanetToAttack[m].transform.position)
                        {
                            selectedPlanetToAttack[m] = alienPlanets[alienPlanets.Count - 1];
                            if (!conqueringAlienPlanets[alienPlanets.Count - 1])
                            {
                                conqueringAlienPlanets[alienPlanets.Count - 1] = true;
                                StartCoroutine(ConqueringAlienPlanet(alienPlanets.Count - 1));
                            }
                            if (humansList[m].Count > 0)
                            {
                                AliensInFormation(alienPlanets.Count - 1);
                            }    
                        }
                    }
                }

                // Destroy resource planet
                Destroy(planetsToHarvest[i]);
                planetsToHarvest.RemoveAt(i);
                emptyResourcePlanets.RemoveAt(i);
                conqueringResourcePlanets.RemoveAt(i);

                yield break;
            }
        }
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

    public void AddHuman(int index)
    {
        if (humansList[index].Count < 50)
        {
            humansList[index].Add(Instantiate(human, humanPlanets[index].transform.position, transform.rotation));
            int count = humansList[index].Count;
            humansList[index][count - 1].GetComponent<Movement>().radius = humanPlanets[index].transform.localScale.x / 2f;
            if (selectedHumanPlanet == null)
            {
                int tot = GetNumHumans();
                slider.maxValue = tot;
                harvestingSlider.maxValue = tot;
                inFormationSlider.maxValue = tot;
                UpdateSlider();
                UpdateHarvestingSlider();
                UpdateInFormationSlider();
            }
            else
            {
                int j = humanPlanets.IndexOf(selectedHumanPlanet);
                int tot = GetNumHumans(j);
                slider.maxValue = tot;
                harvestingSlider.maxValue = tot;
                inFormationSlider.maxValue = tot;
                UpdateSlider(j);
                UpdateHarvestingSlider(j);
                UpdateInFormationSlider(j);
            }
            UpdatePlanet(count, index);
        }
    }

    public void UpdatePlanet(int count, int index) // increases the size of human planet at specific population numbers
    {
        if (count > 8 && count < 38)
        {
            if (Mathf.Sqrt(count - 1) % 1 == 0)
            {
                float humanPlanetScale = 0.5f * Mathf.Pow(FindNextSquare(count), 0.5f);
                humanPlanets[index].transform.localScale = new Vector3(humanPlanetScale, humanPlanetScale, 0f);
                RecalculateFormation();
                for (int i = 0; i < count; i++)
                    humansList[index][i].GetComponent<Movement>().radius = humanPlanetScale / 2f;
            }
        }
    }

    public Vector3 HarvestingPlanetsDistributor(int index) // distributes equally human harvesters to harvest selected planets for harvesting
    {
        indexPlanetToHarvest[index] += 1;
        if (indexPlanetToHarvest[index] < selectedPlanetsToHarvestHumanList[index].Count)
        {
            return selectedPlanetsToHarvestHumanList[index][indexPlanetToHarvest[index]].transform.position;
        }
        else
        {
            indexPlanetToHarvest[index] = 0;
            return selectedPlanetsToHarvestHumanList[index][indexPlanetToHarvest[index]].transform.position;
        }
    }

    public void RecalculateFormation() // rearranges human defenders
    {
        for (int j = 0; j < humansList.Count; j++)
        {
            List<GameObject> entsToPass = new List<GameObject>();
            for (int i = 0; i < humansList[j].Count; i++)
            {
                if (humansList[j][i].GetComponent<HumanJob>().inFormation || humansList[j][i].GetComponent<HumanJob>().goingIntoFormation)
                {
                    entsToPass.Add(humansList[j][i]);
                }
            }
            Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, humanPlanets[j].transform.position, humanPlanets[j].transform.localScale.x / 2f);
            for (int i = 0; i < entsToPass.Count; i++)
            {
                entsToPass[i].GetComponent<HumanJob>().SetIsGoingIntoFormation();
                entsToPass[i].GetComponent<GoInFormationHuman>().enabled = true;
                entsToPass[i].GetComponent<GoInFormationHuman>().coords = humansTargetPos[i];
            }
        }
    }

    public void AttackingAborted(int j)
    {
        for (int i = 0; i < humansList[j].Count; i++)
        {
            if (humansList[j][i].GetComponent<HumanJob>().attacking)
            {
                humansList[j][i].GetComponent<GoToAttackHuman>().enabled = false;
                //humansList[j][i].GetComponent<CombatModeHuman>().StopCombatModeHuman();
                humansList[j][i].GetComponent<Movement>().enabled = true;
            }
        }
    }

    public void HumansGoDefend(int j)
    {
        List<GameObject> entsToPass = new List<GameObject>();
        for (int i = 0; i < humansList[j].Count; i++)
        {
            if (humansList[j][i].GetComponent<HumanJob>().attacking ||
                humansList[j][i].GetComponent<HumanJob>().inFormation || humansList[j][i].GetComponent<HumanJob>().goingIntoFormation)
            {
                entsToPass.Add(humansList[j][i]);
            }
        }
        Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, humanPlanets[j].transform.position, humanPlanets[j].transform.localScale.x / 2f);
        for (int i = 0; i < entsToPass.Count; i++)
        {
            entsToPass[i].GetComponent<GoToAttackHuman>().enabled = false;
            entsToPass[i].GetComponent<HumanJob>().SetIsGoingIntoFormation();
            entsToPass[i].GetComponent<Movement>().enabled = false;
            entsToPass[i].GetComponent<GoInFormationHuman>().enabled = true;
            entsToPass[i].GetComponent<GoInFormationHuman>().coords = humansTargetPos[i];
        }
    }

    public void HarvestersStartHarvesting(int j) // if there are harvesters but no planets selected for harvesting and a new planet to harvest is selected
    {
        for (int i = 0; i < humansList[j].Count; i++)
        {
            if (humansList[j][i].GetComponent<HumanJob>().harvesting)
            {
                humansList[j][i].GetComponent<Movement>().enabled = false;
                humansList[j][i].GetComponent<HarvestResources>().enabled = true;
                humansList[j][i].GetComponent<HarvestResources>().SetPlanet(HarvestingPlanetsDistributor(j),
                    new Vector3(humanPlanets[j].transform.position.x + Random.insideUnitCircle.x * (humanPlanets[j].transform.localScale.x / 2f),
                    humanPlanets[j].transform.position.y + Random.insideUnitCircle.y * (humanPlanets[j].transform.localScale.x / 2f), 0));
            }
        }
    }

    public void AttackersStartAttacking(int j) // if there are attackers but no planets selected for attacking and a new planet to attack is selected
    {
        bool flag = false; // to control if at least one human started to attack
        for (int i = 0; i < humansList[j].Count; i++)
        {
            if (humansList[j][i].GetComponent<HumanJob>().attacking || humansList[j][i].GetComponent<HumanJob>().inFormation || humansList[j][i].GetComponent<HumanJob>().goingIntoFormation)
            {
                humansList[j][i].GetComponent<Movement>().enabled = false;
                humansList[j][i].GetComponent<GoInFormationHuman>().enabled = false;
                humansList[j][i].GetComponent<GoToAttackHuman>().enabled = true;
                humansList[j][i].GetComponent<HumanJob>().SetIsAttacking();
                humansList[j][i].GetComponent<GoToAttackHuman>().SetTarget(selectedPlanetToAttack[j].transform.position); // it can be an alien planet to attack or an human planet to defend
                humansList[j][i].GetComponent<CombatModeHuman>().StartCombatModeHuman();
                flag = true;
            }
        }
        if (selectedPlanetToAttack[j].tag == "AlienPlanet" && flag && !aliensInFormation[alienPlanets.IndexOf(selectedPlanetToAttack[j])])
        {
            AliensInFormation(alienPlanets.IndexOf(selectedPlanetToAttack[j]));
        }
    }

    public void HarvestingAborted(GameObject planetToStopHarvesting) // if clicked on a planet already selected for harvesting, interrupt harvesting for that planet
    {
        for (int j = 0; j < humansList.Count; j++)
        {
            for (int i = 0; i < humansList[j].Count; i++)
            {
                if (humansList[j][i].GetComponent<HumanJob>().harvesting && humansList[j][i].GetComponent<HarvestResources>().coorPlanet == planetToStopHarvesting.transform.position)
                {
                    humansList[j][i].GetComponent<HarvestResources>().PlanetOutOfResources();
                }
            }
        }
    }

    public Vector3 WhereAmIFrom(GameObject human)
    {
        for (int i = 0; i < humansList.Count; i++)
        {
            if (humansList[i].IndexOf(human) != -1)
            {
                return new Vector3(humanPlanets[i].transform.position.x + Random.insideUnitCircle.x * (humanPlanets[i].transform.localScale.x / 2f),
                    humanPlanets[i].transform.position.y + Random.insideUnitCircle.y * (humanPlanets[i].transform.localScale.x / 2f), 0);
            }
        }
        return humanPlanets[0].transform.position;
    }

    public int GetMyPlanetId(GameObject human)
    {
        for (int i = 0; i < humansList.Count; i++)
        {
            if (humansList[i].IndexOf(human) != -1)
            {
                return i;
            }
        }
        return 0;
    }

    public void MoveHumans(int from, int to, int num)
    {
        if (humansList[to].Count + num < 51 && GetFreeHumans(from) - num >= 0)
        {
            for (int i = 0; i < num; i++)
            {
                for (int k = 0; k < humansList[from].Count; k++)
                {
                    if (humansList[from][k].GetComponent<HumanJob>().free)
                    {
                        humansList[from][k].GetComponent<Movement>().SetStartPos(new Vector2(humanPlanets[to].transform.position.x, humanPlanets[to].transform.position.y));
                        humansList[from][k].GetComponent<Movement>().radius = humanPlanets[to].transform.localScale.x / 2f;
                        GameObject temp = humansList[from][k];
                        humansList[from].RemoveAt(k);
                        humansList[to].Add(temp);
                        UpdatePlanet(humansList[from].Count, from);
                        UpdatePlanet(humansList[to].Count, to);
                        if (selectedHumanPlanet != null)
                        {
                            int j = humanPlanets.IndexOf(selectedHumanPlanet);
                            int tot = GetNumHumans(j);
                            slider.maxValue = tot;
                            harvestingSlider.maxValue = tot;
                            inFormationSlider.maxValue = tot;
                            UpdateSlider(j);
                            UpdateHarvestingSlider(j);
                            UpdateInFormationSlider(j);
                            populationTM.text = GetNumHumans(j).ToString();
                        }
                        break;
                    }
                }
            }
        }
    }


    // ALIENS FUNCTIONS

    public void RemoveAlien(GameObject alien) // removes alien from its list
    {
        for (int i = 0; i < aliensList.Count; i++)
        {
            if (aliensList[i].IndexOf(alien) != -1)
            {
                aliensList[i].Remove(alien);
                if (GetNumAliens() < 15)
                {
                    AlienRetreat();
                }
                break;
            }
        }
    }

    public void AddAlien(GameObject alienPlanet) // add a new alien to a specific alien planet and increases the size of its alien planet at specific population numbers
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
                    alienPlanetScale = 0.5f * Mathf.Pow(FindNextSquare(count), 0.5f);
                    alienPlanets[index].transform.localScale = new Vector3(alienPlanetScale, alienPlanetScale, 0f);
                    for (int i = 0; i < count; i++)
                        aliensList[index][i].GetComponent<Movement>().radius = alienPlanetScale / 2f;
                }
            }
            RecalculateFormationAliens(index);
        }
    }

    public void AddAlien() // add a new alien to every alien planet and increases the size alien planets at specific population numbers
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
                        alienPlanetScale = 0.5f * Mathf.Pow(FindNextSquare(count), 0.5f);
                        alienPlanets[i].transform.localScale = new Vector3(alienPlanetScale, alienPlanetScale, 0f);
                        for (int j = 0; j < count; j++)
                            aliensList[i][j].GetComponent<Movement>().radius = alienPlanetScale / 2f;
                    }
                }
                RecalculateFormationAliens(i);
            }
        }
    }

    public void AliensInFormation(int index) // puts aliens of a specific alien planet in defensive formation
    {
        aliensAttacking[index] = false;
        aliensInFormation[index] = true;
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

    public void StartAlienHarvesting()
    {
        for (int i = 0; i < aliensList.Count; i++)
        {
            if (dottedLinesHarvestingAliensList[i].Count == 0 && !aliensAttacking[i] && !aliensInFormation[i] && aliensList[i].Count > 0)
            {
                AliensHarvesting(i);
            }
        }
    }

    public void AliensHarvesting(int index) // aliens of a specific alien planet start harvesting
    {
        aliensAttacking[index] = false;
        aliensInFormation[index] = false;
        if (dottedLinesHarvestingAliensList[index].Count > 0) // if they are already harvesting they stop
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
            for (int i = 0; i < aliensList[index].Count; i += 2)
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

    public void AliensStopHarvesting(int index) // aliens of a specific alien planet stop harvesting
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

    public void StartAlienAttack()
    {
        if (conqueringHumanPlanets.Count > 0)
        {
            for (int i = 0; i < aliensList.Count; i++)
            {
                if (!aliensInFormation[i] && !aliensAttacking[i] && aliensList[i].Count > 0)
                {
                    AlienAttack(i);
                }
            }
        }
    }

    public void AlienAttack(int index) // aliens from a specific alien planet start attacking humans
    {
        aliensAttacking[index] = true;
        aliensInFormation[index] = false;
        int planetToAttack = GetSmallestHumanPlanet();
        indexLastHumanPlanetAttacked = planetToAttack;
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            aliensList[index][i].GetComponent<GoInFormationAlien>().enabled = false;
            AliensStopHarvesting(index);
            aliensList[index][i].GetComponent<Movement>().enabled = false;
            aliensList[index][i].GetComponent<CombatModeAlien>().StartCombatModeAlien();
            aliensList[index][i].GetComponent<GoToAttackAlien>().enabled = true;
            aliensList[index][i].GetComponent<GoToAttackAlien>().SetTarget(humanPlanets[planetToAttack].transform.position);
            aliensList[index][i].GetComponent<AlienJob>().SetIsAttacking();
        }
        if (!conqueringHumanPlanets[planetToAttack])
        {
            conqueringHumanPlanets[planetToAttack] = true;
            StartCoroutine(ConqueringHumanPlanet(planetToAttack));
        }
    }

    public void StartAlienDefence(Vector3 posToDefend)
    {
        if (GetNumAliens() > 15)
        {
            for (int i = 0; i < aliensList.Count; i++)
            {
                if (!aliensInFormation[i] && aliensList[i].Count > 0)
                {
                    AlienDefence(i, posToDefend);
                }
            }
        }
    }

    public void AlienDefence(int index, Vector3 posToDefend)
    {
        aliensAttacking[index] = true;
        aliensInFormation[index] = false;
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            aliensList[index][i].GetComponent<GoInFormationAlien>().enabled = false;
            AliensStopHarvesting(index);
            aliensList[index][i].GetComponent<Movement>().enabled = false;
            aliensList[index][i].GetComponent<CombatModeAlien>().StartCombatModeAlien();
            aliensList[index][i].GetComponent<GoToAttackAlien>().enabled = true;
            aliensList[index][i].GetComponent<GoToAttackAlien>().SetTarget(posToDefend);
            aliensList[index][i].GetComponent<AlienJob>().SetIsAttacking();
        }
    }

    public void AliensFree(int index) // aliens of a specific alien planet are set free
    {
        aliensAttacking[index] = false;
        aliensInFormation[index] = false;
        AliensStopHarvesting(index);
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            aliensList[index][i].GetComponent<GoInFormationAlien>().enabled = false;
            aliensList[index][i].GetComponent<GoToAttackAlien>().enabled = false;
            aliensList[index][i].GetComponent<CombatModeAlien>().StopCombatModeAlien();
            aliensList[index][i].GetComponent<HarvestResourcesAlien>().ResetHarvester();
            aliensList[index][i].GetComponent<Movement>().enabled = true;
            aliensList[index][i].GetComponent<AlienJob>().SetIsFree();
        }
        Invoke("StartAlienHarvesting", 5f);
    }

    public void AlienRetreat() // aliens of a specific planet retreat
    {
        for (int i = 0; i < aliensList.Count; i++)
        {
            if (aliensAttacking[i])
            {
                AliensFree(i);
            }
        }
        conqueringHumanPlanets[indexLastHumanPlanetAttacked] = false;
    }

    public void StartAlienConquestResourcePlanet()
    {
        for (int i = 0; i < aliensList.Count; i++)
        {
            if (!aliensAttacking[i] && !aliensInFormation[i] && aliensList[i].Count > 0)
            {
                AlienConquestResourcePlanet(i);
            }
        }
    }

    public void AlienConquestResourcePlanet(int index) // aliens from a specific alien planet start conquering a resource planet
    {
        aliensAttacking[index] = true;
        aliensInFormation[index] = false;
        int resourcePlanetToConquest = GetNearestResourcePlanetToConquest();
        indexLastResoursePlanetAttackedAliens = resourcePlanetToConquest;
        for (int i = 0; i < aliensList[index].Count; i++)
        {
            aliensList[index][i].GetComponent<GoInFormationAlien>().enabled = false;
            AliensStopHarvesting(index);
            aliensList[index][i].GetComponent<Movement>().enabled = false;
            aliensList[index][i].GetComponent<CombatModeAlien>().StartCombatModeAlien();
            aliensList[index][i].GetComponent<GoToAttackAlien>().enabled = true;
            aliensList[index][i].GetComponent<GoToAttackAlien>().SetTarget(planetsToHarvest[resourcePlanetToConquest].transform.position);
            aliensList[index][i].GetComponent<AlienJob>().SetIsAttacking();
        }
        if (!conqueringResourcePlanets[resourcePlanetToConquest])
        {
            conqueringResourcePlanets[resourcePlanetToConquest] = true;
            StartCoroutine(ConqueringResourcePlanet(resourcePlanetToConquest));
        }
    } 

    public int GetSmallestHumanPlanet()
    {
        int min = 100;
        int index = -1;
        for (int i = 0; i < humansList.Count; i++)
        {
            if (humansList[i].Count < min)
            {
                min = humansList[i].Count;
                index = i;
            }
        }
        return index;
    }

    public int GetNearestResourcePlanetToConquest()
    {
        float minDistance = float.MaxValue;
        float dis;
        int index = 0;
        for (int i = 0; i < planetsToHarvest.Count; i++)
        {
            if (emptyResourcePlanets[i])
            {
                dis = Vector3.Distance(alienPlanets[0].transform.position, planetsToHarvest[i].transform.position);
                if (dis < minDistance)
                {
                    minDistance = dis;
                    index = i;
                }
            }
        }
        return index;
    }

    public void RecalculateFormationAliens(int index) // rearranges alien defenders
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

    public GameObject GetClosestHuman(Vector3 pos, float maxRange)
    {
        GameObject closest = null;
        for (int i = 0; i < humansList.Count; i++)
        {
            foreach (GameObject human in humansList[i])
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
        }
        return closest;
    }

    public GameObject FindNearestPlanetToHarvestForAliens(GameObject alienPlanet)
    {
        float minDistance = float.MaxValue;
        float dis;
        int count = 0;
        for (int i = 0; i < planetsToHarvest.Count; i++)
        {
            if (!emptyResourcePlanets[i])
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

    public int GetNumAliens()
    {
        int tot = 0;
        for (int i = 0; i < aliensList.Count; i++)
        {
            tot += aliensList[i].Count;
        }
        return tot;
    }


    // STOP HARVESTING FOR ALIENS AND HUMANS

    public void EndHarvesting(GameObject planetToStopHarvesting) // triggered by planets when they reach 0 resources available, interrupts harvesting for that planet 
    {
        // humans stop harvesting
        for (int j = 0; j < selectedPlanetsToHarvestHumanList.Count; j++)
        {
            if (selectedPlanetsToHarvestHumanList[j].IndexOf(planetToStopHarvesting) != -1)  // there is
            {
                int pos = selectedPlanetsToHarvestHumanList[j].IndexOf(planetToStopHarvesting);
                selectedPlanetsToHarvestHumanList[j].RemoveAt(pos);
                Destroy(dottedLinesHarvestingHumanList[j][pos]);
                dottedLinesHarvestingHumanList[j].RemoveAt(pos);
                for (int k = 0; k < humansList.Count; k++)
                {
                    for (int i = 0; i < humansList[k].Count; i++)
                    {
                        if (humansList[k][i].GetComponent<HumanJob>().harvesting && humansList[k][i].GetComponent<HarvestResources>().coorPlanet == planetToStopHarvesting.transform.position)
                        {
                            humansList[k][i].GetComponent<HarvestResources>().PlanetOutOfResources();
                        }
                    }
                }
            }
        }

        // aliens stop harvesting
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
                Invoke("StartAlienHarvesting", 5f);
            }
        }

        emptyResourcePlanets[planetsToHarvest.IndexOf(planetToStopHarvesting)] = true;
    }


    // SLIDERS BUTTON

    public void CloseSliders()
    {
        planetPanel.SetActive(false);
        closeSlidersButton.gameObject.SetActive(false);
        openSlidersButton.gameObject.SetActive(true);
    }

    public void OpenSliders()
    {
        planetPanel.SetActive(true);
        if (selectedHumanPlanet == null) // no planet selected
        {
            planetaryNameTM.text = planetaryName;
            populationTM.text = GetNumHumans().ToString();
            planetPanel.transform.GetChild(3).gameObject.SetActive(false);
        }
        else
        {
            int index = humanPlanets.IndexOf(selectedHumanPlanet);
            planetaryNameTM.text = "Planet " + index;
            populationTM.text = GetNumHumans(index).ToString();
            planetPanel.transform.GetChild(3).gameObject.SetActive(true);
            spawnRateTM.text = "1";
            rateOfFireTM.text = "1";
            speedTM.text = "1";
        }
        closeSlidersButton.gameObject.SetActive(true);
        openSlidersButton.gameObject.SetActive(false);
    }


    // HARVEST SLIDER

    public void AddHarvesters(int num) // function for harvesters slider
    {
        if (selectedHumanPlanet == null) // global slider
        {
            int numHarvesting = GetHarvestingHumans();
            for (int i = 0; i < num - numHarvesting; i++)
            {
                FindFreeHumanForHarvesting();
            }
        }
        else // local slider
        {
            int index = humanPlanets.IndexOf(selectedHumanPlanet);
            int numHarvesting = GetHarvestingHumans(index);
            for (int i = 0; i < num - numHarvesting; i++)
            {
                FindFreeHumanForHarvesting(index);
            }
        }
    }

    public void FindFreeHumanForHarvesting()
    {
        for (int k = 0; k < GetNumHumans(); k++)
        {
            for (int j = 0; j < humansList.Count; j++)
            {
                if (k < humansList[j].Count)
                {
                    if (humansList[j][k].GetComponent<HumanJob>().free && selectedPlanetsToHarvestHumanList[j].Count > 0)
                    {
                        humanHarvesters.Add(humansList[j][k]);
                        humansList[j][k].GetComponent<Movement>().enabled = false;
                        humansList[j][k].GetComponent<HarvestResources>().enabled = true;
                        humansList[j][k].GetComponent<HarvestResources>().SetPlanet(HarvestingPlanetsDistributor(j),
                            new Vector3(humanPlanets[j].transform.position.x + Random.insideUnitCircle.x * (humanPlanets[j].transform.localScale.x / 2f),
                            humanPlanets[j].transform.position.y + Random.insideUnitCircle.y * (humanPlanets[j].transform.localScale.x / 2f), 0));
                        humansList[j][k].GetComponent<HumanJob>().SetIsHarvesting();
                        UpdateSlider();
                        return;
                    }
                    else if (humansList[j][k].GetComponent<HumanJob>().free && selectedPlanetsToHarvestHumanList[j].Count == 0)
                    {
                        humanHarvesters.Add(humansList[j][k]);
                        humansList[j][k].GetComponent<HumanJob>().SetIsHarvesting();
                        UpdateSlider();
                        return;
                    }
                }
            }
        }
    }

    public void FindFreeHumanForHarvesting(int j)
    {
        for (int k = 0; k < humansList[j].Count; k++)
        {
            if (humansList[j][k].GetComponent<HumanJob>().free && selectedPlanetsToHarvestHumanList[j].Count > 0)
            {
                humanHarvesters.Add(humansList[j][k]);
                humansList[j][k].GetComponent<Movement>().enabled = false;
                humansList[j][k].GetComponent<HarvestResources>().enabled = true;
                humansList[j][k].GetComponent<HarvestResources>().SetPlanet(HarvestingPlanetsDistributor(j),
                    new Vector3(humanPlanets[j].transform.position.x + Random.insideUnitCircle.x * (humanPlanets[j].transform.localScale.x / 2f),
                    humanPlanets[j].transform.position.y + Random.insideUnitCircle.y * (humanPlanets[j].transform.localScale.x / 2f), 0));
                humansList[j][k].GetComponent<HumanJob>().SetIsHarvesting();
                UpdateSlider(j);
                return;
            }
            else if (humansList[j][k].GetComponent<HumanJob>().free && selectedPlanetsToHarvestHumanList[j].Count == 0)
            {
                humanHarvesters.Add(humansList[j][k]);
                humansList[j][k].GetComponent<HumanJob>().SetIsHarvesting();
                UpdateSlider(j);
                return;
            }
        }
    }

    public void RemoveHarvesters(int num) // function for harvesters slider
    {
        if (selectedHumanPlanet == null) // global slider
        {
            int numHarvesting = GetHarvestingHumans();
            int len = numHarvesting - num;
            for (int i = 0; i < len; i++)
            {
                FindHarvesterToFree();
            }
        }
        else // local slider
        {
            int index = humanPlanets.IndexOf(selectedHumanPlanet);
            int numHarvesting = GetHarvestingHumans(index);
            int len = numHarvesting - num;
            for (int i = 0; i < len; i++)
            {
                FindHarvesterToFree(index);
            }
        }
    }

    public void FindHarvesterToFree()
    {
        humanHarvesters[humanHarvesters.Count - 1].GetComponent<HumanJob>().SetIsFree();
        humanHarvesters[humanHarvesters.Count - 1].GetComponent<HarvestResources>().stopHarvesting = true;
        humanHarvesters.RemoveAt(humanHarvesters.Count - 1);
        UpdateSlider();
        return;
    }

    public void FindHarvesterToFree(int j)
    {
        for (int i = humansList[j].Count - 1; i >= 0; i--)
        {
            if (humansList[j][i].GetComponent<HumanJob>().harvesting)
            {
                humansList[j][i].GetComponent<HumanJob>().SetIsFree();
                if (humansList[j][i].GetComponent<HarvestResources>().isActiveAndEnabled)
                {
                    humansList[j][i].GetComponent<HarvestResources>().stopHarvesting = true;
                }
                humanHarvesters.Remove(humansList[j][i]);
                UpdateSlider(j);
                return;
            }
        }
    }


    // IN FORMATION SLIDER

    public void AddInFormation(int num) // function for in formation slider
    {
        if (true)
        {
            if (selectedHumanPlanet == null) // global slider
            {
                int numFG = GetInFormationOrGoingOrAttackingHumans();
                for (int i = 0; i < num - numFG; i++)
                {
                    FindFreeHumanForDefending();
                }
            }
            else // local slider
            {
                int index = humanPlanets.IndexOf(selectedHumanPlanet);
                if (selectedPlanetToAttack[index] == null) // go in defence (in formation)
                {
                    int numFG = GetInFormationOrGoingHumans(index);
                    for (int i = 0; i < num - numFG; i++)
                    {
                        FindFreeHumanForDefending(index);
                    }
                }
                else // go to attack
                {
                    int numFGA = GetInFormationOrGoingOrAttackingHumans(index);
                    for (int i = 0; i < num - numFGA; i++)
                    {
                        FindFreeHumanForAttacking(index);
                    }
                }
            }
        }
    }

    public void FindFreeHumanForDefending()
    {
        List<GameObject> entsToPass = new List<GameObject>();
        int index = 0;
        bool found = false;
        for (int k = 0; k < GetNumHumans(); k++)
        {
            for (int j = 0; j < humansList.Count; j++)
            {
                if (k < humansList[j].Count)
                {
                    if (humansList[j][k].GetComponent<HumanJob>().free && selectedPlanetToAttack[j] == null)
                    {
                        humanDefendersOrAttackers.Add(humansList[j][k]);
                        for (int m = 0; m < humansList[j].Count; m++)
                        {
                            if (humansList[j][m].GetComponent<HumanJob>().inFormation || humansList[j][m].GetComponent<HumanJob>().goingIntoFormation)
                            {
                                entsToPass.Add(humansList[j][m]);
                            }
                        }
                        index = j;
                        entsToPass.Add(humansList[j][k]);
                        humansList[j][k].GetComponent<HumanJob>().free = false;
                        humansList[j][k].GetComponent<Movement>().enabled = false;
                        humansList[j][k].GetComponent<HarvestResources>().ResetHarvester();
                        UpdateSlider();
                        found = true;
                        break;
                    }
                    else if (humansList[j][k].GetComponent<HumanJob>().free && selectedPlanetToAttack[j] != null) // actually here goes to attack
                    {
                        humanDefendersOrAttackers.Add(humansList[j][k]);
                        humansList[j][k].GetComponent<Movement>().enabled = false;
                        humansList[j][k].GetComponent<GoToAttackHuman>().enabled = true;
                        humansList[j][k].GetComponent<GoToAttackHuman>().SetTarget(selectedPlanetToAttack[j].transform.position);
                        humansList[j][k].GetComponent<HumanJob>().SetIsAttacking();
                        humansList[j][k].GetComponent<CombatModeHuman>().StartCombatModeHuman();
                        UpdateSlider();
                        found = true;
                        if (selectedPlanetToAttack[j].tag == "AlienPlanet")
                        {
                            if (!aliensInFormation[alienPlanets.IndexOf(selectedPlanetToAttack[j])])
                            {
                                AliensInFormation(alienPlanets.IndexOf(selectedPlanetToAttack[j]));
                            }
                        }
                        break;
                    }
                }
            }
            if (found)
            {
                break;
            }
        }

        Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, humanPlanets[index].transform.position, humanPlanets[index].transform.localScale.x / 2f);
        for (int i = 0; i < entsToPass.Count; i++)
        {
            entsToPass[i].GetComponent<HumanJob>().SetIsGoingIntoFormation();
            entsToPass[i].GetComponent<CombatModeHuman>().StartCombatModeHuman();
            entsToPass[i].GetComponent<GoInFormationHuman>().enabled = true;
            entsToPass[i].GetComponent<GoInFormationHuman>().coords = humansTargetPos[i];
        }
    }

    public void FindFreeHumanForDefending(int j)
    {
        List<GameObject> entsToPass = new List<GameObject>();
        int index = 0;
        for (int k = 0; k < humansList[j].Count; k++)
        {
            if (humansList[j][k].GetComponent<HumanJob>().free)
            {
                humanDefendersOrAttackers.Add(humansList[j][k]);
                for (int m = 0; m < humansList[j].Count; m++)
                {
                    if (humansList[j][m].GetComponent<HumanJob>().inFormation || humansList[j][m].GetComponent<HumanJob>().goingIntoFormation)
                    {
                        entsToPass.Add(humansList[j][m]);
                    }
                }
                index = j;
                entsToPass.Add(humansList[j][k]);
                humansList[j][k].GetComponent<HumanJob>().free = false;
                humansList[j][k].GetComponent<Movement>().enabled = false;
                humansList[j][k].GetComponent<HarvestResources>().ResetHarvester();
                UpdateSlider(j);
                break;
            }
        }

        Vector2[] humansTargetPos = circleFormation.CalculateCircleFormation(entsToPass, humanPlanets[index].transform.position, humanPlanets[index].transform.localScale.x / 2f);
        for (int i = 0; i < entsToPass.Count; i++)
        {
            entsToPass[i].GetComponent<HumanJob>().SetIsGoingIntoFormation();
            entsToPass[i].GetComponent<CombatModeHuman>().StartCombatModeHuman();
            entsToPass[i].GetComponent<GoInFormationHuman>().enabled = true;
            entsToPass[i].GetComponent<GoInFormationHuman>().coords = humansTargetPos[i];
        }
    }

    public void FindFreeHumanForAttacking(int j)
    {
        for (int k = 0; k < humansList[j].Count; k++)
        {
            if (humansList[j][k].GetComponent<HumanJob>().free && selectedPlanetToAttack[j] != null)
            {
                humanDefendersOrAttackers.Add(humansList[j][k]);
                humansList[j][k].GetComponent<Movement>().enabled = false;
                humansList[j][k].GetComponent<GoToAttackHuman>().enabled = true;
                humansList[j][k].GetComponent<GoToAttackHuman>().SetTarget(selectedPlanetToAttack[j].transform.position);
                humansList[j][k].GetComponent<HumanJob>().SetIsAttacking();
                humansList[j][k].GetComponent<CombatModeHuman>().StartCombatModeHuman();
                UpdateSlider(j);
                if (selectedPlanetToAttack[j].tag == "AlienPlanet")
                {
                    if (!aliensInFormation[alienPlanets.IndexOf(selectedPlanetToAttack[j])])
                    {
                        AliensInFormation(alienPlanets.IndexOf(selectedPlanetToAttack[j]));
                    }
                }
                return;
            }
        }
    }

    public void RemoveInFormation(int num) // function for in formation slider
    {
        if (selectedHumanPlanet == null) // global slider
        {
            int numInFormationOrGoing = GetInFormationOrGoingOrAttackingHumans();
            int len = numInFormationOrGoing - num;
            for (int i = 0; i < len; i++)
            {
                FindInFormationOrAttackerToFree();
            }
        }
        else // local slider
        {
            int index = humanPlanets.IndexOf(selectedHumanPlanet);
            int numInFormationOrGoing = GetInFormationOrGoingOrAttackingHumans(index);
            int len = numInFormationOrGoing - num;
            for (int i = 0; i < len; i++)
            {
                FindInFormationOrAttackerToFree(index);
            }
        }
    }

    public void FindInFormationOrAttackerToFree()
    {
        humanDefendersOrAttackers[humanDefendersOrAttackers.Count - 1].GetComponent<GoInFormationHuman>().enabled = false;
        humanDefendersOrAttackers[humanDefendersOrAttackers.Count - 1].GetComponent<GoToAttackHuman>().enabled = false;
        humanDefendersOrAttackers[humanDefendersOrAttackers.Count - 1].GetComponent<CombatModeHuman>().StopCombatModeHuman();
        humanDefendersOrAttackers[humanDefendersOrAttackers.Count - 1].GetComponent<Movement>().enabled = true;
        humanDefendersOrAttackers[humanDefendersOrAttackers.Count - 1].GetComponent<HumanJob>().SetIsFree();
        humanDefendersOrAttackers.RemoveAt(humanDefendersOrAttackers.Count - 1);
        RecalculateFormation();
        UpdateSlider();
        for (int j = 0; j < humansList.Count; j++)
        {
            if (GetNumHumans(j) == GetFreeHumans(j) && selectedPlanetToAttack[j] != null && selectedPlanetToAttack[j].tag == "AlienPlanet")
            {
                int count = 0;
                for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                {
                    if (selectedPlanetToAttack[i] == selectedPlanetToAttack[j])
                    {
                        count++;
                    }
                }
                if (count == 1) // check if there is only one human planet attacking the alien planet, if so interrupt conquest
                {
                    AliensFree(alienPlanets.IndexOf(selectedPlanetToAttack[j]));
                    for (int m = 0; m < aliensList.Count; m++)
                    {
                        if (aliensAttacking[m])
                        {
                            for (int n = 0; n < aliensList[m].Count; n++)
                            {
                                if (aliensList[m][n].GetComponent<GoToAttackAlien>().target == selectedPlanetToAttack[j].transform.position)
                                {
                                    AlienRetreat();
                                    break;
                                }
                            }
                        }
                    }
                    conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[j])] = false;
                }
                Destroy(dottedLinesAttackingHumans[j]);
                dottedLinesAttackingHumans[j] = null;
                selectedPlanetToAttack[j] = null;
            }
            else if (GetNumHumans(j) == GetFreeHumans(j) && selectedPlanetToAttack[j] != null && selectedPlanetToAttack[j].tag == "EmptyResourcePlanet")
            {
                int countH = 0;
                int countA = 0;
                for (int i = 0; i < selectedPlanetToAttack.Count; i++)
                {
                    if (selectedPlanetToAttack[i] == selectedPlanetToAttack[j])
                    {
                        countH++;
                    }
                }
                for (int m = 0; m < aliensList.Count; m++)
                {
                    for (int k = 0; k < aliensList[m].Count; k++)
                    {
                        if (aliensList[m][k].GetComponent<AlienJob>().attacking && aliensList[m][k].GetComponent<GoToAttackAlien>().target == selectedPlanetToAttack[j].gameObject.transform.position)
                        {
                            countA++;
                            break;
                        }
                    }
                }
                if (countH == 1 && countA == 0) // check if there is only one human planet and no alien planets attacking the resource planet, if so interrupt conquest
                {
                    conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[j])] = false;
                }

                Destroy(dottedLinesAttackingHumans[j]);
                dottedLinesAttackingHumans[j] = null;
                selectedPlanetToAttack[j] = null;
            }
        }
        return;
    }

    public void FindInFormationOrAttackerToFree(int j)
    {
        for (int i = humansList[j].Count - 1; i >= 0; i--)
        {
            if (humansList[j][i].GetComponent<HumanJob>().attacking || humansList[j][i].GetComponent<HumanJob>().goingIntoFormation || humansList[j][i].GetComponent<HumanJob>().inFormation)
            {
                humansList[j][i].GetComponent<GoInFormationHuman>().enabled = false;
                humansList[j][i].GetComponent<GoToAttackHuman>().enabled = false;
                humansList[j][i].GetComponent<CombatModeHuman>().StopCombatModeHuman();
                humansList[j][i].GetComponent<Movement>().enabled = true;
                humansList[j][i].GetComponent<HumanJob>().SetIsFree();
                humanDefendersOrAttackers.Remove(humansList[j][i]);
                RecalculateFormation();
                UpdateSlider(j);
                if (GetNumHumans(j) == GetFreeHumans(j) && selectedPlanetToAttack[j] != null && selectedPlanetToAttack[j].tag == "AlienPlanet")
                {
                    int count = 0;
                    for (int k = 0; k < selectedPlanetToAttack.Count; k++)
                    {
                        if (selectedPlanetToAttack[k] == selectedPlanetToAttack[j])
                        {
                            count++;
                        }
                    }
                    if (count == 1) // check if there is only one human planet attacking the alien planet, if so interrupt conquest
                    {
                        AliensFree(alienPlanets.IndexOf(selectedPlanetToAttack[j]));
                        for (int m = 0; m < aliensList.Count; m++)
                        {
                            if (aliensAttacking[m])
                            {
                                for (int n = 0; n < aliensList[m].Count; n++)
                                {
                                    if (aliensList[m][n].GetComponent<GoToAttackAlien>().target == selectedPlanetToAttack[j].transform.position)
                                    {
                                        AlienRetreat();
                                        break;
                                    }
                                }
                            }
                        }
                        conqueringAlienPlanets[alienPlanets.IndexOf(selectedPlanetToAttack[j])] = false;
                    }
                    Destroy(dottedLinesAttackingHumans[j]);
                    dottedLinesAttackingHumans[j] = null;
                    selectedPlanetToAttack[j] = null;
                }
                else if (GetNumHumans(j) == GetFreeHumans(j) && selectedPlanetToAttack[j] != null && selectedPlanetToAttack[j].tag == "EmptyResourcePlanet")
                {
                    int countH = 0;
                    int countA = 0;
                    for (int h = 0; h < selectedPlanetToAttack.Count; h++)
                    {
                        if (selectedPlanetToAttack[h] == selectedPlanetToAttack[j])
                        {
                            countH++;
                        }
                    }
                    for (int m = 0; m < aliensList.Count; m++)
                    {
                        for (int k = 0; k < aliensList[m].Count; k++)
                        {
                            if (aliensList[m][k].GetComponent<AlienJob>().attacking && aliensList[m][k].GetComponent<GoToAttackAlien>().target == selectedPlanetToAttack[j].gameObject.transform.position)
                            {
                                countA++;
                                break;
                            }
                        }
                    }
                    if (countH == 1 && countA == 0) // check if there is only one human planet and no alien planets attacking the resource planet, if so interrupt conquest
                    {
                        conqueringResourcePlanets[planetsToHarvest.IndexOf(selectedPlanetToAttack[j])] = false;
                    }

                    Destroy(dottedLinesAttackingHumans[j]);
                    dottedLinesAttackingHumans[j] = null;
                    selectedPlanetToAttack[j] = null;
                }
                return;
            }
        }
    }


    // GET FUNCTIONS FOR GLOBAL SLIDER

    public int GetNumHumans()
    {
        int tot = 0;
        for (int i = 0; i < humansList.Count; i++)
        {
            tot += humansList[i].Count;
        }
        return tot;
    }

    public int GetFreeHumans()
    {
        int numFree = 0;
        for (int j = 0; j < humansList.Count; j++)
        {
            for (int i = 0; i < humansList[j].Count; i++)
            {
                if (humansList[j][i].GetComponent<HumanJob>().free)
                    numFree++;
            }
        }
        return numFree;
    }

    public int GetHarvestingHumans()
    {
        int numHarvestingHumans = 0;
        for (int j = 0; j < humansList.Count; j++)
        {
            for (int i = 0; i < humansList[j].Count; i++)
            {
                if (humansList[j][i].GetComponent<HumanJob>().harvesting)
                    numHarvestingHumans++;
            }
        }
        return numHarvestingHumans;
    }

    public int GetInFormationOrGoingHumans()
    {
        int numInFormationOrGoing = 0;
        for (int j = 0; j < humansList.Count; j++)
        {
            for (int i = 0; i < humansList[j].Count; i++)
            {
                if (humansList[j][i].GetComponent<HumanJob>().inFormation || humansList[j][i].GetComponent<HumanJob>().goingIntoFormation)
                    numInFormationOrGoing++;
            }
        }
        return numInFormationOrGoing;
    }

    public int GetInFormationOrGoingOrAttackingHumans()
    {
        int numInFormationOrGoingOrAttacking = 0;
        for (int j = 0; j < humansList.Count; j++)
        {
            for (int i = 0; i < humansList[j].Count; i++)
            {
                if (humansList[j][i].GetComponent<HumanJob>().inFormation || humansList[j][i].GetComponent<HumanJob>().goingIntoFormation
                    || humansList[j][i].GetComponent<HumanJob>().attacking)
                    numInFormationOrGoingOrAttacking++;
            }
        }
        return numInFormationOrGoingOrAttacking;
    }


    // GET FUNCTIONS FOR LOCAL SLIDER

    public int GetNumHumans(int j)
    {
        return humansList[j].Count;
    }

    public int GetFreeHumans(int j)
    {
        int numFree = 0;
        for (int i = 0; i < humansList[j].Count; i++)
        {
            if (humansList[j][i].GetComponent<HumanJob>().free)
                numFree++;
        }
        return numFree;
    }

    public int GetHarvestingHumans(int j)
    {
        int numHarvestingHumans = 0;
        for (int i = 0; i < humansList[j].Count; i++)
        {
            if (humansList[j][i].GetComponent<HumanJob>().harvesting)
                numHarvestingHumans++;
        }
        return numHarvestingHumans;
    }

    public int GetInFormationOrGoingHumans(int j)
    {
        int numInFormationOrGoing = 0;
        for (int i = 0; i < humansList[j].Count; i++)
        {
            if (humansList[j][i].GetComponent<HumanJob>().inFormation || humansList[j][i].GetComponent<HumanJob>().goingIntoFormation)
                numInFormationOrGoing++;
        }
        return numInFormationOrGoing;
    }

    public int GetInFormationOrGoingOrAttackingHumans(int j)
    {
        int numInFormationOrGoingOrAttacking = 0;
        for (int i = 0; i < humansList[j].Count; i++)
        {
            if (humansList[j][i].GetComponent<HumanJob>().inFormation || humansList[j][i].GetComponent<HumanJob>().goingIntoFormation
                || humansList[j][i].GetComponent<HumanJob>().attacking)
                numInFormationOrGoingOrAttacking++;
        }
        return numInFormationOrGoingOrAttacking;
    }


    // UPDATE SLIDERS

    public void UpdateSlider()
    {
        slider.value = GetFreeHumans();
    }

    public void UpdateHarvestingSlider()
    {
        harvestingSlider.value = GetHarvestingHumans();
    }

    public void UpdateInFormationSlider()
    {
        inFormationSlider.value = GetInFormationOrGoingOrAttackingHumans();
    }


    // UPDATE LOCAL SLIDERS

    public void UpdateSlider(int j)
    {
        slider.maxValue = GetNumHumans(j);
        slider.value = GetFreeHumans(j);
    }

    public void UpdateHarvestingSlider(int j)
    {
        harvestingSlider.maxValue = GetNumHumans(j);
        harvestingSlider.value = GetHarvestingHumans(j);
    }

    public void UpdateInFormationSlider(int j)
    {
        inFormationSlider.maxValue = GetNumHumans(j);
        inFormationSlider.value = GetInFormationOrGoingOrAttackingHumans(j);
    }


    // ADD RESOURCES TO SLIDERS

    public void AddMineralResource()
    {
        mineralResourcesSlider.value++;
    }

    public void AddIceResource()
    {
        iceResourcesSlider.value++;
    }

    public void AddGasResource()
    {
        gasResourcesSlider.value++;
    }


    private int FindNextSquare(int sq) // purely mathematical function for planet sizes
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
}
