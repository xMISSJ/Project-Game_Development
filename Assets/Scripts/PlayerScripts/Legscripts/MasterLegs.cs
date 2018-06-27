using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterLegs : MonoBehaviour
{

    public GameObject body;

    // protected CharacterController move;
    protected Rigidbody rigidBody;
    [SerializeField] protected float maxSpeed = 6;
    protected Vector3 velocity;                   //Current speed
    [SerializeField] protected float acceleration;
    [SerializeField] protected float deacceleration;
    [SerializeField] protected float turnSpeed;
    protected float gravity;
    protected float gravityVelocity;
    [SerializeField] protected CharacterController controller;
    [SerializeField] protected Vector3 moveDirection = Vector3.zero;

    [SerializeField] protected float airFriction = 4;
    // values for impact
    [SerializeField] protected float friction = 4;
    [SerializeField] protected float addedMass;      //Mass of the legs. This is added to the totalmass in the masterlegs.
     protected float totalMass;      //Mass of all bodyparts.


    Vector3 direction = Vector3.zero;
    Vector3 impact;
    float force;
    float startForce = 0;
    float ForceSlow = 0;

    protected void Start()
    {
        gravity = 0.7f;
        gravityVelocity = 0;
    }


    protected void Update()
    {
        controller.Move(moveDirection + impact);
    }

    // Use this for initialization
    public void Construct()
    {
        controller = GetComponentInParent<CharacterController>();
        velocity = new Vector3();
    }

    protected void RotateToCam(ref float horizontal, ref float vertical)
    {
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        //  direction = direction * Quaternion.an
        Quaternion rot = Camera.main.transform.rotation;
       
        rot = Quaternion.Euler(0, rot.eulerAngles.y - transform.root.eulerAngles.y, 0);

        direction = rot * direction;

        horizontal = direction.x;
        vertical = direction.z;
    }

    //use this function if you want to push the player. not for movement
    
    class Impact
    {
        public Vector3 direction;
        public float force;
    }
    List<Impact> impacts = new List<Impact>();
    public void updateImpact()
    {
        impact = Vector3.zero;
        for (int i = 0; i < impacts.Count; i++)
        {
            Impact inp = impacts[i];
            
            if (inp.force < 0)
            {
                impacts.Remove(inp);
                continue;
            }
           
            impact += inp.direction * inp.force * Time.deltaTime ;
            inp.force -= friction * Time.deltaTime;
        }
    }
    public void addImpact(Vector3 direction = new Vector3(), float force = 0)
    {
        Impact inp = new Impact();
        inp.direction = direction.normalized;
        inp.force = force;
        impacts.Add(inp);
    }

    public virtual void Move(float horizontal, float vertical)
    {

    }

    protected float Accelerate(float hor, float vert, float speed)
    {
        // if player gives input 
        if (hor > 0.1f || hor < -0.1f || vert > 0.1f || vert < -0.1)
        {
            speed += acceleration * Time.deltaTime;
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            return speed;
        }
        //if no input is given
        speed -= deacceleration * Time.deltaTime;
        if (speed < 0)
        {
            speed = 0;
        }
        return speed;
    }
    //call this in any leg script to enable accelerating and deccelerating.
    protected float Accelerate(float input, float speed)
    {
        //float speed
        //float max speed
        //0 is the minimum speed
        float limitSpeed = 0;
        if (input < -0.1 || input > 0.1)
        {
            limitSpeed = maxSpeed * input;  //Gets the limited speed relative to how far the player has moved the joystick.
        } else
        {
            limitSpeed = 0;
        }

        //Speeds up the player
        if (speed < limitSpeed && input >= 0)
        {
            speed += acceleration;
        }
        if (speed > limitSpeed && input < 0)
        {
            speed -= acceleration;
        }

        //SLows down the player
        if (speed > 0 && input == 0)
        {
            speed -= deacceleration;
        }
        if (speed < 0 && input == 0)
        {
            speed += deacceleration;
        }

        Mathf.Clamp(speed, -limitSpeed, limitSpeed);           //Limits the speed so it cant go under 0 or above the maxspeed;
        return speed;
    }

    protected void Gravity()
    {
        gravityVelocity += gravity * (1 + (totalMass/100));
        moveDirection.y -= gravityVelocity * Time.deltaTime;

        if (controller.isGrounded)
        {
            gravityVelocity = 0;
        }
    }

    public float AddedMass()    //This is a getter, so that masterbody can acces the value of addedmass
    {
        return addedMass;
    }

    public void SetTotalMass(float totalMass)
    {
        this.totalMass = totalMass + addedMass;     //Gets the total mass of the player to calculate in the gravity.
    }

    public Vector3 MoveDirection()
    {
        return direction;
    }
    public Vector3 Velocity()
    {
        return new Vector3(controller.velocity.x, 0, controller.velocity.z);
    }
    public float TotalMass ()
    {
        return totalMass;
    }

}
