using UnityEngine;

namespace Cards
{
    public class CardPlayerController : MonoBehaviour
    {
        public int PlayerID;
        public Deck deck;
        // public Hand hand;
        public Table table;
        public static CardPlayerController[] Players;


        public virtual void OnTurnStart()
        {
            // Logic for the start of the turn
        }

        public void OnTurnEnd()
        {
            // Logic for the end of the turn
        }

// STUFF THAT OUR PLAYER CAN DO

// get mana from coins or other spells
        public void GetBonusMana(int amount)
        {
            // Logic to add bonus mana
        }

// draw a single card from the deck
        public void DrawCard(bool fast = false)
        {
            // Logic to draw a card
        }

// get card NOT from deck (a token or a coin)
        public void GetCardNotFromDeck(CardAsset cardAsset)
        {
            // Logic to get a card not from the deck
        }

// 2 METHODS FOR PLAYING SPELLS
// 1st overload - takes IDs as arguments
// It is convenient to call this method from visual part
        public void PlayASpellFromHand(int spellCardUniqueID, int targetUniqueID)
        {
            // Logic to play a spell using IDs
        }

// 2nd overload - takes CardLogic and ICharacter interface
// This method is called (for example) by AI
        // public void PlayASpellFromHand(CardLogic playedCard, ICharacter target)
        // {
        //     // Logic to play a spell using objects
        // }

// METHODS TO PLAY CREATURES
        public void PlayACreatureFromHand(int uniqueID, int tablePos)
        {
            // Logic to play a creature using IDs
        }

// 2nd overload - by logic units
        // public void PlayACreatureFromHand(CardLogic playedCard, int tablePos)
        // {
        //     // Logic to play a creature using objects
        // }

        public void Die()
        {
            // Logic for player death
        }

// METHOD TO SHOW GLOW HIGHLIGHTS
        // public void HighlightPlayableCards(bool removeAllHighlights = false)
        // {
            // Logic to highlight playable cards
        // }

// use hero power - activate its effect like you've paid a spell
        public void UseHeroPower()
        {
            // Method implementation
        }

// START GAME METHODS
        public void LoadCharacterInfoFromAsset()
        {
            // Method implementation
        }

        public void TransmitInfoAboutPlayerToVisual()
        {
            // Method implementation
        }
    }
}