using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sabelkalat
{
    enum ViewPoint
    {
        Card, Audience
    }

    public class Character : MonoBehaviour
    {
        [Header("Camera")]

        public Camera characterCamera = null;
        public Transform lookAtTarget = null;

        [Header("Cards")]

        public Card leftCard = null;
        public Card rightCard = null;


        #region Internal State

        private ViewPoint currentViewPoint = ViewPoint.Audience;
        private Card focusedCard = null;

        #endregion Internal State


        #region MonoBehaviour Callbacks

        void Awake()
        {
            ValidateState();
        }

        void Start()
        {
            ActivateViewPoint(ViewPoint.Audience);
            EnsureLookAt();
        }

        void Update()
        {
            EnsureLookAt();
        }

        #endregion MonoBehaviour Callbacks


        #region Input Handlers

        void OnToggleView()
        {
            if (currentViewPoint == ViewPoint.Card)
            {
                ActivateViewPoint(ViewPoint.Audience);
            }
            else
            {
                ActivateViewPoint(ViewPoint.Card);
            }
        }

        void OnFocusCard(InputValue inputValue)
        {
            if (currentViewPoint != ViewPoint.Card) return;
            var newFocusedCard = inputValue.Get<float>() < 0 ? leftCard : rightCard;
            if (focusedCard != null && newFocusedCard != focusedCard)
            {
                focusedCard.Unfocus();
            }
            focusedCard = newFocusedCard;
            focusedCard.Focus();
        }

        void OnConfirmCards()
        {

        }

        void OnToggleCard(InputValue inputValue)
        {

        }

        #endregion Input Handlers

        #region Behaviour

        void ActivateViewPoint(ViewPoint viewPoint)
        {
            if (currentViewPoint == viewPoint) return;
            Debug.Log($"Switching to viewpoint {viewPoint}");
            switch (viewPoint)
            {
                case ViewPoint.Audience:
                    leftCard.Hide();
                    rightCard.Hide();
                    break;
                case ViewPoint.Card:
                    leftCard.Show();
                    rightCard.Show();
                    break;
            }
            currentViewPoint = viewPoint;
            Debug.Log($"Switched to viewpoint {viewPoint}");
        }

        #endregion Behaviour

        private void ValidateState()
        {
            if (leftCard == null)
            {
                Debug.LogError("No left card assigned!");
            }
            if (rightCard == null)
            {
                Debug.LogError("No right card assigned!");
            }
        }

        private void EnsureLookAt()
        {
            if (lookAtTarget != null)
            {
                characterCamera.transform.LookAt(lookAtTarget);
            }
        }
    }

}  // namespace Sabelkalat
