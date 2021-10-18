using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    private int currentWave;

    private void Start()
    {
        GameManager.Instance.OnWaveChanged += UpdateCurrentWave;
        UpdateCurrentWave();
    }

    private void UpdateCurrentWave()
    {
        string formatedString = $"{GameManager.SETTINGS_DATA.WaveIndex}/{GameManager.SETTINGS_DATA.WaveInLevel}";
        waveText.SetText("Current Wave : " + formatedString);
    }
}
