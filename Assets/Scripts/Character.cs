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

        [Range(0.5f, 2.0f)]
        public float eyeHeight = 1.7f;
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
            ActivateViewPoint(ViewPoint.Audience);
            EnsureMainCameraPositionAndRotation();
        }

        void Start()
        {
        }

        void Update()
        {
            EnsureMainCameraPositionAndRotation();
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

        private void EnsureMainCameraPositionAndRotation()
        {
            if (characterCamera == null)
            {
                characterCamera = Camera.main;
            }
            characterCamera.transform.SetParent(transform);
            characterCamera.transform.SetLocalPositionAndRotation(new Vector3(0, eyeHeight, 0), Quaternion.identity);
            if (lookAtTarget != null)
            {
                characterCamera.transform.LookAt(lookAtTarget);
            }
        }
    }

}  // namespace Sabelkalat
