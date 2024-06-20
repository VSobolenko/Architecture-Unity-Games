using Logic;
using UnityEditor;
using UnityEngine;

namespace GameEditor
{
[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(EnemySpawner spawner, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spawner.transform.position, 0.5f);
    }
}
}