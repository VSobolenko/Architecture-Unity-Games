using System;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
internal class ShopItem : MonoBehaviour
{
    public Button buyItemButton;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI availableItemsLefText;
    public Image icon;
    private ProductDescription _productDescription;
    private IIAPService _iapService;
    private IAssets _assets;

    public void Construct(IIAPService iapService, ProductDescription productDescription, IAssets assets)
    {
        _productDescription = productDescription;
        _iapService = iapService;
        _assets = assets;
    }

    public async void Initialize()
    {
        buyItemButton.onClick.AddListener(ObBuyItemClick);

        priceText.text = _productDescription.config.price;
        quantityText.text = _productDescription.config.quantity.ToString();
        availableItemsLefText.text = _productDescription.availablePurchasesLeft.ToString();
        icon.sprite = await _assets.Load<Sprite>(_productDescription.config.icon);
    }

    private void ObBuyItemClick()
    {
        _iapService.StartPurchase(_productDescription.id);
    }
}
}