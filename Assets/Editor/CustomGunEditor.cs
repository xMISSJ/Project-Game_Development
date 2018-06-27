using UnityEngine;
using System.Collections;
using UnityEditor;
/*
[CustomEditor(typeof(RangeWeapon))]
public class CustomGunEditor : Editor
{
    public int ammo = 30;                 //The max amount of ammo in a clip
    public float cooldown = 2f;             //The amount of time it takes before next bullet comes out of gun
    public float damage = 20f;               //The damage one bullet deals
    public float impactForce = 10f;          //The force the bullet will give to the collision object.
    public float reloadTime = 20f;           //Time it takes before gun is reloaded.
    public float bulletSpeed = 20;           //Speed at which the bullets fly
    public float spread = 0.1f;             //A small number to change the direction of the bullet. 0 if you dont want bullets to deviate.
    public int bulletShot = 5;             //How many bullets you shoot at once
    public float decayTime = 2f;             //How long it takes before the bullets despawn.
    public bool useGravity;                 //do bullets use gravity
    public float mass = 9.87f;           //Gravity of the bullets

    protected bool reloading;               //Wether the player is in the process in
    protected int currentAmmo;
    public GameObject bullet;       //Indexing bullets     
    public Rigidbody rigidBody;     //Bullets rigidBody (So we can set their speed)
    public Transform bulletSpawn;   //Place from where to spawn the bullets
    float turnSpeed = 90;    //The speed at which our player turns
    public Camera camera;
    float horizontal;
    float vertical;


    public override void OnInspectorGUI()
    {
        RangeWeapon myTarget = (RangeWeapon)target;

        myTarget.ammo = EditorGUILayout.IntField("Ammo:", myTarget.ammo);
        myTarget.cooldown = EditorGUILayout.FloatField("Cooldown:", myTarget.cooldown);
        myTarget.reloadTime = EditorGUILayout.FloatField("Reload Time:", myTarget.reloadTime);

        myTarget.damage = EditorGUILayout.FloatField("Damage:", myTarget.damage);
        myTarget.impactForce = EditorGUILayout.FloatField("Impact Force:", myTarget.impactForce);
        myTarget.bulletSpeed = EditorGUILayout.FloatField("Bullet Speed:", myTarget.bulletSpeed);
        myTarget.spread = EditorGUILayout.FloatField("Bullet Spread:", myTarget.spread);

        myTarget.bulletShot = EditorGUILayout.IntSlider("Bullets shot at once:", 1, 0, 500, myTarget.bulletShot);
        myTarget.decayTime = EditorGUILayout.FloatField("Decay time", myTarget.decayTime); 








        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}*/