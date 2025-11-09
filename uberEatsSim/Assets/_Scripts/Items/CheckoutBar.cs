using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckoutBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        this.gameObject.SetActive(false);
    }

    public void SetCheckoutBar(int amount)
    {
        slider.value = amount;
    }
}
