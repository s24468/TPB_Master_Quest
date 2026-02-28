using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.ExtenstionMethods;
using DG.Tweening;
using UI;
using UnityEngine.Serialization;

public class HandManager : MonoBehaviour
{
    // PUBLIC FIELDS
    public AreaPosition owner;

    // public bool TakeCardsOpenly = true;
    public SameDistanceChildren slots;

    [Header("Transform References")] public GameObject deck;

    private List<GameObject> CardsInHand = new List<GameObject>();

    public void AddCard(GameObject card)
    {
        CardsInHand.Insert(0, card);
        card.transform.SetParent(slots.transform);
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    public void RemoveCard(GameObject card)
    {
        CardsInHand.Remove(card);
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }


    void UpdatePlacementOfSlots()
    {
        float posX;
        if (CardsInHand.Count > 0)
            posX = (slots.children[0].transform.localPosition.x -
                    slots.children[CardsInHand.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;
        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
    }

    // shift all cards to their new slots
    void PlaceCardsOnNewSlots()
    {
        foreach (GameObject g in CardsInHand)
        {
            g.transform.DOLocalMoveX(slots.children[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            WhereIsTheCardOrCreature w = g.GetComponent<WhereIsTheCardOrCreature>();
            w.Slot = CardsInHand.IndexOf(g);
            w.SetHandSortingOrder();
        }
    }

    GameObject CreateACardAtPosition(CardAsset cardAsset, Vector3 position, Vector3 eulerAngles)
    {
        GameObject card;
        if (cardAsset.IsCreature)
        {
            card = Instantiate(GlobalSettings.Instance.CreatureCardPrefab, position,
                Quaternion.Euler(eulerAngles));
        }
        else
        {
            card = Instantiate(GlobalSettings.Instance.TargetedSpellCardPrefab, position,
                Quaternion.Euler(eulerAngles));
        }

        card.transform.localScale = new Vector3(8f, 8f, 1f);
        OneCardManager manager = card.GetComponent<OneCardManager>();
        manager.cardAsset = cardAsset;
        manager.ReadCardFromAsset();
        manager.cardLogic = new CardLogic(cardAsset, card.GetComponent<IDHolder>().UniqueID);
        return card;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GivePlayerARandomCard();
        }
    }

    public void GivePlayerARandomCardWithDelay(
        float delay,
        Action onComplete = null)
    {
        CardAsset c = deck.GetComponent<Deck>().getRandomCardFromDeck();
        GameObject card = CreateACardAtPosition(c, deck.transform.position, new Vector3(0f, -179f, 0f));
        foreach (Transform t in card.GetComponentsInChildren<Transform>())
        {
            t.tag = owner.ToString() + "Card";
        }

        AddCard(card);

        Vector3 targetPos = slots.children[0].transform.localPosition;
        Sequence s = DOTween.Sequence();

        float moveTime = GlobalSettings.Instance.CardTransitionTimeFast;

        s.Append(card.transform.DOLocalMove(targetPos, moveTime));
        s.Append(card.transform.DORotate(Vector3.zero, moveTime / 2));

        s.OnComplete(() => onComplete?.Invoke());
    }

    public void GivePlayerARandomCard()
    {
        CardAsset c = deck.GetComponent<Deck>().getRandomCardFromDeck();
        GivePlayerACard(c);
    }


    public void GivePlayerACard(CardAsset c)
    {
        GameObject card;

        card = CreateACardAtPosition(c, deck.transform.position, new Vector3(0f, -179f, 0f));

        foreach (Transform t in card.GetComponentsInChildren<Transform>())
        {
            t.tag = owner.ToString() + "Card";
        }

        AddCard(card);

        Sequence s = DOTween.Sequence();
        Vector3 targetPos = slots.children[0].transform.localPosition;

        var moveTime = GlobalSettings.Instance.CardTransitionTimeFast;

        s.Append(card.transform.DOLocalMove(targetPos, moveTime));
        s.Append(card.transform.DORotate(Vector3.zero, moveTime / 2));
    }

    public void GivePlayerARandomCardsCenterThenTarget(
        Player player,
        int count,
        Action onEachCardComplete,
        Action onAllComplete = null)
    {
        if (count <= 0)
        {
            onAllComplete?.Invoke();
            return;
        }

        GivePlayerARandomCardCenterThenTarget(player, () =>
        {
            onEachCardComplete?.Invoke(); // ✅ po jednej karcie
            GivePlayerARandomCardsCenterThenTarget(player, count - 1, onEachCardComplete, onAllComplete);
        });
    }

    public void GivePlayerARandomCardCenterThenTarget(
        Player player,
        Action onComplete = null)
    {
        CardAsset c = deck.GetComponent<Deck>().getRandomCardFromDeck();
        GameObject card = CreateACardAtPosition(c, deck.transform.position, new Vector3(0f, -179f, 0f));

        foreach (Transform t in card.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString() + "Card";

        float moveTime = GlobalSettings.Instance.CardTransitionTimeFast;
        float flipTime = 0.25f; // ustaw pod siebie
        float stayInCenterTime = 0.35f;

        Vector3 rotBack = new Vector3(0f, -179f, 0f);
        Vector3 rotFront = Vector3.zero;

        // Start: ustawiamy na "tył" żeby flip był widoczny
        card.transform.localEulerAngles = rotBack;

        Sequence s = DOTween.Sequence();

        // 1) do środka (anchor = player)
        card.transform.SetParent(player.transform, worldPositionStays: true);
        s.Append(card.transform.DOLocalMove(Vector3.zero, moveTime).SetEase(Ease.OutCubic));

        // 2) obrót w centrum (tył -> przód)
        s.Append(card.transform.DOLocalRotate(rotFront, flipTime).SetEase(Ease.InOutSine));

        // 3) chwila w centrum
        if (stayInCenterTime > 0f)
            s.AppendInterval(stayInCenterTime);

        // 4) obrót z powrotem (przód -> tył)
        s.Append(card.transform.DOLocalRotate(rotBack, flipTime).SetEase(Ease.InOutSine));

        // 5) zmiana parenta na ManaPool w odpowiednim momencie
        s.AppendCallback(() =>
        {
            card.transform.SetParent(player.PArea.ManaPool.transform, worldPositionStays: true);
        });

        // 6) do celu (local zero ManaPoola)
        s.Append(card.transform.DOLocalMove(Vector3.zero, moveTime).SetEase(Ease.InCubic));


        s.OnComplete(() =>
        {
            Destroy(card);
            onComplete?.Invoke();
        });
    }

    public void RemoveRandomCardsFromHandToCenterThenDestroy(
        Player player,
        int count,
        Action onEachCardComplete = null,
        Action onAllComplete = null)
    {
        if (count <= 0)
        {
            onAllComplete?.Invoke();
            return;
        }

        // jeśli ręka pusta -> kończymy
        if (CardsInHand == null || CardsInHand.Count == 0)
        {
            onAllComplete?.Invoke();
            return;
        }

        // nie próbuj usuwać więcej niż masz
        int safeCount = Mathf.Min(count, CardsInHand.Count);

        RemoveRandomCardFromHandToCenterThenDestroy(player, () =>
        {
            onEachCardComplete?.Invoke();
            RemoveRandomCardsFromHandToCenterThenDestroy(player, safeCount - 1, onEachCardComplete, onAllComplete);
        });
    }

    public void RemoveRandomCardFromHandToCenterThenDestroy(
        Player player,
        Action onComplete = null)
    {
        if (CardsInHand == null || CardsInHand.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        // 1) wybierz losową kartę z ręki
        int index = UnityEngine.Random.Range(0, CardsInHand.Count);
        GameObject card = CardsInHand[index];

        // 2) usuń z listy + przestaw resztę
        CardsInHand.RemoveAt(index);
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();

        var where = card.GetComponent<WhereIsTheCardOrCreature>();
        if (where != null)
            where.VisualState = VisualStates.Transition;

        float moveTime = GlobalSettings.Instance.CardTransitionTimeFast;

        // float flipTime = 0.25f;
        // float stayInCenterTime = 0.35f;
        // Vector3 rotBack = new Vector3(0f, -179f, 0f);
        // Vector3 rotFront = Vector3.zero;

        // card.transform.localEulerAngles = rotBack;

        card.transform.SetParent(player.transform, worldPositionStays: true);

        Sequence s = DOTween.Sequence();

        // tylko ruch do środka
        s.Append(card.transform.DOLocalMove(Vector3.zero, moveTime)
            .SetEase(Ease.OutCubic));

        /*
        // ---- OBRACANIE WYŁĄCZONE ----
        s.Append(card.transform.DOLocalRotate(rotFront, flipTime).SetEase(Ease.InOutSine));

        if (stayInCenterTime > 0f)
            s.AppendInterval(stayInCenterTime);

        s.Append(card.transform.DOLocalRotate(rotBack, flipTime).SetEase(Ease.InOutSine));
        */

        // s.OnComplete(() =>
        // {
        //     Destroy(card);
        //     onComplete?.Invoke();
        // });
        s.OnComplete(() =>
        {
            var burn = card.GetComponentInChildren<CardBurnImages>(true);

            if (burn != null)
            {
                burn.Burn(1.5f, 0.8f, () =>
                {
                    Destroy(card);
                    onComplete?.Invoke();
                });
            }
            else
            {
                Destroy(card);
                onComplete?.Invoke();
            }
        });
    }
}