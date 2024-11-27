using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public Coin coinPrefab;
    public int numberOfCoinsX;
    public int numberOfCoinsZ;
    public float coinsDistance = 2f;
    public float coinHeight = 1.35f;

    public Transform origin; // the origin from whici the coins are spawned

    void Start()
    {
        InstantiateCoins();
    }

    void InstantiateCoins()
    {
        // set the start coin position, since its a grid we get the negative half of the amount of coins
        // multiplied by the coin distance so the grid stays centered in the origin
        Vector3 startPos = origin.position + new Vector3( -(float)numberOfCoinsX * coinsDistance / 2f, coinHeight, -(float)numberOfCoinsZ * coinsDistance / 2f);
        startPos += new Vector3(coinsDistance / 2f, 0, coinsDistance / 2f); // we add half the distance to make sure the grid is centralised
        for (int x = 0; x < numberOfCoinsX; x++) // loop through x axis
        {
            for (int z = 0; z < numberOfCoinsZ; z++) // loop through z axis
            {
                Vector3 pos = startPos + new Vector3(x * coinsDistance, 0, z * coinsDistance); // add the current offset to the start position
                Coin coin = Instantiate(coinPrefab, pos, Quaternion.identity); // instantiating coin
                coin.timeOffset = Vector3.Distance(pos,transform.position) * 0.75f; // set time offset based on distance from origin for a cool wave effect
                GameManager.Instance.AddCoinToList(coin); // adding coin to the coins list in the manager script
            }
        }
    }
  
}
