using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatalogue : MonoBehaviour
{

    //Use this class to store all the possible gameobjects in one object. They are accessable by other objects using the Getters.

    [SerializeField]
    List<GameObject> allItems = new List<GameObject>();

    //CHoose target player, choose how much it deviates from the power level, and show the gotten items so far.
    public GameObject getItemObject(int targetPlayer, int range, List<GameObject> gottenItems)
    {
        GameObject cardItem = allItems[0];      //Sets card item to a default item. So it is never empty

        int maxScore = GameManager.instance.scoreCounter.HighestScore();
        if (maxScore == 0) maxScore = 1;
        int minScore = GameManager.instance.scoreCounter.LowestScore();

        //The following code takes the maxscore minuse minscore to get a scalar. Then it divides the targetplayer score by this scalar. The it multiplies it by 10 so it becomes a number between 0 and 10. Then it substracts this number from 10 because we want the player who scores higher get less powerfull weapons.
        float targetPlayerScore = GameManager.instance.scoreCounter.PlayerPoints(targetPlayer - 1);
        if (targetPlayerScore == 0) { targetPlayerScore = 1; } //We do this so we don't divide by zero in the next line.
        float itemRange = 10 - ((targetPlayerScore / (maxScore)) * 10);
        Mathf.RoundToInt(itemRange);
        List<GameObject> possibleItems = new List<GameObject>();
        if (GameManager.instance.firstRound) { itemRange = 5; } //If this is the first round, everyone gets the same tier of weapons!

        //As long as I do have a weapon I already got in my cards,or a weapon my target already has, I will keep trying to get a new one.
        bool duplicateItem = false;         //This bool says wether we found a double item, yes or no.   
        do
        {
            foreach (GameObject item in allItems)
            {
                //This script looks for an item within the powerlevel range I am looking for. And adds them to a pile of possible cards.
                if (item.GetComponent<ItemInfo>().powerLevel >= itemRange - range && item.GetComponent<ItemInfo>().powerLevel <= itemRange + range)
                {
                    //If the other player already has the item you just pulled, it counts as a duplicate.
                    for (int j = 0; j < GameManager.instance.prefabArray[targetPlayer - 1].Length; j++)
                    {
                        if (GameManager.instance.prefabArray[targetPlayer - 1][j] == item)
                        {
                            duplicateItem = true;
                            break;
                        }
                    }
                    //If the card is already in your hand, then its a duplicate.
                    for (int j = 0; j < gottenItems.Count; j++)
                    {
                        if (item == gottenItems[j])
                        {
                            duplicateItem = true;
                            break;
                        }
                    }

                    //If the found card is not a duplicate, add it to the possible items list.
                    if (duplicateItem == false) { possibleItems.Add(item); }
                    else
                    {
                        duplicateItem = false;
                    }
                }
            }
            if (possibleItems.Count == 0) { range++; }
            //As long as there are no possibleItems the player can get, then increase the range it will use to look for item.
            //(Example: Instead of searching for a card with power level 6-8 it will now search for a card with powerlevel 5-9
        } while (possibleItems.Count == 0);

        int number;
        number = Random.Range(0, possibleItems.Count);
        cardItem = possibleItems[number];
        return cardItem;

    }
    /*These Methods have not been updated to take the powerlevel in to account, and thus were commented out for now
    public GameObject getItemObject(Card.cardType requestedCardType)
    {
        List<GameObject> possibleItems = new List<GameObject>();
        foreach (GameObject item in allItems)
        {
            if (item.GetComponent<ItemInfo>().itemType == requestedCardType)
            {
                Debug.Log(item);
                possibleItems.Add(item);
            }
        }
        int number;
        number = Random.Range(0, possibleItems.Count);
        return (possibleItems[number]);
    }

    public GameObject getItemObject(string itemName)
    {
        foreach (GameObject item in allItems)
        {
            if (item.GetComponent<ItemInfo>().itemName == itemName)
            {
                Debug.Log(item);
                return (item);
            }
        }
        Debug.Log("No items found so returning default weapon");
        return (allItems[0]);
    }
    */
}
