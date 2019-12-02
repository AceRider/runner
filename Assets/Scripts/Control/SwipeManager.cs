using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runner.Control
{
    public class SwipeManager : MonoBehaviour
    {
        #region Public properties
        [Header("PlayerControl")]
        public float swipeDistance = 0.1f;
        #endregion

        #region Private properties
        private Vector2 fingerDown;
        private Vector2 fingerUp;
        private bool swipeLeft;
        private bool swipeRight;
        private bool swipeUp;
        #endregion

        #region Public methods
        public bool SwipeLeft { get { return swipeLeft; } }
        public bool SwipeRight { get { return swipeRight; } }
        public bool SwipeUp { get { return swipeUp; } }
        #endregion

        void Update()
        {
            swipeUp = false;
            swipeRight = false;
            swipeLeft = false;

            if (Input.touchCount > 0)

                foreach (Touch touch in Input.touches)
                {
                    var positionCurrent = touch.position;
                    if (touch.phase == TouchPhase.Began)
                    {
                        fingerUp = positionCurrent;
                        fingerDown = positionCurrent;
                    }

                    if ((touch.phase == TouchPhase.Moved) || (touch.phase == TouchPhase.Ended))
                    {
                        fingerDown = positionCurrent;
                        DetectSwipe();
                    }
                }
        }

        void DetectSwipe()
        {

            var verticalValeu = VerticalMovement();
            var horizontalVAlue = HorizontalMovement();

            if (verticalValeu > swipeDistance && verticalValeu > horizontalVAlue)
            {
                if (fingerDown.y - fingerUp.y > 0)
                {
                    //textScreen.text = "UP";
                    swipeUp = true;
                }
                fingerUp = fingerDown;

            }
            else if (horizontalVAlue > swipeDistance && horizontalVAlue > verticalValeu)
            {
                if (fingerDown.x - fingerUp.x < 0)
                {
                    //textScreen.text = "RIGHT";
                    swipeRight = true;
                }
                else if (fingerDown.x - fingerUp.x > 0)
                {
                    //textScreen.text = "LEFT";
                    swipeLeft = true;
                }
                fingerUp = fingerDown;

            }
        }

        float VerticalMovement()
        {
            return Mathf.Abs(fingerDown.y - fingerUp.y);
        }

        float HorizontalMovement()
        {
            return Mathf.Abs(fingerDown.x - fingerUp.x);
        }
    }
}