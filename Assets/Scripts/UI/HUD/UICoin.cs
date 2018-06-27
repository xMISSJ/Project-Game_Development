using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoin : MonoBehaviour {

    float livespan = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        livespan += Time.deltaTime;

        if (livespan > 0.4f)
        {
            Destroy(gameObject);
        }else if(livespan < 0.25f)
        {
            transform.position += new Vector3(0, 200, 0) * Time.deltaTime;
        }
        
	}
}
