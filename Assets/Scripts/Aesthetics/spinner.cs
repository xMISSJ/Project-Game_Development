using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinner : MonoBehaviour {

    Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(spin());
	}

    private IEnumerator spin()
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddTorque(Vector3.up * 1000, ForceMode.Impulse);
    }
}
