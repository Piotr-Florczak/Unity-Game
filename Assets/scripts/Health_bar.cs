using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health_bar : MonoBehaviour
{

    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }


    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHeath (int health)
    {
        slider.value = health;
    }
}