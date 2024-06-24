using System;
using UI.Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
public class OpenWindowButton : MonoBehaviour
{
    public Button button;
    public WindowId windowId;
    private IWindowsService _windowService;

    public void Construct(IWindowsService windowService)
    {
        _windowService = windowService;
    }

    private void Awake()
    {
        button.onClick.AddListener(Open);
    }

    private void Open() => _windowService.Open(windowId);
}
}