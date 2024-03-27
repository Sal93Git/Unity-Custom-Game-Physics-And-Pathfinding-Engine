using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonBallBehaviour : MonoBehaviour
{
    void Update()
    {   
        // check if this object has collided with something
        if(gameObject.GetComponent<PhysicsCollider>().objectCollidedWith !=null)
        {
            GameObject tempObjectReference = gameObject.GetComponent<PhysicsCollider>().objectCollidedWith;
            //print("CANNONBALL HIT" + gameObject.GetComponent<PhysicsCollider>().objectCollidedWith.name);
            print("CANNONBALL HIT" + tempObjectReference);

            // if the cannonball hits the ground destroy this cannonball
            if(tempObjectReference.name == "ground")
            {
                Destroy(gameObject);
            }

            // if the cannonball hits a target dummy apply some force to it
            if(tempObjectReference.tag == "TargetDummyBall")
            {
                Vector3 cannonBallVelocity = gameObject.GetComponent<PhysicsBody>().getVelocity();
                tempObjectReference.GetComponent<PhysicsBody>().setAccleration(1f,0.5f,0f);
                tempObjectReference.GetComponent<PhysicsBody>().setVelocity(cannonBallVelocity.x,-cannonBallVelocity.y,0f);
                tempObjectReference.GetComponent<PhysicsBody>().setGravity(0,-0.8f,0);
            }

            // if the cannonball hits a crate destroy the crate
            if(tempObjectReference.tag == "crate")
            {
                Destroy(tempObjectReference);
            }

            // if the ball hits an obstacle check the mass and determine the next action (which object gets destroyed)
            if(tempObjectReference.tag == "obstalce")
            {
                if(tempObjectReference.GetComponent<PhysicsBody>().getMass() <= gameObject.GetComponent<PhysicsBody>().getMass())
                {
                    Destroy(tempObjectReference);
                }
                else 
                {
                    Destroy(gameObject);
                }
            }

            
            
        }
    }
}
