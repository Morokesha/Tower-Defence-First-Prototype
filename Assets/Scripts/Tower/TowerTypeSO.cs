using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/TowerType")]
public class TowerTypeSO : ScriptableObject
{
    // ���� �����
    public int damage;
    // ���� �����
    public int cost;
    // ������ �����
    public int attackRadius;
    // ���������������� �����
    public float attackSpeed;
    // ������ �����
    public Sprite sprite;
    // ������ �����
    public GameObject projectile;
}
