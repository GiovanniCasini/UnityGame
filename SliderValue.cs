using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValue : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI total;

    void Start()
    {
        //slider.maxValue = FindObjectOfType<Manager>().getCountEntities();
        //UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    public void UpdateText(float val)
    {
        total.text = slider.value.ToString();
    }
}
