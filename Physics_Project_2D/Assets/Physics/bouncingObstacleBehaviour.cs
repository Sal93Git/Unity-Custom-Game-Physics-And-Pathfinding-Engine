using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncingObstacleBehaviour : MonoBehaviour
{
   void Update()
    {
        // check if this object has collided with something
        if(gameObject.GetComponent<PhysicsCollider>().objectCollidedWith !=null)
        {
            GameObject tempObjectReference = gameObject.GetComponent<PhysicsCollider>().objectCollidedWith;

            // if the ball hits the ground simulate a bounce
            if(tempObjectReference.name == "ground")
            {
                gameObject.GetComponent<PhysicsBody>().velocity *=-1;
            } 
        }
    }
}
