using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsEngine : MonoBehaviour
{

    void Update()
    {
        UpdateVelocities();
        UpdatePosition();
        Collisions();
    }

    void UpdateVelocities()
    {
        PhysicsBody[] bodies = GameObject.FindObjectsOfType<PhysicsBody>();
        foreach(PhysicsBody b in bodies)
        {
            b.velocity += b.acceleration * Time.deltaTime;
            b.velocity += b.gravity * Time.deltaTime;
        }
    }

    void UpdatePosition()
    {
        PhysicsBody[] bodies = GameObject.FindObjectsOfType<PhysicsBody>();
        foreach(PhysicsBody b in bodies)
        {
            b.gameObject.transform.position += b.velocity * Time.deltaTime;
        }

    }

    void Collisions()
    {
        PhysicsCollider[] colliders = GameObject.FindObjectsOfType<PhysicsCollider>();
        for(int i = 0; i < colliders.Length; i++)
        {
            PhysicsCollider a = colliders[i];
            a.objectCollidedWith = null;
            for (int j = i +1 ; j < colliders.Length; j ++)
            {
                PhysicsCollider b = colliders[j];
                bool circleToCircle = (a.type == PhysicsCollider.Type.CIRCLE && b.type == PhysicsCollider.Type.CIRCLE);
                if(circleToCircle)
                {
                    float aRad = a.transform.localScale.x / 2f;
                    float bRad = b.transform.localScale.x / 2f;

                    Vector3 aToB = b.transform.position - a.transform.position;
                    float intersectionDepth = (aRad + bRad) - aToB.magnitude;
                    if(intersectionDepth > 0f )
                    {
                        //Debug.Log("Collision detected between circles");
                        a.objectCollidedWith = b.gameObject;
                    }   
                }

                bool pointToRectangle = (a.type == PhysicsCollider.Type.POINT && b.type == PhysicsCollider.Type.RECTANGLE || a.type == PhysicsCollider.Type.RECTANGLE && b.type == PhysicsCollider.Type.POINT);
                if(pointToRectangle)
                {
                    PhysicsCollider point = a.type == PhysicsCollider.Type.POINT ? a : b;
                    PhysicsCollider aaRect = b.type == PhysicsCollider.Type.RECTANGLE ? b : a;

                    float width = aaRect.transform.localScale.x;
                    float height = aaRect.transform.localScale.y;

                    float leftHandSide = aaRect.transform.position.x - (width / 2f);
                    float rightHandSide = aaRect.transform.position.x + (width / 2f);
                    float topSide = aaRect.transform.position.y + (height / 2f);
                    float bottomSide = aaRect.transform.position.y - (height / 2f);

                    bool onLeftHandSide = point.transform.position.x < leftHandSide;
                    bool onRightHandSide = point.transform.position.x > rightHandSide;
                    bool below = point.transform.position.y < bottomSide;
                    bool above = point.transform.position.y > topSide;

                    if(onLeftHandSide || onRightHandSide || below || above) continue;
                    point.objectCollidedWith = b.gameObject;
                    //print("POINT ON RECTANGLE COLLISION");
                    continue;
                }
                

                bool RectangleToRectangle = (a.type == PhysicsCollider.Type.RECTANGLE && b.type == PhysicsCollider.Type.RECTANGLE);
                if(RectangleToRectangle)
                {
                    PhysicsCollider aaRectA =  a;
                    PhysicsCollider aaRectB =  b;

                    float widthA = aaRectA.transform.localScale.x;
                    float heightA = aaRectA.transform.localScale.y;

                    float leftHandSideA = aaRectA.transform.position.x - (widthA / 2f);
                    float rightHandSideA = aaRectA.transform.position.x + (widthA / 2f);
                    float topSideA = aaRectA.transform.position.y + (heightA / 2f);
                    float bottomSideA = aaRectA.transform.position.y - (heightA / 2f);

                    
                    float widthB = aaRectB.transform.localScale.x;
                    float heightB = aaRectB.transform.localScale.y;

                    float leftHandSideB = aaRectB.transform.position.x - (widthB / 2f);
                    float rightHandSideB = aaRectB.transform.position.x + (widthB / 2f);
                    float topSideB = aaRectB.transform.position.y + (heightB / 2f);
                    float bottomSideB = aaRectB.transform.position.y - (heightB / 2f);

                    // temp bools to hold if we have a single side overlap
                    bool bottomOverTop = false;
                    bool topOverBottom = false;
                    bool leftOverRight = false;
                    bool rightOverLeft = false;

                    // we check every side of the rectangles for on overlap on that given side
                    if(bottomSideA < topSideB && bottomSideA > bottomSideB)
                    {
                        bottomOverTop = true;
                    }
                    if(bottomSideB < topSideA && bottomSideB > bottomSideA)
                    {
                        topOverBottom = true;
                    }
                    if(leftHandSideA < rightHandSideB && leftHandSideA > leftHandSideB)
                    {
                        leftOverRight = true;
                    }
                    if(leftHandSideB < rightHandSideA && leftHandSideB > leftHandSideA)
                    {
                        rightOverLeft = true;
                    }

                    // we check for an overlap to exist both on the X and the Y axis
                    if((bottomOverTop || topOverBottom ) && (leftOverRight ||rightOverLeft ))
                    {
                        aaRectA.objectCollidedWith = b.gameObject;
                        // print("RECT ON RECT COLLISION!"+b.gameObject);
                    }                  
                }
                

                bool circleToRectangle = (a.type == PhysicsCollider.Type.CIRCLE && b.type == PhysicsCollider.Type.RECTANGLE || a.type == PhysicsCollider.Type.RECTANGLE && b.type == PhysicsCollider.Type.CIRCLE);
                if(circleToRectangle)
                {
                    PhysicsCollider circle = a.type == PhysicsCollider.Type.CIRCLE ? a : b;
                    PhysicsCollider Rect = b.type == PhysicsCollider.Type.RECTANGLE ? b : a;

                    float circleRadius = circle.transform.localScale.x / 2f;
                    float width = Rect.transform.localScale.x;
                    float height = Rect.transform.localScale.y;

                    float leftHandSide = Rect.transform.position.x - (width / 2f);
                    float rightHandSide = Rect.transform.position.x + (width / 2f);
                    float topSide = Rect.transform.position.y + (height / 2f);
                    float bottomSide = Rect.transform.position.y - (height / 2f);

                    float closestX = Mathf.Clamp(circle.transform.position.x, leftHandSide, rightHandSide);
                    float closestY = Mathf.Clamp(circle.transform.position.y, bottomSide, topSide);

                    // Calculate the distance between the circle's center and the closest point on the rectangle
                    float distanceOnX = circle.transform.position.x - closestX;
                    float distanceOnY = circle.transform.position.y - closestY;

                    if (distanceOnX * distanceOnX + distanceOnY * distanceOnY < circleRadius * circleRadius)
                    {
                        //Debug.Log("Collision detected between circle and rectangle");
                        //Debug.Log(a.gameObject + "hit "+ b.gameObject);
                        circle.objectCollidedWith = b.gameObject;
                    }
                   
                }
                

            }
        }
    }
}
