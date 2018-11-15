using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]

public class Controller2D : MonoBehaviour {

    const float skinWidth = .05f;

 

    public LayerMask collisionMask;

    BoxCollider2D colider;
    RaycastOrigins raycastOrigins;

    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;

    public float horizontalRaySpacing;
    public float vecticalRaySpacing;

    // Use this for initialization
    void Start () {
        //Fetch the Collider from the GameObject
        colider = GetComponent<BoxCollider2D>();
        calcRaySpacing();
    }
	
	// Update is called once per frame
	void Update () {

    }

    //Gravity simulation

    public void Move(Vector3 velocity)
    {
        
        UpdateRaycastOrigins();
            
        if(velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        
        if(velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }


        transform.Translate(velocity);
       
    }


    //Colisions vectical

    void VerticalCollisions(ref Vector3 velocity)
    {
        // Velocity direction +/- 1
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < VerticalRayCount; i++)
        {
            //  Figure raycastOrigin to render
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

            // vector2.right only to make vector addition, velocity.x ?
            rayOrigin += Vector2.right * (vecticalRaySpacing * i + velocity.x); 

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            // Drawing rays
            // raycast(x,y) + right translation * space_between * rays count ; -2 stands for lenght (and down dir)
            //Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right * vecticalRaySpacing * i, Vector2.up * -2, Color.red);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // Debug.Log(velocity.y);
            if (hit)
            {

                //changing ref velocity according to getting closer to the object
                //  meaning velocity = 0 most of the times
                // hit.distance always positive then * dirY, hit.distance turns 0 on very close
                velocity.y = (hit.distance - skinWidth) * directionY;
                //velocity.y = 0;

                rayLength = hit.distance;

            }

        }


    }

    //Horizontal collider

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
            }
        }
    }

    //Raycast methods

    void calcRaySpacing()
    {
        Bounds bounds = colider.bounds;
        bounds.Expand(skinWidth * -2);

        // At least 2 raycast need to exist, no top limit
        HorizontalRayCount = Mathf.Clamp(HorizontalRayCount, 2, int.MaxValue);
        VerticalRayCount = Mathf.Clamp(VerticalRayCount, 2, int.MaxValue);

        // For at least 2 raycasts its size / (2 -1) so first would be draw on 0 and next on size.x
        horizontalRaySpacing = bounds.size.x / (HorizontalRayCount - 1);
        vecticalRaySpacing = bounds.size.x / (VerticalRayCount - 1);

    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = colider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
