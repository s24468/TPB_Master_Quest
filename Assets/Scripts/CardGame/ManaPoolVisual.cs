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
        // EDITOR TEST
        // =========================

        [Header("Editor test (only in edit mode)")]
        [SerializeField] private int testMaxCrystals = 5;
        [SerializeField] private int testAvailableCrystals = 3;

        // =========================
        // UI
        // =========================

        [Header("UI")]
        [SerializeField] private Image[] crystals;
        [SerializeField] private TextMeshProUGUI progressText;

        [Header("Colors")]
        [SerializeField] private Color availableColor = Color.white;
        [SerializeField] private Color spentColor = Color.gray;

        // =========================
        // LOGIC
        // =========================

        [Header("Logic")]
        [SerializeField] private ManaPoolLogic logic = new ManaPoolLogic();

        private int HardCap => crystals != null ? crystals.Length : 0;

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
                    if (h.collider == col) { passed = true; break; }

                cursorOverThisMana = passed;

                if (passed)
                    HoveredManaPool = this;
                else if (HoveredManaPool == this)
                    HoveredManaPool = null;
            }

            // -------- EDITOR PREVIEW --------
            if (Application.isEditor && !Application.isPlaying)
            {
                logic.SetState(testMaxCrystals, testAvailableCrystals, HardCap);
                Refresh();
            }
        }

        // =========================
        // PUBLIC API
        // =========================

        public void SetMax(int max)
        {
            logic.SetMax(max, HardCap);
            Refresh();
        }

        public void SetAvailable(int available)
        {
            logic.SetAvailable(available);
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
            return manaArea != null && manaArea.owner == dragged.playerOwner.PArea.owner;
        }

        public void AcceptDrop(DraggingActions dragged)
        {
            dragged.playerOwner.SacrificeCardForOneMaxMana(dragged.DraggedUniqueID);
            dragged.ConsumeFromHandAndDestroy();
        }
    }
}
