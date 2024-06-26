using System.Linq;
using Logic;
using StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameEditor
{
[CustomEditor(typeof(LevelStaticData))]
public class LevelStaticDataEditor : Editor
{
    private const string InitialPointTag = "InitialPoint";
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var levelData = (LevelStaticData) target;

        if (GUILayout.Button("Collect"))
        {
            levelData.enemySpawners =
                FindObjectsOfType<SpawnMarker>()
                    .Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().id, x.monsterTypeId,
                                                      x.transform.position))
                    .ToList();

            levelData.levelKey = SceneManager.GetActiveScene().name;
            
            levelData.initialHeroPosition = GameObject.FindWithTag(tag: InitialPointTag).transform.position;
        }

        EditorUtility.SetDirty(target);
    }
}
}