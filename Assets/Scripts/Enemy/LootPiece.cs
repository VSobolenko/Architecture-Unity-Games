using System;
using System.Collections;
using Data;
using TMPro;
using UnityEngine;

namespace Enemy
{
public class LootPiece : MonoBehaviour
{
    public GameObject skull;
    public GameObject fxPrefab;
    public TextMeshPro lootText;
    public GameObject pickupPopup; 
    
    private Loot _loot;
    private bool _picked;
    private WorldData _worldData;

    public void Construct(WorldData worldData)
    {
        _worldData = worldData;
    }
    
    public void Initialize(Loot loot)
    {
        _loot = loot;
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickup();
    }

    private void Pickup()
    {
        if (_picked)
            return;

        _picked = true;
        UpdateWorldData();

        HideSkull();
        PlayPickupFx();
        ShowText();
        StartCoroutine(StartDestroyTimer());
    }

    private void UpdateWorldData()
    {
        _worldData.lootData.Collect(_loot);
    }

    private void HideSkull()
    {
        skull.SetActive(false);
    }

    private void PlayPickupFx()
    {
        Instantiate(fxPrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator StartDestroyTimer()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    private void ShowText()
    {
        lootText.text = $"{_loot.value}";
        pickupPopup.SetActive(true);
    }
}
}