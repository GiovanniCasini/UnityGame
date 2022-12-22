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

    void Start()
    {
        slider.onValueChanged.AddListener(UpdateText);
    }

    public void UpdateText(float val)
    {
        if (slider.value + FindObjectOfType<HarvestingSlider>().lastValue > FindObjectOfType<Manager>().GetNumHumans()
            || (FindObjectOfType<Manager>().GetFreeHumans() == 0 && slider.value >= lastValue)
            || slider.value - FindObjectOfType<Manager>().GetInFormationOrGoingOrAttackingHumans() > FindObjectOfType<Manager>().GetFreeHumans())
        {
            slider.value = lastValue;
        }
        else
        {
            totalHumans.text = slider.value.ToString();
            if (slider.value > lastValue)
            {
                FindObjectOfType<Manager>().AddInFormation((int)slider.value);
            }
            if (slider.value < lastValue)
            {
                FindObjectOfType<Manager>().RemoveInFormation((int)slider.value);
            }
            lastValue = (int)slider.value;
        }
    }
}
