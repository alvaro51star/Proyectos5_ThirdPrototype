using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerUpdating : MonoBehaviour
{
    [SerializeField] private Image slider;

    private void OnEnable()
    {
        GameManager.OnTimeChange += ChangeSlider;
    }

    private void OnDisable()
    {
        GameManager.OnTimeChange -= ChangeSlider;
    }

    private void ChangeSlider(float time)
    {
        slider.fillAmount = time / GameManager.instance.maxTimeLevel;
    }
}
