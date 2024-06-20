using System;
using UnityEngine;

namespace Hero
{
[RequireComponent(typeof(HeroHealth))]
public class HeroDeath : MonoBehaviour
{
    public HeroHealth heroHealth;
    public HeroMove heroMove;
    public HeroAttack heroAttack;
    public HeroAnimator heroAnimator;

    public GameObject deathFx;
    private bool _isDead;

    private void Start()
    {
        heroHealth.HealthChanged += HealthChanged;
    }

    private void OnDestroy()
    {
        heroHealth.HealthChanged -= HealthChanged;
    }

    private void HealthChanged()
    {
        if (!_isDead && heroHealth.Current <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;
        heroMove.enabled = false;
        heroAttack.enabled = false;
        heroAnimator.PlayDeath();

        Instantiate(deathFx, transform.position, Quaternion.identity);
    }
}
}