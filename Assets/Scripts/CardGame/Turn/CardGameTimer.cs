using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Cards
{
    public class CardGameTimer : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private float timeForOneTurn = 30f;

        [Header("UI")] [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI PopUpText;

        [Header("Events")] public UnityEvent TimerExpired;


        private float timeRemaining;
        private bool isRunning;

        private void Awake()
        {
            UpdateTimerText(timeForOneTurn);
        }

        public void whoseTurnPopUp(Player player)
        {
            StopAllCoroutines(); // zatrzymaj poprzedni popup
            PopUpText.DOKill(); // zabij stare tweens
            StartCoroutine(ShowTurnPopupRoutine(player));
        }

        private IEnumerator ShowTurnPopupRoutine(Player player)
        {
            PopUpText.gameObject.SetActive(true);

            //bardzo ważne: reset stanu
            PopUpText.alpha = 1f;
            PopUpText.transform.localScale = Vector3.one;
            PopUpText.text = $"{player.name}'s turn";
            //  ustaw kolor (z zachowaniem alpha = 1)
            Color baseColor = player.color;
            baseColor.a = 1f;
            PopUpText.color = baseColor;
            // Start animacji od małej skali
            PopUpText.transform.localScale = Vector3.zero;

            // Scale in
            PopUpText.transform
                .DOScale(1f, 0.4f)
                .SetEase(Ease.OutBack);

            yield return new WaitForSeconds(1.2f);

            // Fade out
            yield return PopUpText
                .DOFade(0f, 0.4f)
                .WaitForCompletion();

            PopUpText.gameObject.SetActive(false);

            //przywróć alpha na przyszłość
            PopUpText.alpha = 1f;
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
            timerText.text = "Time left: " + seconds.ToString();
        }
    }
}