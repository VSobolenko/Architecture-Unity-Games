using System;
using System.Linq;
using Infrastructure;
using Infrastructure.Services;
using UnityEngine;

namespace Enemy
{
[RequireComponent(typeof(EnemyAnimator))]
public class Attack : MonoBehaviour
{
    public EnemyAnimator enemyAnimator;
    public float cooldown = 3f;
    public float cleavage = 0.5f;
    public float effectiveDistance = 0.5f;

    private IGameFactory _gameFactory;
    private Transform _heroTransform;
    private float _attackCooldown;
    private bool _isAttacking;
    private int _layerMask;
    private Collider[] _hits =  new Collider[1];
    private bool _attackIsActive;

    private void Awake()
    {
        _gameFactory = AllServices.Container.Single<IGameFactory>();
        _layerMask = 1 << LayerMask.NameToLayer("Player");
        _gameFactory.HeroCreated += OnHeroCreated;
    }

    private void Update()
    {
        UpdateCooldown();
        
        if (CanAttack())
            StartAttack();
    }

    private void OnAttack()
    {
        if (Hit(out Collider hit))
        {
            PhysicsDebug.DrawDebug(StartPoint(), cleavage, 1);
        }
    }

    private bool Hit(out Collider hit)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), cleavage, _hits, _layerMask);
        hit = _hits.FirstOrDefault();
        return hitCount > 0;
    }

    private Vector3 StartPoint() => new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z) + transform.forward * effectiveDistance;

    private void OnAttackEnded()
    {
        _attackCooldown = cooldown;
        _isAttacking = false;
    }

    private void UpdateCooldown()
    {
        if (!CooldownIsUp())
            _attackCooldown -= Time.deltaTime;
    }

    private bool CanAttack() => _attackIsActive && !_isAttacking && CooldownIsUp();

    private void StartAttack()
    {
        transform.LookAt(_heroTransform);
        enemyAnimator.PlayAttack();

        _isAttacking = true;
    }

    private bool CooldownIsUp() => _attackCooldown <= 0;

    private void OnHeroCreated() => _heroTransform = _gameFactory.HeroGameObject.transform;

    public void EnableAttack()
    {
        _attackIsActive = true;
    }

    public void DisableAttack()
    {
        _attackIsActive = false;
    }
}
}