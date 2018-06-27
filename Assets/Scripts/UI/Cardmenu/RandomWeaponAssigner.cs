using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeaponAssigner : MonoBehaviour {

    public GameObject[,] randomPrefabs;      //The prefabs all the players start with.
    private int offset;                      //This is so that if the object is looking for player objects, but one player doesnt exist, the for loop doesnt stop prematurely.

    [SerializeField]
    private GameObject[] legs;
    [SerializeField]
    private GameObject[] arms;
    [SerializeField]
    private GameObject[] heads;
    [SerializeField]
    private GameObject[] bodies;


    // Use this for initialization
    public void intantiate()
    {
        randomPrefabs = new GameObject[5, 5];

        for (int i = 0; i < 5; i++)
        {
            randomPrefabs[i,0] = getRandomBodyPart(Card.cardType.body);
            randomPrefabs[i, 1] = getRandomBodyPart(Card.cardType.head);
            randomPrefabs[i, 2] = getRandomBodyPart(Card.cardType.arms);
            randomPrefabs[i, 3] = getRandomBodyPart(Card.cardType.arms);
            randomPrefabs[i, 4] = getRandomBodyPart(Card.cardType.legs);
        }
    }
    
    public GameObject getRandomBodyPart(Card.cardType requestedCardType)
    {
        int number;
        switch ((int)requestedCardType)
        {
            case 0:
                number = Random.Range(0, arms.Length);
                return (arms[number]);
            case 1:
                number = Random.Range(0, legs.Length);
                return (legs[number]);
            case 2:
                number = Random.Range(0, bodies.Length);
                return (bodies[number]);
            case 3:
                number = Random.Range(0, heads.Length);
                return (heads[number]);
            default:
                number = Random.Range(0, heads.Length);
                return (arms[number]);
        }
    }
}
