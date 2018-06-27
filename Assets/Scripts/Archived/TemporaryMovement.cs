using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryMovement : MonoBehaviour {

    public GameObject bullet;

    private CharacterController controller;
    public float movementSpeed = 5;
    public float acceleration;
    public float deceleration;
    public float turnSpeed;
    
    public Vector3 moveDirection;
    public Rigidbody rigidBody;
    public GameObject armLeft;
    public GameObject armRight;
    GameObject wapen;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Make the player controlable relative to the camera (e.g. Player presses forward and always moves away from camera.)
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0f;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = (horizontal * right + vertical * forward);
        moveDirection *= movementSpeed;

        transform.Translate(moveDirection * Time.deltaTime);

    }
}