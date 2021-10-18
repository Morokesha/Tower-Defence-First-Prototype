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
        // Если нажали левую кнопку мыши и нажали не на ЮАЙ элемент
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (towerTypeActive != null)
            {
                //точка в которую будет установка башни
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
        // проверяем лучом что находится под нами
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(mousePosition, Vector3.forward, 25f, towerMask);
        // переменная, которая показывает установлена ли сейчас башня на выбранном месте
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
        // вычитаем из текущих денег стоимость башни
        int currentMoney = GameManager.SETTINGS_DATA.Money - towerCost;

        // если остаток денег больше или равен 0
        if (currentMoney >= 0)
        {
            // то делаем покупку и вычитаем деньги
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
