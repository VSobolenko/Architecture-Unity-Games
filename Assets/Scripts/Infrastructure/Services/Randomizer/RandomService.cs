using Infrastructure.Services.Randomizer;
using UnityEngine;

namespace Infrastructure
{
internal class RandomService : IRandomService
{
    public int Next(int min, int max) =>
        Random.Range(min, max);
}
}