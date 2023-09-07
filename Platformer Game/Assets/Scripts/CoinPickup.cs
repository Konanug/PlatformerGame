using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{

    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinValue=1;
    bool collected=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Player"&&collected==false){
            collected=true;
            FindObjectOfType<GameSession>().AddScore(coinValue);
            AudioSource.PlayClipAtPoint(coinPickupSFX,Camera.main.transform.position,0.1f);
            Destroy(gameObject);
        }
        else{
            return;
        }
    }
}
