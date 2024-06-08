using System;
using UnityEngine;
using UnityGameFramework.Runtime;

public class ProjectileData//  : EntityData
{
    public Vector3 TargetPosition;
    public Action<Entity> OnTouchDown;
}