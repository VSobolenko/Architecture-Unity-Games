using System;
using Infrastructure;
using Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
public class AgentMoveToHero : Follow
{
    public NavMeshAgent agent;
    private IGameFactory _gameFactory;
    private Transform _heroTransform;
    private const float MINDistance = 1;

    private void Start()
    {
        _gameFactory = AllServices.Container.Single<IGameFactory>();

        if (_gameFactory.HeroGameObject != null) 
            InitializeHeroTransform();
        else
        {
            _gameFactory.HeroCreated += HeroCreated;
        }
    }

    private void Update()
    {
        if (Initialized() && HeroNotReached())
            agent.destination = _heroTransform.position;
    }

    private bool Initialized() => _heroTransform != null;

    private void HeroCreated() => InitializeHeroTransform();

    private void InitializeHeroTransform() => _heroTransform = _gameFactory.HeroGameObject.transform;

    private bool HeroNotReached() => Vector3.Distance(agent.transform.position, _heroTransform.position) >= MINDistance;
}
}