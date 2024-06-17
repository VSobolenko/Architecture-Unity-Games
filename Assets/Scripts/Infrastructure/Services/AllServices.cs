using System;

namespace Infrastructure.Services
{
public class AllServices
{
    private static AllServices _instance;
    public static AllServices Container => _instance ??= new AllServices();

    public TService RegisterSingle<TService>(TService implementation) where TService : IService
    {
        Implementation<TService>.ServiceInstance = implementation;

        return Single<TService>();
    }

    public TService Single<TService>() where TService : IService
    {
        return Implementation<TService>.ServiceInstance;
    }
    
    private static class Implementation<TService> where TService : IService
    {
        public static TService ServiceInstance;
    }
}
}