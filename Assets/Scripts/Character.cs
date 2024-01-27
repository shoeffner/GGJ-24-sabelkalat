using System;
using System.ComponentModel.Design;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sabelkalat
{
    enum ViewPoint
    {
        Undefined, Card, Audience, WaitForNextRound
    }

    public class Character : MonoBehaviour
    {
        [Header("Camera")]

        public Camera characterCamera = null;
        public Transform lookAtTarget = null;

        [Header("Cards")]

        public Card leftCard = null;
        public Card rightCard = null;

        public CardDealer cardDealer = null;

        #region Internal State

        private ViewPoint currentViewPoint = ViewPoint.Undefined;
        private Card focusedCard = null;
        private PlayerInput playerInput = null;

        #endregion Internal State


        #region MonoBehaviour Callbacks

        void Awake()
        {
            ValidateState();
        }

        void Start()
        {
            GameOrganizer.Instance.OnNextRound += OnNextRound;
            playerInput = GetComponentInChildren<PlayerInput>();
            playerInput.enabled = true;
            focusedCard = leftCard;
            ActivateViewPoint(ViewPoint.Audience, true);
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
            if (newFocusedCard != focusedCard)
            {
                focusedCard.Unfocus();
            }
            focusedCard = newFocusedCard;
            focusedCard.Focus();
        }

        void OnConfirmCards()
        {
            Debug.Log("Submitting!");
            playerInput.enabled = false;
            ActivateViewPoint(ViewPoint.WaitForNextRound, true);
            cardDealer.Submit();
        }

        void OnToggleCard(InputValue inputValue)
        {
            Debug.Log("Toggle card");
            if (currentViewPoint == ViewPoint.Audience || cardDealer == null) return;
            var previous = inputValue.Get<float>() < 0;

            if (focusedCard == leftCard)
            {
                if (previous)
                {
                    cardDealer.PreviousSetup();
                }
                else
                {
                    cardDealer.NextSetup();
                }
            }
            else
            {
                if (previous)
                {
                    cardDealer.PreviousPunchline();
                }
                else
                {
                    cardDealer.NextPunchline();
                }
            }
        }

        #endregion Input Handlers

        #region Callbacks

        public void OnNextRound()
        {
            ActivateViewPoint(ViewPoint.Card, true);
            playerInput.enabled = true;
        }

        #endregion

        #region Behaviour

        void ActivateViewPoint(ViewPoint viewPoint, bool force = false)
        {
            if (currentViewPoint == viewPoint && !force) return;
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
                case ViewPoint.WaitForNextRound:
                    leftCard.Hide();
                    rightCard.Hide();
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
            if (cardDealer == null)
            {
                Debug.LogError("No card dealer assigned!");
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
