using System;

namespace Data
{
[Serializable]
public class LootData
{
    public int collected;
    public Action changed;

    public void Collect(Loot loot)
    {
        collected += loot.value;
        changed?.Invoke();
    }

    public void Add(int count)
    {
        collected += count;
        changed?.Invoke();
    }
}
}