using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InFormationSlider : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI totalHumans;
    public int lastValue = 0;
    public int lastValueLocal = 0;
    public Manager manager;

    void Start()
    {
        manager = FindObjectOfType<Manager>();
        slider.onValueChanged.AddListener(UpdateText);
    }

    public void UpdateText(float val)
    {
        if (manager.selectedHumanPlanet == null) // global
        {
            if (!manager.updatingGlobalSlider)
            {
                if (slider.value + manager.GetHarvestingHumans() > manager.GetNumHumans()
                    || (manager.GetFreeHumans() == 0 && slider.value >= lastValue)
                    || slider.value - manager.GetInFormationOrGoingOrAttackingHumans() > manager.GetFreeHumans())
                {
                    slider.value = lastValue;
                }
                else
                {
                    totalHumans.text = slider.value.ToString();
                    if (slider.value > lastValue)
                    {
                        manager.AddInFormation((int)slider.value);
                    }
                    if (slider.value < lastValue)
                    {
                        manager.RemoveInFormation((int)slider.value);
                    }
                    lastValue = (int)slider.value;
                }
            }
            else
            {
                lastValue = manager.GetInFormationOrGoingOrAttackingHumans();
                totalHumans.text = slider.value.ToString();
            }
        }
        else // local
        {
            int index = manager.humanPlanets.IndexOf(manager.selectedHumanPlanet);
            if (!manager.updatingLocalSlider)
            {
                if (slider.value + manager.GetHarvestingHumans(index) > manager.GetNumHumans(index)
                    || (manager.GetFreeHumans(index) == 0 && slider.value >= lastValueLocal)
                    || slider.value - manager.GetInFormationOrGoingOrAttackingHumans(index) > manager.GetFreeHumans(index))
                {
                    slider.value = lastValueLocal;
                }
                else
                {
                    totalHumans.text = slider.value.ToString();
                    if (slider.value > lastValueLocal)
                    {
                        manager.AddInFormation((int)slider.value);
                    }
                    if (slider.value < lastValueLocal)
                    {
                        manager.RemoveInFormation((int)slider.value);
                    }
                    lastValueLocal = (int)slider.value;
                }
            }
            else
            {
                lastValueLocal = manager.GetInFormationOrGoingOrAttackingHumans(index);
                totalHumans.text = slider.value.ToString();
            }
        }
    }
}
