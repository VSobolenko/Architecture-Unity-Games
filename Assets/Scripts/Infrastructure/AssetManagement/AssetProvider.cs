using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Infrastructure
{
public class AssetProvider : IAssets
{
    private Dictionary<string, AsyncOperationHandle> _completeCache = new();
    private Dictionary<string, List<AsyncOperationHandle>> _handles = new();

    public void Initialize()
    {
        Addressables.InitializeAsync();
    }

    public async Task<T> Load<T>(AssetReference assetReference) where T : class
    {
        if (_completeCache.TryGetValue(assetReference.AssetGUID, out var completeHandle))
            return completeHandle.Result as T; 

        return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(assetReference), assetReference.AssetGUID);
    }

    public async Task<T> Load<T>(string address) where T : class
    {
        if (_completeCache.TryGetValue(address, out var completeHandle))
            return completeHandle.Result as T;

        return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(address), address);
    }

    public Task<GameObject> Instantiate(string address)
    {
        return Addressables.InstantiateAsync(address).Task;
    }

    public Task<GameObject> Instantiate(string address, Vector3 at)
    {
        return Addressables.InstantiateAsync(address, at, Quaternion.identity).Task;
    }

    public Task<GameObject> Instantiate(string address, Transform under)
    {
        return Addressables.InstantiateAsync(address, under).Task;
    }

    private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
    {
        handle.Completed += completeHandle => { _completeCache[cacheKey] = completeHandle; };

        AddHandle(cacheKey, handle);

        return await handle.Task;
    }

    private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
    {
        if (!_handles.TryGetValue(key, out  var resourcesHandle))
        {
            resourcesHandle = new List<AsyncOperationHandle>();
            _handles[key] = resourcesHandle;
        }
        
        resourcesHandle.Add(handle);
    }

    public void Cleanup()
    {
        foreach (List<AsyncOperationHandle> resourcesHandle in _handles.Values)
            foreach (AsyncOperationHandle handle in resourcesHandle)
                Addressables.Release(handle);
        
        _completeCache.Clear();
        _handles.Clear();
    }
}
}