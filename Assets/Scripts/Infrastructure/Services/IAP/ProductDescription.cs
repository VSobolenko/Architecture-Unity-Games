using UnityEngine.Purchasing;

namespace Infrastructure.Services.IAP
{
public class ProductDescription
{
    public string id;
    public Product product;
    public ProductConfig config;
    public int availablePurchasesLeft;
}
}