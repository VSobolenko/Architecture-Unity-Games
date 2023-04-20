using System;
using UnityEngine;

namespace CameraLogic
{
public class CameraFollow : MonoBehaviour
{
    public float rotationAngleX;
    public float distance;
    public float offsetY;
    
    [SerializeField]
    private Transform _following;

    private void LateUpdate()
    {
        if (_following == null)
            return;

        var rotation = Quaternion.Euler(rotationAngleX, 0, 0);
        var followingPosition = _following.position;
        followingPosition.y += offsetY;
        var pos = rotation * new Vector3(0, 0, -distance) + followingPosition;

        transform.rotation = rotation;
        transform.position = pos;
    }

    public void Follow(GameObject gameObjectFollow)
    {
        _following = gameObjectFollow.transform;
    }
}
}