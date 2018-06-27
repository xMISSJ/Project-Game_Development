using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scoreMeter : MonoBehaviour
{
    public bool canLerp, startRoutine;
    public GameObject[] bars;                                                               //stores references to all bar gameobjects when they are declared;
    public Renderer[] barRenderers;                                                         //stores references to all bar renderers to change the color.
    public Color[] barColors = { Color.black, Color.green, Color.red, Color.yellow };       //stores all the colors for the bars, corresponding to the indices of the bars array
    public float[] barSizes;                                                                //stores the bar sizes based on scores
    public GameObject barPrefab;                                                            //stores the prefab for the cylinder
    public Sprite[] sprites;                                                                
    public int barAmount;                                                                   //the amount of bars
    public float minBarSize, maxBarSize, spacer;
    public Text playertext;

    //instantiates all bars and assigns them to this object. also extracts any needed information for easy acces later
    public void Init()
    {
        spacer = 0.05f;
        minBarSize = 0.1f;
        barAmount = 4;
        bars = new GameObject[barAmount];
        barRenderers = new Renderer[barAmount];
        barSizes = new float[barAmount];
        
        for (int i = 0; i < barAmount; i++)
        {
            bars[i] = Instantiate(barPrefab);
            bars[i].transform.SetParent(gameObject.transform);
            barRenderers[i] = bars[i].GetComponent<Renderer>();
            barRenderers[i].material.color = barColors[i];
            barSizes[i] = minBarSize;
        }
        ResizeAndMove();
    }

    // Update is called once per frame
    void Update()
    {
        if(canLerp)
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1, 1), 0.05f);
        
        if (SceneManager.GetActiveScene().name != "InbetweenRounds")
        {
            canLerp = false;
            GetComponent<TextMesh>().text = "";
        }
        else if(startRoutine){
            StartCoroutine(DelayedLerp());
            startRoutine = false;
        }
    }

    public void ResizeAndMove()
    {
        float lastYpos = 0f;
        float lastScale = 0f;

        for (int i = 0; i < barAmount; i++)
        {
            if (barSizes[i] < minBarSize) { barSizes[i] = minBarSize; }
            bars[i].transform.localScale = new Vector3(1, barSizes[i], 1);
            bars[i].transform.localPosition = new Vector3(bars[i].transform.localPosition.x
                , lastYpos + lastScale + barSizes[i] + spacer
                , bars[i].transform.localPosition.z);
            lastYpos = bars[i].transform.localPosition.y;
            lastScale = barSizes[i];
        }
    }



    private IEnumerator DelayedLerp()
    {
        yield return new WaitForSeconds(1);
        canLerp = true;
    }
}
