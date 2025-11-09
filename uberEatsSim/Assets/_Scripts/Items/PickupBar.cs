using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PickupBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        this.gameObject.SetActive(false);
    }

    public void SetPickupBar(int amount)
    {
        slider.value = amount;
    }
}
