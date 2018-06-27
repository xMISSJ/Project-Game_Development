using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugLegs : MasterLegs {

    public GameObject slugTrail;        // This is the actual trail, consists game objects.
    float trailTimer;
    float spawnTimer;
    TrailRenderer trailRenderer;        // This is a particle effect of the trail.
    GameObject trail;
    float trailLife;

    //Music
    [SerializeField]
    float minVolume;
    [SerializeField]
    float maxVolume;
    float controllerDistance; // This is how much the vertical and the horizontal the controller has moved /2.

    // Use this for initialization.
    void Start ()
    {
        base.Start();

        trailLife = 3;      // Indicates how long the trail game objects are alive.
        trailTimer = 3;     // Indicates the timer for the particle effect trail.
        spawnTimer = 3;     // Indicates the timer for spawning the trail game objects.

        // Fetches the TrailRenderer in the children and set this to false.
        trailRenderer = gameObject.GetComponentInChildren<TrailRenderer>();
        trailRenderer.enabled = false;
    }

    public override void Move(float horizontal, float vertical)
    {
        // Makes it so the camera moves in the same direction as the input. Also ref, because the changes are immediately applied. No return needed.
        RotateToCam(ref horizontal, ref vertical);

        // Makes it so the legs are moved in the right direction, so the legs don't move into the direction of the arms.
        moveDirection = (Quaternion.AngleAxis(gameObject.transform.eulerAngles.y, Vector3.up) * new Vector3(horizontal * maxSpeed, 0, vertical * maxSpeed) * Time.deltaTime);

        if (vertical != 0 || horizontal != 0)
            {
            // Changes the visual part of the legs. 180 Degrees is added so the legs are in the right direction in comparison to your moveDirection.
            gameObject.transform.GetChild(0).transform.rotation = Quaternion.LookRotation(new Vector3(vertical, 0, -horizontal)) * transform.parent.rotation * Quaternion.Euler(0, 180, 0);
            }
        Gravity();

        controllerDistance = Mathf.Clamp(Mathf.Abs(horizontal + vertical), 0, 1);
    }

    // This update so the update in MasterLegs is executed along with the update here.
    new private void Update()
    {
        base.Update();
        trailTimer -= Time.deltaTime;
        spawnTimer -= Time.deltaTime;

        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().volume = Mathf.Lerp(minVolume, maxVolume, controllerDistance);
        }

        // After a certain time, enables the TrailRenderer and the trail.
        if (trailTimer <= 0)
        {
            trailRenderer.enabled = true;

            // Checks whether the spawnTimer <= 0 and also whether the gameObject is moving.
            // Mathf.Abs here so that the value is always positive. This is so the the slug keeps spawning trails even when going left.
            if (spawnTimer <= 0 && Mathf.Abs(gameObject.transform.root.gameObject.GetComponent<CharacterController>().velocity.x) > 0)
            {
                // Instantiate a gameObject just after the last part of the tail and makes this the parent.
                trail = Instantiate(slugTrail, gameObject.transform.GetChild(0).Find("Cube3")) as GameObject;

                Destroy(trail, trailLife);

                spawnTimer = 0.1f;
            }
        }
    }
}
