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


    // public void GivePlayerARandomCardCenterThenTarget(
    //     Player player,
    //     Action onComplete = null)
    // {
    //     CardAsset c = deck.GetComponent<Deck>().getRandomCardFromDeck();
    //     GameObject card = CreateACardAtPosition(c, deck.transform.position, new Vector3(0f, -179f, 0f));
    //     foreach (Transform t in card.GetComponentsInChildren<Transform>())
    //         t.tag = owner.ToString() + "Card";
    //     Sequence s = DOTween.Sequence();
    //
    //     float moveTime = GlobalSettings.Instance.CardTransitionTimeFast;
    //     float stayInCenterTime = 0.35f;
    //
    //     // card.transform.SetParent(player.transform, worldPositionStays: true);
    //
    //     // s.Append(card.transform.DOLocalMove(Vector3.zero, moveTime));
    //     // s.Append(card.transform.DORotate(Vector3.zero, moveTime / 2));
    //     // if (stayInCenterTime > 0f)
    //     //     s.AppendInterval(stayInCenterTime);
    //     // // 3) Flip z powrotem
    //     // s.Append(card.transform.DORotate(Vector3.zero, moveTime / 2));
    //     // 4) Center -> Target (wraca Z)
    //     // card.transform.SetParent(player.PArea.ManaPool.transform, worldPositionStays: true);
    //     // s.Append(card.transform.DOLocalMove(Vector3.zero, moveTime));
    //     // 1) Ustaw parent na "center anchor" (u Ciebie: player.transform) i leć do środka
    //
    //     
    //     card.transform.SetParent(player.transform, worldPositionStays: true);
    //
    //     s.Append(card.transform.DOLocalMove(Vector3.zero, moveTime));
    //     s.Join(card.transform.DORotate(Vector3.zero, moveTime / 2f));
    //
    //     // 2) Pauza w centrum
    //     if (stayInCenterTime > 0f)
    //         s.AppendInterval(stayInCenterTime);
    //
    //     // 3) (opcjonalny) flip z powrotem - ale u Ciebie rotujesz do Vector3.zero drugi raz,
    //     // więc jeśli chcesz "flip back", to musisz rotować do innej wartości (np. -179).
    //     // Zostawiam jak masz:
    //     s.Append(card.transform.DORotate(Vector3.zero, moveTime / 2f));
    //
    //     // 4) DOPIERO TERAZ zmień parent na ManaPool (w trakcie sekwencji)
    //     s.AppendCallback(() =>
    //     {
    //         card.transform.SetParent(player.PArea.ManaPool.transform, worldPositionStays: true);
    //     });
    //
    //     // 5) I dopiero po re-parent leć do local zero w Manapoolu
    //     s.Append(card.transform.DOLocalMove(Vector3.zero, moveTime));
    //
    //     
    //
    //     s.OnComplete(() => onComplete?.Invoke());
    // }
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
}