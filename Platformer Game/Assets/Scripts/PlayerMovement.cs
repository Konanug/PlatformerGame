using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed=10f;
    [SerializeField] float jumpSpeed=5f;
    [SerializeField] float climbSpeed=5f;
    [SerializeField] Vector2 deathKick=new Vector2(10f,10f);
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform bow;
    [SerializeField] AudioClip bowFireSFX;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCollider;
    BoxCollider2D myBoxCollider;
    float startingGravity;
    bool isAlive=true;
    public Camera cam;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody=GetComponent<Rigidbody2D>();
        myAnimator=GetComponent<Animator>();
        myCollider=GetComponent<CapsuleCollider2D>();
        startingGravity=myRigidbody.gravityScale;
        myBoxCollider=GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        ClimbingLadder();
        Die();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate(){
        Vector2 lookDir=mousePos-myRigidbody.position;
        float angle = Mathf.Atan2(lookDir.y,lookDir.x)*Mathf.Rad2Deg;
        bow.transform.rotation=Quaternion.Euler(0,0,angle);
    }

    void OnFire(InputValue value){
        if(isAlive){
            AudioSource.PlayClipAtPoint(bowFireSFX,bow.transform.position);
            GameObject arrow = Instantiate(arrowPrefab,bow.position,bow.rotation);
            Rigidbody2D arrowRb=arrow.GetComponent<Rigidbody2D>();
            arrowRb.AddForce(bow.right*20f,ForceMode2D.Impulse);
        }
        else{
            return;
        }
    }

    void OnMove(InputValue value){
        if(isAlive){
            moveInput=value.Get<Vector2>();
            Debug.Log(moveInput);
        }
    }

    void OnJump(InputValue value){
        if(value.isPressed&&myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))&&isAlive){
            myRigidbody.velocity+=new Vector2(0f,jumpSpeed);
        }
    }

    void Run(){
        Vector2 playerVelocity=new Vector2(moveInput.x*runSpeed,myRigidbody.velocity.y);
        myRigidbody.velocity=playerVelocity;
        bool playerHasHorizontalSpeed=Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon;
        myAnimator.SetBool("isRunning",playerHasHorizontalSpeed);
    }

    void FlipSprite(){
        bool playerHasHorizontalSpeed=Mathf.Abs(myRigidbody.velocity.x)>Mathf.Epsilon;
        if(playerHasHorizontalSpeed){
            transform.localScale=new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);
        }
    }

    void ClimbingLadder(){
        bool playerIsClimbing=Mathf.Abs(myRigidbody.velocity.y)>Mathf.Epsilon;
        if(myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            Vector2 climbVelocity=new Vector2(myRigidbody.velocity.x,moveInput.y*climbSpeed);
            myRigidbody.velocity=climbVelocity;
            myRigidbody.gravityScale=0f;
                myAnimator.SetBool("isClimbing",playerIsClimbing);
        }
        else{
            myRigidbody.gravityScale=startingGravity;
            myAnimator.SetBool("isClimbing",false);
        }
    }

    void Die(){
        if(myCollider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards"))){
            isAlive=false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity=deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
