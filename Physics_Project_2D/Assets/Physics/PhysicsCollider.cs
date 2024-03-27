using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCollider : MonoBehaviour
{
    public enum Type
    {
        CIRCLE,
        POINT,
        RECTANGLE
    }

    public Type type;

    public GameObject objectCollidedWith;
}
