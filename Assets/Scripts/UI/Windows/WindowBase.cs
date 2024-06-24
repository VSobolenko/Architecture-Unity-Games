using System;
using Data;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
public abstract class WindowBase : MonoBehaviour
{
    public Button closeButton;

    protected IPersistentProgressService _progressService;
    protected PlayerProgress Progress => _progressService.Progress;

    public void Construct(IPersistentProgressService progressService)
    {
        _progressService = progressService;
    }

    private void Awake() => OnAwake();

    private void Start()
    {
        Initialize();
        SubscribeUpdates();
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    protected virtual void OnAwake() => closeButton.onClick.AddListener(() => Destroy(gameObject));

    protected virtual void Initialize()
    {
    }

    protected virtual void SubscribeUpdates()
    {
    }

    protected virtual void Cleanup()
    {
    }
}
}