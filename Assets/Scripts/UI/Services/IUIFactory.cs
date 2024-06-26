using System;
using System.Threading.Tasks;
using Infrastructure.Services;
using UnityEngine;

namespace UI.Services
{
public interface IUIFactory : IService
{
    GameObject CreateShop();
    Task CreateUIRoot();
}
}