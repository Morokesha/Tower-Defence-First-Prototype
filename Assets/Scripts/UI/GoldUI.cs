using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    private int moneyCount;
    private void Start()
    {
        GameManager.Instance.OnMoneyChanged += UpdateMoneyText;
        moneyCount = GameManager.SETTINGS_DATA.Money;

        goldText.SetText("Gold :" + moneyCount.ToString());
    }

    private void UpdateMoneyText(int nextValue)
    {
        goldText.SetText("Gold :" + nextValue.ToString());
    }

}
