using Infrastructure;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
public class AgentMoveToHero : Follow
{
    public NavMeshAgent agent;
    private Transform _heroTransform;

    public void Construct(Transform heroTransform)
    {
        _heroTransform = heroTransform;
    }

    private const float MINDistance = 1;


    private void Update()
    {
        if (Initialized() && HeroNotReached())
            agent.destination = _heroTransform.position;
    }

    private bool Initialized() => _heroTransform != null;

    private bool HeroNotReached() => Vector3.Distance(agent.transform.position, _heroTransform.position) >= MINDistance;
}
}