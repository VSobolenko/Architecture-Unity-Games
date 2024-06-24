using System;

namespace UI.Services
{
public class WindowsService : IWindowsService
{
    private IUIFactory _uiFactory;

    public WindowsService(IUIFactory uiFactory)
    {
        _uiFactory = uiFactory;
    }

    public void Open(WindowId id)
    {
        switch (id)
        {
            case WindowId.Unknown:
                break;
            case WindowId.Shop:
                _uiFactory.CreateShop();
                break;
        }
    }
}
}