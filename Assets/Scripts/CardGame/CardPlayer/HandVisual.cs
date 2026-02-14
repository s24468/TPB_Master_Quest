using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cards.ExtenstionMethods;
using DG.Tweening;
using UI;

public class HandVisual : MonoBehaviour
{
    // PUBLIC FIELDS
    public AreaPosition owner;
    public bool TakeCardsOpenly = true;
    public SameDistanceChildren slots;

    [Header("Transform References")] public GameObject Deck;

    private List<GameObject> CardsInHand = new List<GameObject>();

    public void AddCard(GameObject card)
    {
        CardsInHand.Insert(0, card);
        card.transform.SetParent(slots.transform);
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    private void Awake()
    {
        setDeck();
        ShufflingExtention.Shuffle(Deck.GetComponent<Deck>().cards);
    }


    public void setDeck()
    {
        var cardDictionaryCollected = DataPersistenceManager.instance.gameData.CardDictionaryCollected;

        var cards = CardDataLoader.Instance.GetCreatureCards();
        cards.AddRange(CardDataLoader.Instance.GetSpellCards());

        var deck = Deck.GetComponent<Deck>();

        foreach (var cardDic in cardDictionaryCollected)
        {
            int key = Convert.ToInt32(cardDic.Key);
            int count = cardDic.Value;

            var card = cards.FirstOrDefault(c => c.ID == key);
            if (card == null)
            {
                Debug.LogWarning($"setDeck(): Nie znaleziono karty o ID={key}");
                continue;
            }

            for (int i = 0; i < count; i++)
            {
                // TWORZYSZ NOWĄ INSTANCJĘ dla każdej kopii karty
                CardAsset cardAsset = ScriptableObject.CreateInstance<CardAsset>();

                // Unikalne ID instancji karty (każda kopia ma inne)
                // var ids = Services.Get<IInstanceIdService>();
                // cardAsset.Id = ids.NewId($"type{card.ID}");
                cardAsset.Id = card.ID.ToString();
                cardAsset.Name = card.Name;
                cardAsset.CardImage = card.CardSprite;
                cardAsset.ManaCost = card.Mana;
                cardAsset.IsCreature = card.Type.ToLower() == "creature";
                cardAsset.Description = card.Description;
                cardAsset.CasualPower = card.CasualPower;
                cardAsset.TPower = card.TPower;
                cardAsset.PPower = card.PPower;
                cardAsset.BPower = card.BPower;

                deck.cards.Add(cardAsset);
            }
        }
    }

    public void RemoveCard(GameObject card)
    {
        CardsInHand.Remove(card);
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // remove card with a given index from hand
    public void RemoveCardAtIndex(int index)
    {
        CardsInHand.RemoveAt(index);
        PlaceCardsOnNewSlots();
        UpdatePlacementOfSlots();
    }

    // get a card GameObject with a given index in hand
    public GameObject GetCardAtIndex(int index)
    {
        return CardsInHand[index];
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
        manager.cardLogic = new CardLogic(cardAsset,card.GetComponent<IDHolder>().UniqueID);
        return card;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GivePlayerARandomCard();
        }
    }

    public void GivePlayerARandomCard()
    {
        CardAsset c = getRandomCardFromDeck();
        // var ids = Services.Get<IInstanceIdService>();
        // string instanceId = ids.NewId($"type{c.Id}");
        GivePlayerACard(c);
    }

    public CardAsset getRandomCardFromDeck()
    {
        CardAsset c = Deck.GetComponent<Deck>().cards[0];
        Deck.GetComponent<Deck>().cards.RemoveAt(0);
        return c;
    }

    public void GivePlayerACard(CardAsset c, bool fast = false, bool fromDeck = true)
    {
        GameObject card;

        card = CreateACardAtPosition(c, Deck.transform.position, new Vector3(0f, -179f, 0f));

        foreach (Transform t in card.GetComponentsInChildren<Transform>())
        {
            t.tag = owner.ToString() + "Card";
        }

        AddCard(card);


        Sequence s = DOTween.Sequence();
        Vector3 targetPos = slots.children[0].transform.localPosition;

        float upTime = 1.8f;
        float moveTime = GlobalSettings.Instance.CardTransitionTimeFast;

        s.Append(card.transform.DOLocalMove(targetPos, moveTime));
        s.Append(card.transform.DORotate(Vector3.zero, moveTime / 2));
        // s.Append(card.transform.DOLocalMove(slots.children[0].transform.localPosition,
        //     GlobalSettings.Instance.CardTransitionTimeFast));
        // if (TakeCardsOpenly)
        //     s.Insert(0f, card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTimeFast));
    }
}