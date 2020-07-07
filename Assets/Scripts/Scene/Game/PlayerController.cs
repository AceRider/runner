using UnityEngine;
using UnityEngine.UI;
using Runner.Common;
using Runner.Control;

namespace Runner.Scene.Game
{
    //Controller responsible to all behavior of the player
    public class PlayerController : MonoBehaviour
    {
        #region Public properties
        [Header("PlayerAttack")]
        public GameObject attack;
        public Transform attackStartPos;

        [Header("PlayerLives")]
        public Texture lifeImg;
        public Texture deadImg;
        public RawImage[] livesQntyImg;

        [Header("PlayerScore")]
        public GameObject gameOverPanel;
        public Text highScore;

        [Header("PlayerControl")]
        public GameObject attackButton;
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
        private int highestScore;
        #endregion
        #region String properties
       // private const string highScoreStr = "High Score: ";
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
            PlataformGenerator.RunGenerator();

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
                    livesQntyImg[0].texture = deadImg;
                    gameOverPanel.SetActive(true);
                    attackButton.SetActive(false);

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
            if (other is BoxCollider && PlataformGenerator.recentPlatform.tag != "platformTSection")
                PlataformGenerator.RunGenerator();

            if (other is CapsuleCollider)
            {
                canTurn = true;
            }
        }
        //After a TSection platform, disable player turn
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
            for (int i = 0; i < livesQntyImg.Length; i++)
            {
                if (i >= livesLeft)
                    livesQntyImg[i].texture = deadImg;
            }
        }
        //Get the HighScore in PlayerPrefs
        private void GetUpdatedHighScore()
        {
            if (PlayerPrefs.HasKey("highscore"))
            {
                highestScore = PlayerPrefs.GetInt("highscore");
                //highScore.text = highScoreStr + highestScore;
                highScore.text = RunnerStrings.HIGH_SCORE + highestScore;
            }
            else
            {
                highestScore = 0;
                highScore.text = RunnerStrings.HIGH_SCORE + highestScore;
            }
        }

        //Update the HighScore in PlayerPrefs
        private void SetUpdatedHighScore()
        {
            GameDataManager.singleton.SetUpdatedHighScore();
        }
        #endregion
        #region Invoke methods
        //Call the function for player attack
        void HalfMoonPunch()
        {
            //shot the attack from the right hand of the character
            attack.transform.position = attackStartPos.position;
            attack.SetActive(true);
            attackRigidbody.AddForce(this.transform.forward * 3000);
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
        // Get the controls input by the user
        void Update()
        {
            if (PlayerController.isDead) return;

            #if UNITY_EDITOR
                     UpdateStandalone();
            #else
                    UpdateMobile();
            #endif
        }
        #region Editor Actions      
        private void UpdateStandalone()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerJump();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                PlayerRight();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                PlayerLeft();
            }
        }
        #endregion
        #region Mobile Actions  
        private void UpdateMobile()
        {
            if (SwipeManager.Instance.SwipeUp)
            {
                PlayerJump();
            }
            else if (SwipeManager.Instance.SwipeRight)
            {
                PlayerRight();
            }
            else if (SwipeManager.Instance.SwipeLeft)
            {
                PlayerLeft();
            }
        }
        #endregion
        #region Player Actions
        public void PlayerAttack()
        {
            //check if player is not jumping before attack  
            if (!PlayerController.isDead && playerAnim.GetBool("isJumping") == false)
            {
                playerAnim.SetBool("isMagic", true);
            }
        }
        public void PlayerJump()
        {
            //check if player is not attacking before jump
            if (playerAnim.GetBool("isMagic") == false)
            {
                playerAnim.SetBool("isJumping", true);
                //Set the rigidbody up along with the animation
                playerRigidbody.AddForce(Vector3.up * 200);
            }
        }
        public void PlayerRight()
        {
            if (canTurn)
            {
                this.transform.Rotate(Vector3.up * 90);
                PlayerTurnDirection();
            }
            else
            {
                this.transform.Translate(swipeDistance, 0, 0);
            }
        }
        public void PlayerLeft()
        {
            if (canTurn)
            {
                this.transform.Rotate(Vector3.up * -90);
                PlayerTurnDirection();
            }
            else
            {
                this.transform.Translate(-swipeDistance, 0, 0);
            }
        }
        private void PlayerTurnDirection()
        {
            //update generator to create platform after change direction
            PlataformGenerator.worldGenerator.transform.forward = -this.transform.forward;
            PlataformGenerator.RunGenerator();

            if (PlataformGenerator.recentPlatform.tag != "platformTSection")
                PlataformGenerator.RunGenerator();

            this.transform.position = new Vector3(startPosition.x, this.transform.position.y, startPosition.z);
        }
        #endregion
    }
}
