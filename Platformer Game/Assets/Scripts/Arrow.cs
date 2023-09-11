using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //[SerializeField] float bulletSpeed=20f;
    [SerializeField] AudioClip arrowPlatformImpactSFX;
    [SerializeField] AudioClip slimeDeathSFX;
    Rigidbody2D myRigidBody;
    PlayerMovement player;
    float xSpeed;
    SpriteRenderer mySpriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        /*mySpriteRenderer=GetComponent<SpriteRenderer>();
        myRigidBody=GetComponent<Rigidbody2D>();
        player=FindObjectOfType<PlayerMovement>();
        xSpeed=player.transform.localScale.x*bulletSpeed;*/
        if(xSpeed<0){
            mySpriteRenderer.flipX=true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //myRigidBody.velocity=new Vector2(xSpeed,0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Enemy"){
            AudioSource.PlayClipAtPoint(slimeDeathSFX,gameObject.transform.position);
            //destroys the object arrow makes contact with
            Destroy(other.gameObject);
            //destroys the arrow itself
            Destroy(gameObject);
        }
    }

    private IEnumerator OnCollisionEnter2D(Collision2D other) {
        AudioSource.PlayClipAtPoint(arrowPlatformImpactSFX,gameObject.transform.position);
        yield return new WaitForSecondsRealtime(3f);
        Destroy(gameObject);
    }
}
