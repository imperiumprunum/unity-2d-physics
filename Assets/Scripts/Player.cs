using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (Controller2D))]

public class Player : MonoBehaviour {

    Vector3 velocity;
    float gravity = -20.0f;
    float moveSpeed = 10.0f;

    Controller2D controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {

        //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //velocity.x = input.x * moveSpeed;
        //  v = vo + at

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	}
}
