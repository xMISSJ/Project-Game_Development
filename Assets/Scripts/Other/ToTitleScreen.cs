using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTitleScreen : MonoBehaviour {
    //public string levelToLoad;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    /* public void NextScene(string scene)
     {
         SceneManager.LoadScene(scene);
     }*/

    public void SwitchScene(string levelToLoad)
    {
            //SceneManager.LoadScene("TitleScreen_JL");
            SceneManager.LoadScene(levelToLoad);

    }
}
