using System;
using Data;
using Infrastructure.Services;

namespace Infrastructure
{
public interface ISaveLoadService : IService
{
    void SaveProgress();
    PlayerProgress LoadProgress();
}
}