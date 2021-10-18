using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    [SerializeField] private TowerTypeHolder towerList;
    //словарь по типу башен с их трансформом.
    private Dictionary<TowerTypeSO, Transform> btnDictionaryTower;

    private void Awake()
    {
        btnDictionaryTower = new Dictionary<TowerTypeSO, Transform>();
       
    }

    private void Start()
    {
        MakeButtonTower();
        ConstructionManager.Instance.OnSelectedTowerType += UpdateActiveButton;
        UpdateActiveButton(null);
    }

    private void MakeButtonTower()
    {
        Transform sampleButton = transform.Find("TowerBtn");
        sampleButton.gameObject.SetActive(false);

        int index = 0;

        foreach (var towerType in towerList.towerTypeList)
        {
            Transform btnTransform = Instantiate(sampleButton, transform);
            btnTransform.gameObject.SetActive(true);

            btnTransform.Find("Image").GetComponent<Image>().sprite = towerType.sprite;
            btnTransform.Find("text").GetComponent<TextMeshProUGUI>().text = towerType.cost.ToString();

            //позиционирование кнопок ,создаем из скритпа на основании словаря столько кнопок сколько итемов в словаре
            float offsetamount = 80 * index;

            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetamount + 50f, 50f);

            // на кнопку вешаем слушателя (функцию), которая будет слушать клик по кнопке
            // и если мы на нее нажали, устанавливаем тип здания как активный
            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                ConstructionManager.Instance.SetActiveTowerType(towerType);
            });

            btnDictionaryTower[towerType] = btnTransform;
            index++;
        }
    }

    private void UpdateActiveButton(TowerTypeSO towerType)
    {
        foreach (var buildingType in btnDictionaryTower.Keys)
        {
            Transform btnTransform = btnDictionaryTower[buildingType];
            btnTransform.Find("Selected").gameObject.SetActive(false);
        }

        if (towerType != null)
        {
            // включаем selected только у выбранной башни
            btnDictionaryTower[towerType].Find("Selected").gameObject.SetActive(true);
        }
    }

}
