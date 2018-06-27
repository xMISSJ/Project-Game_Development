using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

    MeshRenderer MR;
	// Use this for initialization
	void Start () {
        MR = gameObject.GetComponent<MeshRenderer>();

    }
    float Maxtime = 1;
    float time =1;
    public float FadeTime
    {
        get { return time; }
        set { Maxtime = value;
            time = value;
        }
    }
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        MR.material.color = new Color(1,1,1,(1/Maxtime)* time);
        if(time <= -0.001f)
        {
            Destroy(gameObject);
        }
	}
}
