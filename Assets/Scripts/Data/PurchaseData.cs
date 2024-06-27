using System;
using System.Collections.Generic;

namespace Data
{
[Serializable]
public class PurchaseData
{
    public List<BoughtIAP> boughtIAPs = new List<BoughtIAP>();

    public event Action Changed;
    
    public void AddPurchase(string id)
    {
        var boughtIAP = ProductById(id);

        if (boughtIAP != null)
            boughtIAPs.Add(boughtIAP);
        else
        {
            boughtIAPs.Add(new BoughtIAP
            {
                productId = id,
                count = 1,
            });
        }
        
        Changed?.Invoke();
    }

    private BoughtIAP ProductById(string id)
    {
        return boughtIAPs.Find(x => x.productId == id);
    }
}
}