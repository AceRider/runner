﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Runner.Utils;
using Runner.Common;

//Controller responsible to all behavior of the player
public class PlayerController : MonoBehaviour
{
    #region Public properties
    [Header("PlayerAttack")]
    public GameObject attack;
    public Transform attackStartPos;

    [Header("PlayerLives")]
    public Texture aliveIcon;
    public Texture deadIcon;
    public RawImage[] icons;

    [Header("PlayerScore")]
    public GameObject gameOverPanel;
    public Text highScore;

    [Header("PlayerControl")]
    [Range(1.0f, 5.0f)]
    public float swipeDistance = 1f;
    #endregion
    
    #region Public static properties
    public static GameObject player;
    public static GameObject currentPlatform;
    public static AudioSource[] soundEffect;
    public static bool isDead;
    #endregion
    
    #region Private properties
    private int livesLeft;
    private Animator playerAnim;
    private bool canTurn = false;
    private Vector3 startPosition;
    private Rigidbody playerRigidbody;
    private Rigidbody attackRigidbody;
    private string highScoreText;
    #endregion

    #region String properties
    private const string highScoreStr = "High Score: ";
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        startPosition = player.transform.position;

        playerAnim = this.GetComponent<Animator>();
        playerRigidbody = this.GetComponent<Rigidbody>();
        attackRigidbody = attack.GetComponent<Rigidbody>();
        
        soundEffect = GameObject.FindWithTag("gamedata").GetComponentsInChildren<AudioSource>();
        
        //to create the first platform attached
        GenerateWorld.RunDummy();

        GetUpdatedHighScore();
        GetUpdatedLives();
    }

    #region Collision & Trigger
    //The collision to check when the character dies.
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Fire" || collision.gameObject.tag == "Wall") && !isDead) 
        {
            playerAnim.SetTrigger("isDead");
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
                SetUpdatedHighScore();
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

        if (other is CapsuleCollider)
        {
            canTurn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other is CapsuleCollider)
            canTurn = false;
    }
    #endregion
    #region Updates methods
    //Get the lives for the player
    private void GetUpdatedLives()
    {
        isDead = false;
        livesLeft = PlayerPrefs.GetInt("lives");
        for (int i = 0; i < icons.Length; i++)
        {
            if (i >= livesLeft)
                icons[i].texture = deadIcon;
        }
    }
    //Get the HighScore in PlayerPrefs
    private void GetUpdatedHighScore()
    {
        if (PlayerPrefs.HasKey("highscore"))
        {
            highScore.text = highScoreStr + PlayerPrefs.GetInt("highscore");
        }
        else
        {
            highScore.text = highScoreStr + "0";
        }
    }

    //Update the HighScore in PlayerPrefs
    private void SetUpdatedHighScore()
    {
        PlayerPrefs.SetInt("lastscore", PlayerPrefs.GetInt("score"));
        if (PlayerPrefs.HasKey("highscore"))
        {
            int hs = PlayerPrefs.GetInt("highscore");
            if (hs < PlayerPrefs.GetInt("score"))
                PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("score"));
        }
        else
        {
            PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("score"));
        }
    }
    #endregion
    #region Invoke methods
    //Call the function for player attack
    void HalfMoonPunch()
    {
        //shot the attack from the right hand of the character
        attack.transform.position = attackStartPos.position;
        attack.SetActive(true);
        attackRigidbody.AddForce(this.transform.forward * 4000);
        //deactivate the sphere to be used again
        Invoke("KillAttack", 1);
    }
    //Stop the attack
    void KillAttack()
    {
        attack.SetActive(false);
    }
    void StopJump()
    {
        playerAnim.SetBool("isJumping", false);
    }

    void StopMagic()
    {
        playerAnim.SetBool("isMagic", false);
    }
    void RestartGame()
    {
        RunnerUtils.OpenScene(RunnerSceneType.Game);
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.isDead) return;
        if(SwipeInput.Instance.SwipeUp && playerAnim.GetBool("isMagic") == false)
        {
            playerAnim.SetBool("isJumping",true);
            //Set the rigidbody up along with the animation
            playerRigidbody.AddForce(Vector3.up * 200);
        }
        else if (SwipeInput.Instance.DoubleTap && playerAnim.GetBool("isJumping") == false)
        {
            playerAnim.SetBool("isMagic", true);
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
            this.transform.Translate(swipeDistance, 0, 0);
        }
        else if (SwipeInput.Instance.SwipeLeft)
        {
            this.transform.Translate(-swipeDistance, 0, 0);
        }
    }

}
