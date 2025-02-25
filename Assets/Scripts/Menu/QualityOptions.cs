using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QualityOptions : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int quality;
    void Start()
    {
        quality = PlayerPrefs.GetInt("numeroDeCalidad", 2);
        dropdown.value = quality;
        AdjustQuality();
    }

    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("numeroDeCalidad", dropdown.value);
        quality = dropdown.value;
    }
}