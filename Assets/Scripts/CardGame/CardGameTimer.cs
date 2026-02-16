using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Cards
{
    public class CardGameTimer : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float timeForOneTurn = 30f;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI timerText;

        [Header("Events")]
        public UnityEvent TimerExpired;

        private float timeRemaining;
        private bool isRunning;

        private void Awake()
        {
            UpdateTimerText(timeForOneTurn);
        }

        public void StartTimer()
        {
            timeRemaining = timeForOneTurn;
            isRunning = true;
            UpdateTimerText(timeRemaining);
        }

        public void StopTimer()
        {
            isRunning = false;
        }

        private void Update()
        {
            if (!isRunning) return;

            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                isRunning = false;
                UpdateTimerText(0f);
                TimerExpired?.Invoke();
                return;
            }

            UpdateTimerText(timeRemaining);
        }

        private void UpdateTimerText(float time)
        {
            if (timerText == null) return;

            int seconds = Mathf.CeilToInt(time);
            timerText.text = seconds.ToString();
        }
    }
}