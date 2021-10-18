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
            // ����� ����� ����� �� ��������� �����
            int remainder = Mathf.RoundToInt(tower.towerData.cost / 2);
            // � ����������� ������� �� ������
            GameManager.Instance.AddMoney(remainder);

            Destroy(tower.gameObject);
        });
    }
}
