using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterArm : MonoBehaviour
{
    //erft over van Timer
    //niet laten overeverven van timer als je gewoon methodes van timer nodig hebt
    //alleen overerven als je een uitbreiding of variatie van timer maakt. gaan we later fixen
    [SerializeField]
    protected float addedMass;      //Mass of the Weapon. This is added to the totalmass in the masterlegs.
    protected float aimOffsetX;                     //This variable keeps track of how high the player aims. So he can automatically aim for players above him.
    protected float aimLimit = 30f;                 //This is the max the player can aim up or down in degrees.
    protected float horizontal;
    protected float vertical;

    //For playing sound effects:
    AudioSource audioSource;
    [SerializeField]
    AudioClip ShotSound;


    public Color color;
    [HideInInspector]
    public Transform bulletSpawn;   //Place from where to spawn the bullets

    public MasterArm()
    {
        
    }

    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ShotSound;
    }

    void Update()
    {
    }

    public virtual void PointArms(float horizontal, float vertical)
    {
      
        if (vertical != 0 || horizontal != 0)
        {
            transform.parent.eulerAngles = new Vector3(0, Mathf.Atan2(vertical, horizontal) * 180 / Mathf.PI + Camera.main.transform.eulerAngles.y + 90, 0);
           // transform.localPosition = transform.rotation * transform.localPosition;
        }
        Debug.DrawRay(transform.position, this.transform.forward * 100f, Color.red);    //Draws the direction the player is looking.
    }


    public virtual void Function()
    {

    }

    public virtual void Shoot()
    {
        if (audioSource.clip != null)
        audioSource.Play();
    }

    public virtual void Fire()
    {

    }

    public float AddedMass()    //This is a getter, so that masterbody can acces the value of addedmass
    {
        return addedMass;
    }
}
