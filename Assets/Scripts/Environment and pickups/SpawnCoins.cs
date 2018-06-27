using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{

    public GameObject coins;

    public Vector3 center;
    public Vector3 size;
    playerStats ps;

    public float secondsBetweenSpawn;
    public float elapsedTime = 0.0f;

    // max. amount of coin spawning
    public int maxCoin = 5;
    public int coinCount = 0;

    // Use this for initialization
    void Start()
    {
        ps = GameManager.instance.GetComponent<playerStats>();
    }

    // Update is called once per frame
    void Update()
    {

        elapsedTime += Time.deltaTime;

        if (elapsedTime > secondsBetweenSpawn)
        {
            elapsedTime = 0;

            if (coinCount < maxCoin)
            {
                CoinsSpawner();
            }
        }
    }
    // spawn the coins at a random position in the Gizmo
    public void CoinsSpawner()
    {
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        GameObject coin = Instantiate(coins, pos, Quaternion.Euler(90, 0, 0));
        coin.GetComponent<CoinCollect>().ps = ps;

        coinCount++;
    }

    // keeps the spawning object (coins) within a certain area
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }

    public IEnumerator BeforeRounStart(float countdown = 10)
    {
        float timer = countdown;

        // countdown until coins spawn
        while (timer > 0)
        {
            Debug.Log("Coins Countdown: " + timer);
            yield return new WaitForSeconds(1.0f);
            timer--;
        }
        // activates this script again
        if (timer == 0)
        {
            enabled = true;
        }
    }
}
