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
            EnqueueSetHandsInterleaved(whoGoesFirst, whoGoesSecond, 5, 1f);
        }

        private void EnqueueSetHandsInterleaved(Player a, Player b, int cards, float interval)
        {
            new DelayCommand(1f).AddToQueue();
            for (int i = 0; i < cards; i++)
            {
                new DrawACardCommand(a).AddToQueue();
                new DrawACardCommand(b).AddToQueue();
                new DelayCommand(interval).AddToQueue();
            }

            new StartATurnCommand(a).AddToQueue();
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