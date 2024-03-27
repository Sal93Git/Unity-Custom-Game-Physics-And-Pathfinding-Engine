using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    public GameObject projectile;
    public Slider forceSlider;
    public Slider angleSlider;

    public LandIngMarkerBehaviour landingMarker;
    GameObject firedProjectile;
    [SerializeField]
    float cannonBallGravity = -0.3f;
    
    float initialSpeed;
    float launchAngle;
    float initialVelocityX;
    float initialVelocityY;
    void Update()
    {
       // Get the Initial speed and angle values from the GUI sliders values
        initialSpeed = forceSlider.value;
        launchAngle = angleSlider.value;

        initialVelocityX = initialSpeed * Mathf.Cos(Mathf.Deg2Rad * launchAngle);
        initialVelocityY = initialSpeed * Mathf.Sin(Mathf.Deg2Rad * launchAngle);

        //Set the rotation of the cannon based on the launch angle provided as a float
        float zRotationAngle = launchAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, zRotationAngle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Check if a cannonball exists in the scene ( i only want to fire 1 cannonball at a time)
            if(firedProjectile == null)
            {
                
                // Instantiate the object at the current position and rotation of the GameObject
                firedProjectile=Instantiate(projectile, transform.position, transform.rotation);
               
                // Provide the Landing marker with a reference to the newly created cannonball
                landingMarker.cannonBall = firedProjectile;
                
                PhysicsBody cannonBallScript = firedProjectile.GetComponent<PhysicsBody>();
                if (cannonBallScript != null)
                {
                    // Set the values on the PhysicsBody script (Velocity, Gravity , Acceleration)
                    cannonBallScript.setVelocity(initialVelocityX,initialVelocityY,0f);
                    cannonBallScript.setGravity(0f,cannonBallGravity,0f);
                    cannonBallScript.setAccleration(0f,0f,0f);             
                }
                else
                {
                    // Debug log if the object didnt have the exected script
                    Debug.LogWarning("PhysicsBody not found on the instantiated object");
                }

            }
        }

    }

    public float returnCannonBallVelocityX()
    {
        return initialVelocityX;
    }
    public float returnCannonBallVelocityY()
    {
        return initialVelocityY;
    }
    public float returnCannonBallGravity()
    {
        return cannonBallGravity;
    }
}
