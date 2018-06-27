using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);        //Cause object to persist inbetween multiple scenes.
    }
}
