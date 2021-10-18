using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/TowerType")]
public class TowerTypeSO : ScriptableObject
{
    // урон башни
    public int damage;
    // цена башни
    public int cost;
    // радиус атаки
    public int attackRadius;
    // скорострельность башни
    public float attackSpeed;
    // спрайт башни
    public Sprite sprite;
    // снаряд башни
    public GameObject projectile;
}
