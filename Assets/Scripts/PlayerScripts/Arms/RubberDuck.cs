using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubberDuck : MasterArm {
    
    public float maxTime = 0;
    float time = 0;
    public bool melt = false;
    AudioSource aS;
     new void Start()
    {
         base.Start();
        Debug.Log(aS);
        aS = GetComponent<AudioSource>();
    }

    // Use this for initialization
    public override void Fire() {

        // time bewteen ducks
        if(time > 0)
        {
            time -= Time.deltaTime;
            return;
        }
        time = maxTime;
        melt = true;

        // spawn new duck
        GameObject duck = Instantiate(gameObject, transform);
        duck.transform.position += Vector3.up;
        melt = false;

        duck.transform.parent = null;
        duck.AddComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (melt)
        {
            if (!aS.isPlaying)
            {
                aS.Play();
            }
            aS.volume = transform.localScale.x;
            //shrink duck until 0 then destroy
            transform.localScale -= Vector3.one * Time.deltaTime * 0.5f;  
            if(transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
	}
}
