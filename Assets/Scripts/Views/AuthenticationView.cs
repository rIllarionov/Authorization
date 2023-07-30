using System;
using Api;
using TMPro;
using UnityEngine;
using User;

namespace Views
{
    public class AuthenticationView : MonoBehaviour
    {
        public event Action <MonoBehaviour, UserProfile> Authorized;

        [SerializeField] 
        private TMP_InputField _usernameInput;
        [SerializeField] 
        private TMP_InputField _passwordInput;
        [SerializeField] 
        private GameObject _preloader;

        [SerializeField] 
        private GameObject _incorrectInput;

        [SerializeField] 
        private TextMeshProUGUI _textMessage;

        [SerializeField] 
        private GameObject _authorizationInput;

        public void Register()
        {
            if (!ValidateInput())
            {
                return;
            }

            ShowPreloader();
            WebApi.Instance.AuthenticationAPI.SendRegistrationRequest(_usernameInput.text, _passwordInput.text,
                OnSuccess, OnError);
        }

        public void LogIn()
        {
            if (!ValidateInput())
            {
                return;
            }

            ShowPreloader();
            WebApi.Instance.AuthenticationAPI.SendLoginRequest(_usernameInput.text, _passwordInput.text,
                OnSuccess, OnError);
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(_usernameInput.text) || string.IsNullOrEmpty(_passwordInput.text))
            {
                //Debug.LogError("Login or Password is empty.");
                
                _authorizationInput.SetActive(false);
                _incorrectInput.SetActive(true);
                _textMessage.text = "Login or Password is empty.";
                return false;
            }

            return true;
        }

        private void OnSuccess(UserProfile userProfile)
        {
            _preloader.gameObject.SetActive(false);
            Authorized?.Invoke(this, userProfile);
        }

        private void OnError(string errorMessage)
        {
            _preloader.gameObject.SetActive(false);
            _authorizationInput.SetActive(false);
            
            _incorrectInput.SetActive(true);
            _textMessage.text = errorMessage;
        }

        private void ShowPreloader()
        {
            _preloader.gameObject.SetActive(true);
        }
    }
}