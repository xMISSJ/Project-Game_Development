using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour {

    public Mesh AttackBoxMesh;
    new public List<Collider> collider;
    bool updated = true;
	// Use this for initialization
	void Start () {
       // AttackBoxMesh = GetComponent<MeshCollider>().sharedMesh;
    }

    void OnTriggerStay(Collider col)
    {
        //if(transform.root.name != "Player1") { return; }
        if (updated)
        {

            collider = new List<Collider>();
            updated = false;
        }

        collider.Add(col);    
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        // Gizmos.DrawMesh
        Gizmos.DrawMesh(AttackBoxMesh,0, transform.position,transform.rotation,transform.localScale);
    }
    // Update is called once per frame
    void Update () {
        // if (transform.root.name != "Player1") { return; }
       
        if (updated)
        {
            collider = new List<Collider>();
        }
       
        updated = true;

    }
}
