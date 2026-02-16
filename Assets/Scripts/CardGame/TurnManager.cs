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

        private Player whoseTurn;


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

        public void OnGameStart()
        {
            CardLogic.CardsCreatedThisGame.Clear();
            CreatureLogic.CreaturesCreatedThisGame.Clear();

            foreach (Player p in Player.Players)
            {
                p.ManaThisTurn = 0;
                p.ManaLeft = 0;
            }

            // determine who starts the game.
            int rnd = Random.Range(0, 2); // 2 is exclusive boundary
            // Debug.Log(Player.Players.Length);
            Player whoGoesFirst = Player.Players[rnd];
            // Debug.Log(whoGoesFirst);
            Player whoGoesSecond = whoGoesFirst.otherPlayer;
            // Debug.Log(whoGoesSecond);

            StartCoroutine(setHand(whoGoesFirst));
            StartCoroutine(setHand(whoGoesSecond));
            // draw 4 cards for first player and 5 for second player
            // int initDraw = 4;
            // for (int i = 0; i < initDraw; i++)
            // {
            //     // second player draws a card
            //     whoGoesSecond.DrawACard();
            //     // first player draws a card
            //     whoGoesFirst.DrawACard();
            // }

            // whoGoesSecond.DrawACard();
            new StartATurnCommand(whoGoesFirst).AddToQueue();
        }

        IEnumerator setHand(Player player)
        {
            yield return new WaitForSeconds(1f);
            // float duration = startNumberOfCards; // Czas trwania (5 sekund)
            float duration = 5f; // Czas trwania (5 sekund)
            float interval = 1f; // Interwał (1 sekunda)
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                new DrawACardCommand(player).AddToQueue();
                yield return new WaitForSeconds(interval);
                elapsedTime += interval;
            }
        }
        public void StartTurn(Player p)
        {
            whoseTurn = p;

            p.DrawCardAtTurnStart();

            timer.StartTimer();

            var tm = p.GetComponent<TurnMaker>();
            tm.OnTurnStart();

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                EndTurn();
        }

        public void EndTurn()
        {
            // stop timer
            timer.StopTimer();
            // send all commands in the end of current player`s turn
            whoseTurn.OnTurnEnd();
            new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
        }
    }
    // public class TurnManager : MonoBehaviour
    // {
    //     [SerializeField] public CardGameTimer timer;
    //
    //     // for Singleton Pattern
    //     public static TurnManager Instance;
    //
    //     private Player _whoseTurn;
    //
    //     // public void EnableEndTurnButtonOnStart(Player P)
    //     // {
    //     //     if (P == LowPlayer && CanControlThisPlayer(AreaPosition.Low) ||
    //     //         P == TopPlayer && CanControlThisPlayer(AreaPosition.Top))
    //     //         EndTurnButton.interactable = true;
    //     //     else
    //     //         EndTurnButton.interactable = false;
    //     // }
    //
    //     public Player whoseTurn
    //     {
    //         get { return _whoseTurn; }
    //
    //         set
    //         {
    //             _whoseTurn = value;
    //             timer.StartTimer();
    //
    //             // EnableEndTurnButtonOnStart(_whoseTurn);
    //
    //             TurnMaker tm = whoseTurn.GetComponent<TurnMaker>();
    //             // player`s method OnTurnStart() will be called in tm.OnTurnStart();
    //             tm.OnTurnStart();
    //             // if (tm is PlayerTurnMaker)
    //             // {
    //             //     whoseTurn.HighlightPlayableCards();
    //             // }
    //             //
    //             // // remove highlights for opponent.
    //             // whoseTurn.otherPlayer.HighlightPlayableCards(true);
    //         }
    //     }
    //
    //     void Awake()
    //     {
    //         Instance = this;
    //         if (timer == null)
    //         {
    //             timer = GetComponent<CardGameTimer>();
    //         }
    //
    //         timer.TimerExpired.AddListener(EndTurn);
    //     }
    //
    //     void Start()
    //     {
    //         OnGameStart();
    //     }
    //
    //     public void OnGameStart()
    //     {
    //         //Debug.Log("In TurnManager.OnGameStart()");
    //
    //         CardLogic.CardsCreatedThisGame.Clear();
    //         CreatureLogic.CreaturesCreatedThisGame.Clear();
    //
    //         foreach (Player p in Player.Players)
    //         {
    //             p.ManaThisTurn = 0;
    //             p.ManaLeft = 0;
    //             // p.TransmitInfoAboutPlayerToVisual();
    //             // p.deck.cards = p.deck.cards.Count;
    //         }
    //
    //         // Sequence s = DOTween.Sequence();
    //         // s.PrependInterval(3f);
    //         // s.OnComplete(() =>
    //         // {
    //         // determine who starts the game.
    //         int rnd = Random.Range(0, 2); // 2 is exclusive boundary
    //         // Debug.Log(Player.Players.Length);
    //         Player whoGoesFirst = Player.Players[rnd];
    //         // Debug.Log(whoGoesFirst);
    //         Player whoGoesSecond = whoGoesFirst.otherPlayer;
    //         // Debug.Log(whoGoesSecond);
    //
    //         // draw 4 cards for first player and 5 for second player
    //         int initDraw = 4;
    //         for (int i = 0; i < initDraw; i++)
    //         {
    //             // second player draws a card
    //             whoGoesSecond.DrawACard();
    //             // first player draws a card
    //             whoGoesFirst.DrawACard();
    //         }
    //
    //         // add one more card to second player`s hand
    //         whoGoesSecond.DrawACard();
    //         //new GivePlayerACoinCommand(null, whoGoesSecond).AddToQueue();
    //         // whoGoesSecond.DrawACoin();
    //         new StartATurnCommand(whoGoesFirst).AddToQueue();
    //         // });
    //     }
    //
    //     public void StartTurn(Player p)
    //     {
    //         _whoseTurn = p;
    //
    //         p.DrawCardAtTurnStart();
    //
    //         timer.StartTimer();
    //         // EnableEndTurnButtonOnStart(p);
    //
    //         var tm = p.GetComponent<TurnMaker>();
    //         tm.OnTurnStart();
    //
    //         // if (tm is PlayerTurnMaker)
    //         //     p.HighlightPlayableCards();
    //         //
    //         // p.otherPlayer.HighlightPlayableCards(true);
    //     }
    //
    //     void Update()
    //     {
    //         if (Input.GetKeyDown(KeyCode.Space))
    //             EndTurn();
    //     }
    //
    //     public void EndTurn()
    //     {
    //         // stop timer
    //         timer.StopTimer();
    //         // send all commands in the end of current player`s turn
    //         whoseTurn.OnTurnEnd();
    //         new StartATurnCommand(whoseTurn.otherPlayer).AddToQueue();
    //     }
    //
    //     public void StopTheTimer()
    //     {
    //         timer.StopTimer();
    //     }
    // }
}