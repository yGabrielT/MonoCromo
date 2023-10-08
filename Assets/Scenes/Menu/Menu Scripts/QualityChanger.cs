using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualityChanger : MonoBehaviour
{
    public static int qualidadeSelecionada;
    [SerializeField] private TMP_Dropdown dropdown;

    private void Update()
    {
        dropdown.value = qualidadeSelecionada;
        DropdownSample(dropdown);
    }

    public static void DropdownSample(TMP_Dropdown dropdown)
    {
        switch (dropdown.value)
        {
            case 0: QualitySettings.SetQualityLevel(4); Debug.Log("qualidade alta"); qualidadeSelecionada = 0; break;
            case 1: QualitySettings.SetQualityLevel(2); Debug.Log("qualidade media"); qualidadeSelecionada = 1; break;
            case 2: QualitySettings.SetQualityLevel(0); Debug.Log("qualidade baixa"); qualidadeSelecionada = 2; break;
        }
    }
}
