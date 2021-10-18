using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public static Tower Create(Vector2 position, TowerTypeSO towerType, Transform pfTower)
    {
        //создание башни
        Transform towerPosition = Instantiate(pfTower, position, Quaternion.identity);
        //указываем этот же скрипт
        Tower currentTower = towerPosition.GetComponent<Tower>();
        currentTower.SetSpriteTower(towerType);
        return currentTower;
    }

    [HideInInspector] public TowerTypeSO towerData;
    [SerializeField] private LayerMask enemyLayer;

    private SpriteRenderer spriteRenderer;
    private Enemy targetEnemy;
    private float timerShot = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(HandleTargetCO());
    }

    private void Update()
    {
        HandleShot();
    }

    private void SetSpriteTower(TowerTypeSO towerData)
    {
        this.towerData = towerData;

        GetComponent<SpriteRenderer>().sprite = towerData.sprite;
    }

    private void HandleShot()
    {
        timerShot += Time.deltaTime;
        if (timerShot >= towerData.attackSpeed)
        {
            if (targetEnemy != null)
            {
                Projectile.Create(towerData.projectile, transform.position, targetEnemy, towerData.damage);
                timerShot = 0f;
            }
        }
    }

    private IEnumerator HandleTargetCO()
    {
        while (true)
        {
            SetNearbyTarget();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void SetNearbyTarget()
    {
        //нужно получить коллайдеры ближайших врагов
        Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, towerData.attackRadius, enemyLayer);
        Enemy nearbyTarget = null;

        foreach (var Collider2D in targetColliders)
        {
            Enemy enemy = Collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (nearbyTarget == null)
                {
                    nearbyTarget = enemy;
                }
                else
                {
                    float distanceBetweenEnemy = (enemy.transform.position - transform.position).sqrMagnitude;
                    float distanceBetweenNearbyEnemy = (nearbyTarget.transform.position - transform.position).sqrMagnitude;

                    // если дистанция текущего врага, меньше чем уже установленного ближайшим врагов
                    if (distanceBetweenEnemy < distanceBetweenNearbyEnemy)
                    {
                        // то ближайшего врага необходимо обновить
                        nearbyTarget = enemy;
                    }
                }
            }
        }

        targetEnemy = nearbyTarget;
    }

    private void OnDrawGizmosSelected()
    {
        if (towerData == null)
        {
            Gizmos.DrawWireSphere(transform.position, 5f);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, towerData.attackRadius);
        }
    }
}
