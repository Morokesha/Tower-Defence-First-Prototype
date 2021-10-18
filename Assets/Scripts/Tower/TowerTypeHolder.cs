using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower/Tower List")]
public class TowerTypeHolder : ScriptableObject
{
    public List<TowerTypeSO> towerTypeList;
}
