using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MasterArm
{

    [SerializeField] private int ammo;                 //The max amount of ammo in a clip
    [SerializeField] private float pushForce;           //Force of the bullet inpact
    [SerializeField] private float cooldown;             //The amount of time it takes before next bullet comes out of gun
    [SerializeField] private float damage;               //The damage one bullet deals
    [SerializeField] private float reloadTime;           //Time it takes before gun is reloaded.
    [SerializeField] private float bulletSpeed;           //Speed at which the bullets fly
    [SerializeField] private float spread;             //A small number to change the direction of the bullet. 0 if you dont want bullets to deviate.
    [SerializeField] private int bulletShot;             //How many bullets you shoot at once
    [SerializeField] private float decayTime;             //How long it takes before the bullets despawn.
    [SerializeField] private bool useGravity;                 //do bullets use gravity
    [SerializeField] private float mass;             //mass of the bullets
    [SerializeField] private Vector3 sizeModifier;        //Changes the size of the bullet
    [SerializeField] private GameObject bullet;       //Indexing bullets     

    GameObject target;      //AUTO aim target.

    //public GameObject test;

    //Timer related stuff
    protected float timeLeft;
    protected bool timerRunning;
    protected bool timerEnded = false;

    private bool reloading;               //Wether the player is in the process of reloading
    private int currentAmmo;

    private void Update()
    {
        //Dit zorgt dat de timer afteld. Dit moeten we later nog veranderen zodat we niet overerven van de timer
        if (timerRunning == true)
        {
            TimerRunning();
        }

        //AutoAim(test);
        /*
        if (autoAim)
        {
            AutoAim(target);
        }
        checkAutoAim();
        */
    }

    new void Start()
    {
        //test = GameObject.Find("GOAL");
        currentAmmo = ammo;
        base.Start();
    }

    public override void Fire()                     //Shoots a bullet.
    {
        if (currentAmmo > 0 && timerRunning == false)
        {
            TimerStart(cooldown);
            Shoot();
        }
        if (currentAmmo <= 0 && timerRunning == false && reloading == false)
        {
            reloading = true;
            TimerStart(reloadTime);
        }
        if (reloading == true && timerRunning == false)            //If your ammo is empty, and you have not started reloading
        {
            Reload(ammo);
        }
    }

    public override void Shoot()
    {
        for (int i = 0; i < bulletShot; i++)
        {
            var tempBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
           // tempBullet.transform.SetParent(gameObject.transform);
            //Creates a spread for each coordinate.
            float spreadX = Random.Range(-spread, spread);
            float spreadY = Random.Range(-spread, spread);
            float spreadZ = Random.Range(-spread, spread);

            tempBullet.transform.Rotate(spreadX, spreadY, spreadZ);
            Bullet b = tempBullet.GetComponent<Bullet>();
            b.player = gameObject.transform.root.gameObject;
            b.force = pushForce;
            b.damage = damage; // damage voor wapens, wordt meegegeven aan de bullets (vul je in in Unity editor) check prefabs
            tempBullet.GetComponent<Rigidbody>().mass = mass;
            tempBullet.GetComponent<Rigidbody>().AddForce(tempBullet.transform.forward * bulletSpeed * 100);
            tempBullet.GetComponent<Rigidbody>().useGravity = useGravity;
            tempBullet.transform.localScale = Vector3.Scale(sizeModifier, tempBullet.transform.localScale);
            tempBullet.GetComponent<Renderer>().material.color = this.color;            //Changes bullet color to the color of the player.


            //Deletes bullet after 4 seconds
            Destroy(tempBullet, decayTime);
        }

        //Depletes ammo from the player. (Bullet shot is how many bullets you shoot at once)
        currentAmmo -= bulletShot;
        base.Shoot();


    }

    /*
    private void checkAutoAim()
    {
        RaycastHit objectHit;
        Vector3 aimedPosition = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
        //Shoots a raycast to check collision
        if (Physics.SphereCast(transform.position, 20, aimedPosition, out objectHit, 50f))
        {
            target = objectHit.collider.gameObject;
            Debug.Log(target.layer);
            int layerPlayer = LayerMask.NameToLayer("Player");

            if (target.layer == layerPlayer)
            {
                autoAim = true;
            }
            else
            if (aimOffsetX != 0)
            {
                autoAim = false;
            }

        }
    }
    */

        private void AutoAim(GameObject target)
    {
        //This codes makes it so the gun auto aims towards the enemy player
        //Vector3 targetPosition = new Vector3(test.transform.position.x, test.transform.position.y, test.transform.position.z);
        aimOffsetX = -Mathf.Atan2(target.transform.position.y - transform.position.y, (target.transform.position.x + target.transform.position.z) - (transform.position.x + transform.position.z));
    }
    

    public void Reload(int ammo)
    {
        currentAmmo = ammo;
        reloading = false;
    }


    //Timer Related stuff:
    protected void TimerRunning()
    {
        if (timerRunning == true)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime * 10;
            }
            if (timeLeft <= 0)
            {
                TimerEnd();
            }
        }
    }

    protected void TimerStart(float time)
    {
        if (timerRunning == false)
        {
            timeLeft = time;
            timerRunning = true;
            timerEnded = false;
        }
    }
    protected void TimerPause()
    {
        timerRunning = false;
    }
    protected void TimerEnd()
    {
        timeLeft = 0;
        timerRunning = false;
        timerEnded = true;
    }
}
