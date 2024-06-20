using System;
using Enemy;
using UnityEngine;

namespace UI
{
public class ActorUI : MonoBehaviour
{
    public HpBar hpBar;
    private IHealth _heroHealth;

    public void Construct(IHealth health)
    {
        _heroHealth = health;
        _heroHealth.HealthChanged += UpdateHpBare;
    }

    private void OnDestroy()
    {
        _heroHealth.HealthChanged -= UpdateHpBare;
    }

    private void UpdateHpBare()
    {
        hpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
    }
}
}