using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Enemy/EnemyType")]
public class EnemyTypeSO : ScriptableObject
{
    public int health;
    public Sprite sprite;
    public RuntimeAnimatorController animator;
    public int coins;
    public EnemyDifficulty enemyDifficulty;
}

public enum EnemyDifficulty { Easy, Medium, Hard }
