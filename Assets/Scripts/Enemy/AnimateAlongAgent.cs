using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAnimator))]
public class AnimateAlongAgent : MonoBehaviour
{
    public NavMeshAgent agent;
    public EnemyAnimator animator;
    private const float MINVelocity = 0.1f;

    private void Update()
    {
        if (ShouldMove())
            animator.Move(agent.velocity.magnitude);
        else
            animator.StopMoving();
    }

    private bool ShouldMove() => agent.velocity.magnitude > MINVelocity && agent.remainingDistance > agent.radius;
}
}