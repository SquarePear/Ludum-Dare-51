using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour {
        public static GameUIManager Instance;
        
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _inventoryText;

        private void Awake() { 
                if (Instance == null)
                        Instance = this;
        }
        
        private void OnDestroy() {
                if (Instance == this)
                        Instance = null;
        }
        
        public void UpdateScore(int score) {
                _scoreText.text = score.ToString();
        }
        
        public void UpdateTimer(float time) {
                if (time <= 0)
                        time = 0;

                _timerText.text = $"{time:00.0}";
        }
        
        public void UpdateHands(int held, int max) {
                _inventoryText.text = $"{held} / {max}";
        }
}