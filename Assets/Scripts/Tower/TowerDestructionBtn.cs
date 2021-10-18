using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerDestructionBtn : MonoBehaviour
{
    [SerializeField] private Tower tower;
    [SerializeField] private Button destructionBtn;

    private void Awake()
    {
        destructionBtn.onClick.AddListener(() => {
            // берем часть денег за постройку башни
            int remainder = Mathf.RoundToInt(tower.towerData.cost / 2);
            // и присваиваем обратно на баланс
            GameManager.Instance.AddMoney(remainder);

            Destroy(tower.gameObject);
        });
    }
}
