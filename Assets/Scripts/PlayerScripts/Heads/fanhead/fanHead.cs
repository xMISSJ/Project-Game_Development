using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fanHead : MasterHead {
    public List<string> fanTags;
    public float maxAngle = 90;
    public float maxDistance = 5;
    public float speed = 1;

	// Update is called once per frame
	void Update () {
        foreach (string t in fanTags)
        {
            GameObject[] fannables = GameObject.FindGameObjectsWithTag(t);
            foreach (GameObject item in fannables)
            {
                if(Vector3.Distance(item.transform.position,transform.position)< maxDistance)
                {
                    if (Vector3.Angle(transform.rotation* Vector3.forward, item.transform.position - transform.position) < maxAngle / 2)
                    {
                        item.transform.position += (item.transform.position - transform.position).normalized * speed * Time.deltaTime;
                    }
                }

            }
        }
        
	}
}
