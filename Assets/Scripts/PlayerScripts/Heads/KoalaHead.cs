using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoalaHead : MasterHead {

	// Use this for initialization
	new void Start () {
        GetComponentInParent<Body>().invertControls = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
