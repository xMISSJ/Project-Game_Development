using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterHead : MonoBehaviour {
    // vanuit deze class aanroepen dat de controls ge-invert moeten worden
    [SerializeField] protected float addedMass;      //Mass of the head. This is added to the totalmass in the masterlegs.
                                                     // Use this for initialization
    protected void Start () {
        GetComponentInParent<Body>().invertControls = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public float AddedMass()    //This is a getter, so that masterbody can acces the value of addedmass
    {
        return addedMass;
    }

}
