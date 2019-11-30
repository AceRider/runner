using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    public static GameObject player;
    public static GameObject currentPlatform;
    bool canTurn = false;
    Vector3 startPosition;
    public static bool isDead;
    Rigidbody rb;
    public GameObject attack;
    public Transform attackStartPos;
    Rigidbody aRb;

    int livesLeft;
    public Texture aliveIcon;
    public Texture deadIcon;
    public RawImage[] icons;

    public GameObject gameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        aRb = attack.GetComponent<Rigidbody>();

        player = this.gameObject;
        startPosition = player.transform.position;
        
        //to create the first platform attached
        GenerateWorld.RunDummy();

        isDead = false;
        livesLeft = PlayerPrefs.GetInt("lives");

        for(int i = 0; i < icons.Length; i++)
        {
            if (i >= livesLeft)
                icons[i].texture = deadIcon;
        }
    }

    void HalfMoonPunch() 
    {
        //shout the attack from the right hand of the character
        attack.transform.position = attackStartPos.position;
        attack.SetActive(true);
        aRb.AddForce(this.transform.forward * 4000);
        //deactivate the sphere to be used again
        Invoke("KillAttack", 1);
    }

    void KillAttack()
    {
        attack.SetActive(false);
    }

    void RestartGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    //Check the collision to check when the character dies.
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Fire" || collision.gameObject.tag == "Wall") && !isDead) 
        {
            anim.SetTrigger("isDead");
            isDead = true;
            livesLeft--;
            PlayerPrefs.SetInt("lives", livesLeft);
            if (livesLeft > 0)
            {
                //after dies, wait a second before restart the scene
                Invoke("RestartGame", 1);
            }
            else
            {
                //no more lives left, show game over screen
                icons[0].texture = deadIcon;
                gameOverPanel.SetActive(true);

                //save last score and verify if is the highest score saved
                PlayerPrefs.SetInt("lastscore", PlayerPrefs.GetInt("score"));
                if(PlayerPrefs.HasKey("highscore"))
                {
                    int hs = PlayerPrefs.GetInt("highscore");
                    if(hs < PlayerPrefs.GetInt("score"))
                        PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("score"));
                }
                else
                {
                    PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("score"));
                }
            }
        }
        else
        {
            currentPlatform = collision.gameObject;
        }
    }

    //In a TSection platform, check if the player can turn. Other wise he only can run forward
    private void OnTriggerEnter(Collider other)
    {
        if(other is BoxCollider && GenerateWorld.lastPlatform.tag != "platformTSection")
            GenerateWorld.RunDummy();

        if (other is SphereCollider)
        {
            canTurn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other is SphereCollider)
            canTurn = false;
    }

    void StopJump() 
    {
        anim.SetBool("isJumping", false);
    }

    void StopMagic()
    {
        anim.SetBool("isMagic", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.isDead) return;
        if(SwipeInput.Instance.SwipeUp && anim.GetBool("isMagic") == false)
        {
            anim.SetBool("isJumping",true);
            //Set the rigidbody up along with the animation
            rb.AddForce(Vector3.up * 200);
        }
        else if (SwipeInput.Instance.DoubleTap && anim.GetBool("isJumping") == false)
        {
            anim.SetBool("isMagic", true);
        }
        else if (SwipeInput.Instance.SwipeRight && canTurn)
        {
            this.transform.Rotate(Vector3.up * 90);
            //update dummy to create platform after change direction
            GenerateWorld.dummyTraveller.transform.forward = -this.transform.forward;
            GenerateWorld.RunDummy();

            if(GenerateWorld.lastPlatform.tag != "platformTSection")
                GenerateWorld.RunDummy();

            this.transform.position = new Vector3(startPosition.x, this.transform.position.y,startPosition.z);
        }
        else if (SwipeInput.Instance.SwipeLeft && canTurn)
        {
            this.transform.Rotate(Vector3.up * -90);
            //update dummy to create platform after change direction
            GenerateWorld.dummyTraveller.transform.forward = -this.transform.forward;
            GenerateWorld.RunDummy();

            if (GenerateWorld.lastPlatform.tag != "platformTSection")
                GenerateWorld.RunDummy();

            this.transform.position = new Vector3(startPosition.x, this.transform.position.y, startPosition.z);
        }
        else if (SwipeInput.Instance.SwipeRight)
        {
            this.transform.Translate(-0.5f, 0, 0);
        }
        else if (SwipeInput.Instance.SwipeLeft)
        {
            this.transform.Translate(0.5f, 0, 0);
        }
    }
}
