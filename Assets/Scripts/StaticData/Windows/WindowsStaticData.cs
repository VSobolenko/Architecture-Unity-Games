using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
[CreateAssetMenu(fileName = "WindowStaticData", menuName = "StaticData/Window static data")]
public class WindowsStaticData : ScriptableObject
{
    public List<WindowConfig> configs;
}
}