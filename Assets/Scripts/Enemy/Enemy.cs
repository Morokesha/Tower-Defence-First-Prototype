using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(EnemyTypeSO enemyType, Transform enemyPf, List<Transform> waypoints, Vector3 startPosition)
    {
        Transform enemyTransform = Instantiate(enemyPf, startPosition, Quaternion.identity);
        Enemy currentEnemy = enemyTransform.GetComponent<Enemy>();
        currentEnemy.SetData(enemyType, waypoints);

        return currentEnemy;
    }

    public event Action<int> OnEnemyDied;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator enemyAnimator;

    private List<Transform> waypoints;
    private EnemyTypeSO enemyData;

    private float speed = 2.2f;
    private int health;
    private int currentHealth; 
    private int indexPoint = 0;
    private bool isDead = false;

    

    private void Update()
    {
        if (isDead == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[indexPoint].position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Waypoint"))
        {
            indexPoint++;
        }
    }

    private void SetData(EnemyTypeSO enemyData, List<Transform> waypoints)
    {
        this.enemyData = enemyData;
        this.waypoints = waypoints;

        spriteRenderer.sprite = enemyData.sprite;
        enemyAnimator.runtimeAnimatorController = enemyData.animator;
        health = enemyData.health;
        currentHealth = enemyData.health;
    }

    public void ReachedToEndPoint()
    {
        OnEnemyDied?.Invoke(0);
        Destroy(gameObject);
    }
    
    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;
        //границы здоровья, хп не ниже нуля, в заданном диапазоне
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        StartCoroutine(EnemyHurtCo());
        if (IsDead())
        {
            OnEnemyDied?.Invoke(enemyData.coins);
            StartCoroutine(EnemyGoingToDie());
        }
    }

    private IEnumerator EnemyHurtCo()
    {
        enemyAnimator.SetTrigger("Hurt");
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator EnemyGoingToDie()
    {
        isDead = true;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        //сортируем мертвых ниже живых
        spriteRenderer.sortingOrder = 0;
        enemyAnimator.SetTrigger("Die");

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private bool IsDead(){
        return currentHealth == 0;
        }
}
