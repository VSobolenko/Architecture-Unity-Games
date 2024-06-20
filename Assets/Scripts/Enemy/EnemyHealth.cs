using System;
using UnityEngine;

namespace Enemy
{
[RequireComponent(typeof(EnemyAnimator))]
public class EnemyHealth : MonoBehaviour, IHealth
{
    public EnemyAnimator enemyAnimator;
    [SerializeField] private float _current;
    [SerializeField] private float _max;

    public event Action HealthChanged;

    public float Current
    {
        get => _current;
        set => _current = value;
    }

    public float Max
    {
        get => _max;
        set => _max = value;
    }

    public void TakeDamage(float damage)
    {
        Current -= damage;
        
        enemyAnimator.PlayHit();
        HealthChanged?.Invoke();
    }
}
}