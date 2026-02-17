using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Cards
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] public CardGameTimer timer;

        // for Singleton Pattern
        public static TurnManager Instance;

        public Player whoseTurn;


        void Awake()
        {
            Instance = this;
            if (timer == null)
            {
                timer = GetComponent<CardGameTimer>();
            }

            timer.TimerExpired.AddListener(EndTurn);
        }

        void Start()
        {
            OnGameStart();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndTurn();
            }
        }

        public void OnGameStart()
        {
            CardLogic.CardsCreatedThisGame.Clear();
            CreatureLogic.CreaturesCreatedThisGame.Clear();
            // determine who starts the game.
            int rnd = Random.Range(0, 2);
            Player whoGoesFirst = Player.Players[rnd];
            Player whoGoesSecond = whoGoesFirst.otherPlayer;

            StartCoroutine(GameStartSequence(whoGoesFirst, whoGoesSecond));
        }

        private IEnumerator GameStartSequence(Player first, Player second)
        {
            bool firstDone = false;
            bool secondDone = false;

            StartCoroutine(SetHand(first, () => firstDone = true));
            StartCoroutine(SetHand(second, () => secondDone = true));

            // czekamy aż obie się skończą
            yield return new WaitUntil(() => firstDone && secondDone);

            new StartATurnCommand(first).AddToQueue();
        }

        private IEnumerator SetHand(Player player, System.Action onComplete)
        {
            yield return new WaitForSeconds(1f);

            float duration = 5f;
            float interval = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                player.DrawACard();
                yield return new WaitForSeconds(interval);
                elapsedTime += interval;
            }

            onComplete?.Invoke();
        }

        public void StartTurn(Player p)
        {
            whoseTurn = p;
            timer.whoseTurnPopUp(p);
            p.DrawACard();
            new RefillManaPoolCommand(whoseTurn).AddToQueue();
            p.SacrificeUsedThisTurn = false;
            timer.StartTimer();
            var tm = p.GetComponent<TurnMaker>();
            tm.OnTurnStart();
        }

        public void EndTurn()
        {
            timer.StopTimer();
            whoseTurn.OnTurnEnd();
            new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
        }
    }
}