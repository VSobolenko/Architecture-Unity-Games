using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
public interface IGameFactory : IService
{
    GameObject CreateHero(GameObject at);
    void CreateHub();
}
}