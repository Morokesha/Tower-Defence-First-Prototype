using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance { get; private set; }

    public event Action<TowerTypeSO> OnSelectedTowerType;

    [SerializeField] private Transform pfTower;
    [SerializeField] private LayerMask towerMask;
    [SerializeField] private TowerTypeSO towerTypeActive;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // ���� ������ ����� ������ ���� � ������ �� �� ��� �������
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (towerTypeActive != null)
            {
                //����� � ������� ����� ��������� �����
                Vector2 spawnPoint;
                if (CanBuildTower(UtillClass.GetMouseWorldPosition(),out spawnPoint))
                {
                    if (CanBuyTower())
                    {
                        BuildingTowerPosition(spawnPoint);
                    }
                }
            }
        }
    }

    public void SetActiveTowerType(TowerTypeSO towerType)
    {
        towerTypeActive = towerType;
        OnSelectedTowerType?.Invoke(towerType);
    }

        private bool CanBuildTower(Vector3 mousePosition, out Vector2 centerPoint)
    {
        // ��������� ����� ��� ��������� ��� ����
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(mousePosition, Vector3.forward, 25f, towerMask);
        // ����������, ������� ���������� ����������� �� ������ ����� �� ��������� �����
        bool hasTower = false;
        bool isTowerSide = false;
        Vector2 placePoint = Vector2.zero;

        foreach (var info in raycastHits)
        {
            if (info.collider != null)
            {
                Tower tower =info.collider.GetComponent<Tower>();
                if (tower != null)
                {
                    hasTower = true;
                }

                if (info.collider.CompareTag("TowerSide"))
                {
                    isTowerSide = true;
                    placePoint = info.collider.bounds.center;
                }
            }
        }

            if (isTowerSide && hasTower == false)
            {
                centerPoint = placePoint;
                return true;
            }

            centerPoint = placePoint;
            return false;

    }

    private bool CanBuyTower()
    {
        int towerCost = towerTypeActive.cost;
        // �������� �� ������� ����� ��������� �����
        int currentMoney = GameManager.SETTINGS_DATA.Money - towerCost;

        // ���� ������� ����� ������ ��� ����� 0
        if (currentMoney >= 0)
        {
            // �� ������ ������� � �������� ������
            GameManager.Instance.TakeMoney(towerCost);
            return true;
        }

        return false;
    }

    private void BuildingTowerPosition(Vector2 mousePosition)
    {
        Tower.Create(mousePosition, towerTypeActive, pfTower);

        SetActiveTowerType(null);
    }
}
