using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
[RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
public class EnemyDeath : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public EnemyAnimator enemyAnimator;

    public GameObject deathFx;
    public event Action Happened;

    private void Start()
    {
        enemyHealth.HealthChanged += HealthChanged;
    }

    private void OnDestroy()
    {
        enemyHealth.HealthChanged -= HealthChanged;

    }

    private void HealthChanged()
    {
        if (enemyHealth.Current <=0)
        {
            Die();
        }
    }

    private void Die()
    {
        enemyHealth.HealthChanged += HealthChanged;

        enemyAnimator.PlayDeath();
        SpawnDeathFx();
        StartCoroutine(DestroyTimer());
        
        Happened?.Invoke();
    }

    private GameObject SpawnDeathFx() => Instantiate(deathFx, transform.position, Quaternion.identity);

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
}