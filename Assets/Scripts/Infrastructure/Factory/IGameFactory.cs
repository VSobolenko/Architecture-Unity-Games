using System.Collections.Generic;
using Hero;
using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
public interface IGameFactory : IService
{
    GameObject CreateHero(GameObject at);
    void CreateHub();
    List<ISavedProgressReader> ProgressReaders { get; }
    List<ISavedProgress> ProgressWriters { get; }
    void Cleanup();
}
}