using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MasterArm {


    private GameObject rotatingPoint;

    public float maxRotation = 90;
    public float normal = 0; // default camera view
    public float smooth = 20;
    private Transform swordTransform;

    private bool attack = false; // define whether we're zoomed in or not

    new void Start()
    {
        rotatingPoint = transform.GetChild(0).gameObject;
    }

    void OnTriggerEnter(Collider enemy)         //CHecks if hitting enemy
    {
        if (enemy.gameObject.tag.Equals("Enemy"))
        { 
        enemy.enabled = false;
        }
    }



    public override void Fire()
    {
        attack = true;
    }

    void Update()
    {
        if (attack == true)
        {
            // moves the sword horizontally
            rotatingPoint.transform.localEulerAngles = new Vector3(rotatingPoint.transform.localRotation.x, 90, Mathf.Lerp(rotatingPoint.transform.localEulerAngles.z, maxRotation , Time.deltaTime * smooth));
            attack = false;
            //gameObject.transform.eulerAngles = new Vector3(Mathf.Asin(rotatingPoint.transform.rotation.x) * 180 / Mathf.PI , 360, Mathf.Lerp(gameObject.transform.eulerAngles.z, maxRotation, Time.deltaTime * smooth));
        }
        else
        {   // puts the sword back to its original position
            rotatingPoint.transform.localEulerAngles = new Vector3(rotatingPoint.transform.localRotation.x, 90, Mathf.Lerp(rotatingPoint.transform.localEulerAngles.z, normal, Time.deltaTime * smooth));
           }

    }
}
