using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static Projectile Create (GameObject pfProjectile,Vector3 position, Enemy enemy, int damage)
    {
        GameObject projectile = Instantiate(pfProjectile, position, Quaternion.identity);
        Projectile arrowProjectile = projectile.transform.GetComponent<Projectile>();
        arrowProjectile.SetData(enemy, damage);

        return arrowProjectile;
    }

    [SerializeField] private float speedProjectile = 10.0f;
    private Vector3 lastMoveDir;
    private Enemy targetEnemy;
    private int damage;
    //Время жизни снаряда
    private float timeToDie = 2.0f;

    private void Update()
    {
        Vector3 moveDir;

        if (targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            moveDir = lastMoveDir;
        }

        transform.position += moveDir * speedProjectile * Time.deltaTime;
        // задаю угол вращения стрел относительно их направления полета
        transform.eulerAngles = new Vector3(0, 0, UtillClass.GetAngleFromVector(moveDir));

        //Уничтожение стрел по прошествию заданного времени
        timeToDie -= Time.deltaTime;
        if (timeToDie <=0)
        {
            Destroy(gameObject);
        }
    }

    private void SetData(Enemy targetEnemy, int damage)
    {
        this.targetEnemy = targetEnemy;
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.Damage(damage);
            Destroy(gameObject);
        }
    }
}
