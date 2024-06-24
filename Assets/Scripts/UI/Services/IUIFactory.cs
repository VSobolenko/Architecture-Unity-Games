using System;
using Infrastructure.Services;
using UnityEngine;

namespace UI.Services
{
public interface IUIFactory : IService
{
    GameObject CreateShop();
    void CreateUIRoot();
}
}