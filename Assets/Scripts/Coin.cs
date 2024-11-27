using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float amplitude = 1f;
    public float rotationSpeed = 50;
    Vector3 startPos;
    public float timeOffset = 0; // time offset so coins moves on different time
    bool collected  = false;
    Vector3 collectedStartPosition; // the position of the coin when collected
    float collectAnimationTime = .55f;
    float currentCollectAnimationTime = 0;
    Vector3 coinUIPosition;
    Camera cam;
    
    void Start()
    {
        startPos = transform.position;
        cam = Camera.main;
        coinUIPosition = FindObjectOfType<GameUI>().coinsIcon.position;
        coinUIPosition.z = 3f;
    }

    void Update()
    {
        if(collected)
        {
            CollectCoinAnimation();
            return;
        }
        float verticalMovement = Mathf.Sin(Time.time * moveSpeed + timeOffset) * amplitude;
        Vector3 newPos = startPos + Vector3.up * verticalMovement;
        transform.position = newPos;

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider coll)
    {
        // Check if was triggered by Player
        if (coll.gameObject.CompareTag("Player"))
        {
            // Call CollectCoin on Manager and destroy this coin
            GetComponent<Collider>().enabled = false; // disable the collider component
            collected = true;
            collectedStartPosition = transform.position;
        }
    }

    void CollectCoinAnimation()
    {
        if(currentCollectAnimationTime >= collectAnimationTime) // animation is complete, destroy the coin
        {
            GameManager.Instance.CollectCoin(this);
            Destroy(gameObject);
            return;
        }
        // get the end position of the coin, using ScreenToWorldPoint transform the coin icon ui position
        // to world position then lerp the coin position from the start position to this position
        Vector3 endPos = cam.ScreenToWorldPoint(coinUIPosition); 
       
        currentCollectAnimationTime += Time.deltaTime;
        float t = currentCollectAnimationTime / collectAnimationTime; // percentage of the animation from 0 to 1
        
        transform.position = Vector3.Slerp(collectedStartPosition, endPos, t);
       
       // scale down the coin as it gets closer to the end position
        float scale = Mathf.Lerp(1f, 0.1f, t);
        transform.localScale = new Vector3(scale,scale,scale);
    }
}
