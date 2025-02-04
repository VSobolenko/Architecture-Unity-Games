using System;
using Data;
using Enemy;
using Infrastructure.Services;
using Services.Input;
using UnityEngine;

namespace Hero
{
[RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
public class HeroAttack : MonoBehaviour, ISavedProgressReader
{
    public HeroAnimator Animator;
    public CharacterController CharacterController;

    private IInputService _inputService;

    private static int _layerMask;
    private Collider[] _hits = new Collider[3];

    private float radius;
    private Stats _stats;

    private void Awake()
    {
        _inputService = AllServices.Container.Single<IInputService>();

        _layerMask = 1 << LayerMask.NameToLayer("Hittable");
    }

    private void Update()
    {
        if(_inputService.IsAttackButtonUp() && !Animator.IsAttacking)
            Animator.PlayAttack();
    }

    public void OnAttack()
    {
        for (int i = 0; i < Hit(); i++)
            _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.damage);
    }

    private int Hit() => Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.damageRadius, _hits, _layerMask);

    private Vector3 StartPoint() =>
        new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);

    public void LoadProgress(PlayerProgress progress)
    {
        _stats = progress.heroStats;
    }
}
}