using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 gravity;

    public int mass;

    public void setAccleration (float _x, float _y , float _z)
    {
        acceleration.x = _x;
        acceleration.y = _y;
        acceleration.z = _z;
    }

    public void setVelocity (float _x, float _y , float _z)
    {
        velocity.x = _x;
        velocity.y = _y;
        velocity.z = _z;
    }
    public void setGravity (float _x, float _y , float _z)
    {
        gravity.x = _x;
        gravity.y = _y;
        gravity.z = _z;
    }

    public Vector3 getVelocity()
    {
        return velocity;
    }

    public int getMass()
    {
        return mass;
    }

    public void setMass(int massToSet)
    {
        mass = massToSet;
    }

}
