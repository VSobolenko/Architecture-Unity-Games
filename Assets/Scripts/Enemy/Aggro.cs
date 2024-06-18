using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
public class Aggro : Follow
{
    public TriggerObserver triggerObserver;
    public AgentMoveToHero follow;

    public float cooldown;
    private Coroutine _aggroCoroutine;
    private bool _hasAggroTarget;

    private void Start()
    {
        triggerObserver.TriggerEnter += TriggerEnter;
        triggerObserver.TriggerExit += TriggerExit;

        SwitchFollowOff();
    }

    private void OnDestroy()
    {
        triggerObserver.TriggerEnter -= TriggerEnter;
        triggerObserver.TriggerExit -= TriggerExit;
    }
    
    private void TriggerEnter(Collider obj)
    {
        if(_hasAggroTarget) return;
      
        StopAggroCoroutine();

        SwitchFollowOn();
    }

    private void TriggerExit(Collider obj)
    {
        if(!_hasAggroTarget) return;
      
        _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
    }

    private IEnumerator SwitchFollowOffAfterCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        SwitchFollowOff();
    }

    private void StopAggroCoroutine()
    {
        if(_aggroCoroutine == null) return;
      
        StopCoroutine(_aggroCoroutine);
        _aggroCoroutine = null;
    }

    private void SwitchFollowOn()
    {
        _hasAggroTarget = true;
        follow.enabled = true;
    }

    private void SwitchFollowOff()
    {
        _hasAggroTarget = false;
        follow.enabled = false;
    }
}
}