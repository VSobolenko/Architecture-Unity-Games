using System;
using Data;
using Enemy;
using UnityEngine;

namespace Hero
{
[RequireComponent(typeof(HeroAnimator))]
public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
{
    private State _state;
    public HeroAnimator animator;

    public event Action HealthChanged;
    public float Current
    {
        get => _state.currentHp;
        set
        {
            if (_state.currentHp != value)
            {
                _state.currentHp = value;
                HealthChanged?.Invoke();
            }
        }
    }

    public float Max
    {
        get => _state.maxHp;
        set => _state.maxHp = value;
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _state = progress.heroState;
        HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.heroState.currentHp = Current;
        progress.heroState.maxHp = Max;
    }

    public void TakeDamage(float damage)
    {
        if (Current <= 0)
            return;

        Current -= damage;
        animator.PlayHit();
    }
}
}