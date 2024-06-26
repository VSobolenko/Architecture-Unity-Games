using Infrastructure;
using Infrastructure.Services;
using UnityEngine;

namespace Logic
{
public class LevelTransferTrigger : MonoBehaviour
{
    private const string PlayerTag = "Player";
    public string transferTo;
    private IGameStateMachine _stateMachine;

    private bool _triggered;
    private void Awake()
    {
        _stateMachine = AllServices.Container.Single<IGameStateMachine>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered)
            return;

        if (other.CompareTag(PlayerTag))
        {
            _stateMachine.Enter<LoadLevelState, string>(transferTo);
            _triggered = true;
        }
    }
}
}