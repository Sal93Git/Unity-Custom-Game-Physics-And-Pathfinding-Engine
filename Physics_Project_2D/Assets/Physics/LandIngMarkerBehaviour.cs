using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandIngMarkerBehaviour : MonoBehaviour
{
    public GameObject cannonBall;
    public GameObject cannon;
    PhysicsBody cannonBallPhysicsBody ;
    public float timeOfFlight; 
    float initialVelocityY; 
    float initialVelocityX;
    public float landingPositionX;
    public Slider forceSlider;
    public Slider angleSlider;
    public float launchAngle;
    float _cannonballGravity;
    
    void Update()
    {
        // Get the initial X and Y Velocities determined by the cannon script 
        initialVelocityY = cannon.GetComponent<Cannon>().returnCannonBallVelocityY();
        initialVelocityX = cannon.GetComponent<Cannon>().returnCannonBallVelocityX();

        // Here we calculate the countdown for the project to hit the ground using initial velocity, the instantiate position and the gravity applied
        timeOfFlight = (2f * (initialVelocityY + cannon.transform.position.y)) / Mathf.Abs(cannon.GetComponent<Cannon>().returnCannonBallGravity());

        // DISPLACEMENT: Calculate landing position (the predicted displacement) we use start position plus the velocity times flight time
        landingPositionX = cannon.transform.position.x + initialVelocityX * timeOfFlight;
        
        // Position the landing marker at the estimated displacement location
        this.transform.position = new Vector3(landingPositionX, 0f, 0f);
    }


}
