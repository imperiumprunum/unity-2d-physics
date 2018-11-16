using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Controller2D))]

public class Player : MonoBehaviour {

    Vector3 velocity;
    float gravity;
    float moveSpeed = 10.0f;

    float jumpVelocity;

    public float jumpHeight = 4;
    public float jumptTimeApex = .5f;

    float accelerationTimeAirborn = .2f;
    float accelrationTimeGrounded = .1f;

    float velocitySmoothing;

    Controller2D controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();

        // kinematic equations
        gravity = -(2 * jumpHeight) / Mathf.Pow(jumptTimeApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * jumptTimeApex;
	}
	
	// Update is called once per frame
	void Update () {

        //  prevents gravity stacking
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        //  movement smoothing
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocitySmoothing, (controller.collisions.below)?accelrationTimeGrounded:accelerationTimeAirborn);
        //  v = vo + at

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}
}
