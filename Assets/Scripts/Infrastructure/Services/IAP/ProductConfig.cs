using System;
using UnityEngine.Purchasing;

namespace Infrastructure.Services.IAP
{
[Serializable]
public class ProductConfig
{
    public string id;
    public ProductType productType;

    public int maxPurchaseCount;
    public ItemType itemType;
    public int quantity;
    public string price;
    public string icon;
}
}