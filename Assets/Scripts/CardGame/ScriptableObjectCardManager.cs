using System;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Cards
{
    public class ScriptableObjectCreator : MonoBehaviour
    {
        public string SavePath = "Assets/SOAssets"; // Path to save the ScriptableObjects

        private void Awake()
        {
            if (Directory.Exists(SavePath) && Directory.GetFiles(SavePath).Length == 0)
            {
                List<Card> creatureCards = CardDataLoader.Instance.GetCreatureCards();
                CreateCardAssets(creatureCards);
                List<Card> spellCards = CardDataLoader.Instance.GetSpellCards();
                CreateCardAssets(spellCards);
            }
        }

        public void CreateCardAssets(List<Card> cards)
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            foreach (var card in cards)
            {
                CardAsset cardAsset = ScriptableObject.CreateInstance<CardAsset>();

                cardAsset.Name = card.Name;
                cardAsset.CardImage = card.CardSprite;
                cardAsset.ManaCost = card.Mana;
                cardAsset.IsCreature = card.Type.ToLower() == "creature";
                cardAsset.Description = card.Description;
                cardAsset.CasualPower = card.CasualPower;
                cardAsset.TPower = card.TPower;
                cardAsset.PPower = card.PPower;
                cardAsset.BPower = card.BPower;

                // Save ScriptableObject as an asset
                string assetPath = Path.Combine(SavePath, $"{card.ID}_{card.Name}.asset");
                assetPath = assetPath.Replace("\\", "/"); // Ensure the path uses forward slashes

#if UNITY_EDITOR
                UnityEditor.AssetDatabase.CreateAsset(cardAsset, assetPath);
#endif
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif

            Debug.Log($"Created {cards.Count} CardAssets at {SavePath}.");
        }
    }
}