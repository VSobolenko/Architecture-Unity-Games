using Infrastructure.Services;
using UnityEngine;

namespace Infrastructure
{
public interface IAssets : IService
{
    GameObject Instantiate(string path);
    GameObject Instantiate(string path, Vector3 at);
}
}