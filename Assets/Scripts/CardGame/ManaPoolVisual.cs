using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    [ExecuteInEditMode]
    public class ManaPoolVisual : MonoBehaviour, IDropTarget
    {
        // =========================
        // HOVER (jak TableVisual)
        // =========================

        private bool cursorOverThisMana = false;
        private BoxCollider col;

        public static ManaPoolVisual HoveredManaPool { get; private set; }
        public static bool CursorOverSomeManaPool => HoveredManaPool != null;

        // =========================
        // UI
        // =========================

        [Header("UI")] [SerializeField] private Image[] crystals;
        [SerializeField] private TextMeshProUGUI progressText;

        [Header("Colors")] [SerializeField] private Color availableColor = Color.white;
        [SerializeField] private Color spentColor = Color.gray;

        // =========================
        // LOGIC
        // =========================

        [Header("Logic")] [SerializeField] public ManaPoolLogic logic = new ManaPoolLogic();


        private void Awake()
        {
            col = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            // -------- HOVER --------
            if (Application.isPlaying)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);

                bool passed = false;
                foreach (var h in hits)
                    if (h.collider == col)
                    {
                        passed = true;
                        break;
                    }

                cursorOverThisMana = passed;

                if (passed)
                    HoveredManaPool = this;
                else if (HoveredManaPool == this)
                    HoveredManaPool = null;
            }
        }

        // =========================
        // PUBLIC API
        // =========================

        public void SubtractAvailableCrystals(int value)
        {
            logic.SubtractAvailableCrystals(value);
            Refresh();
        }

        public void AddMaxCrystals(int value)
        {
            logic.AddMaxCrystals(value);
            Refresh();
        }

        public void RefillAll()
        {
            logic.RefillToMax();
            Refresh();
        }


        public void Refresh()
        {
            int max = logic.MaxCrystals;
            int available = logic.AvailableCrystals;

            for (int i = 0; i < crystals.Length; i++)
            {
                bool isActiveSlot = i < max;
                crystals[i].enabled = isActiveSlot;

                if (!isActiveSlot)
                    continue;

                crystals[i].color = (i < available)
                    ? availableColor
                    : spentColor;
            }

            if (progressText != null)
                progressText.text = $"{available}/{max}";
        }

        public bool CanAcceptDrop(DraggingActions dragged)
        {
            var manaArea = GetComponentInParent<PlayerArea>();
            if (manaArea == null)
            {
                return false;
            }

            var ownerPlayer = manaArea.owner;

            // 1) musi być ten sam gracz (tak jak masz)
            bool isSameOwner = ownerPlayer == dragged.playerOwner.PArea.owner;
            // 2) tylko raz na turę
            bool notUsedYet = !dragged.playerOwner.SacrificeUsedThisTurn;

            return isSameOwner && notUsedYet;
        }

        public void AcceptDrop(DraggingActions dragged)
        {
            dragged.playerOwner.SacrificeUsedThisTurn = true;
            new AddMaxManaCommand(dragged.playerOwner, 1).AddToQueue();
            dragged.ConsumeFromHandAndDestroy();
        }
    }
}