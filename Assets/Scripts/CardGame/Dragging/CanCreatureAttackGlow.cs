using UnityEngine;

namespace Cards.Dragging
{
    public class CanCreatureAttackGlow : MonoBehaviour
    {
        [Header("Refs")] [SerializeField] private FrameGlowHighlighter glow; // podepnij z prefaba
        [SerializeField] private float checkInterval = 0.08f;

        [Header("Colors")] [SerializeField] private Color canAttackColor = new Color(1f, 1f, 1f, 1f); // biały

        private CreatureLogic _logic;
        private float _nextCheck;
        private bool _lastOn;

        private void Awake()
        {
            if (glow == null)
                glow = GetComponentInChildren<FrameGlowHighlighter>(true);

            CacheLogic();
            Apply(false);
        }

        private void OnEnable()
        {
            // jak creature wraca z poola / włącza się później
            CacheLogic();
            _nextCheck = 0f;
        }

        private void Update()
        {
            if (Time.time < _nextCheck) return;
            _nextCheck = Time.time + checkInterval;
        
            if (_logic == null)
            {
                CacheLogic();
                if (_logic == null) return;
            }
        
            bool shouldGlow = (_logic.AttacksLeftThisTurn > 0) && !_logic.Frozen;
            // jeśli później wrócisz do ownersTurn w CreatureLogic, możesz dać:
            // bool shouldGlow = _logic.CanAttack;
        
            if (shouldGlow == _lastOn) return;
        
            Apply(shouldGlow);
        }

        private void Apply(bool on)
        {
            _lastOn = on;
            if (glow == null) return;

            if (on)
            {
                glow.SetColor(canAttackColor);
                glow.Show();
            }
            else
            {
                glow.Hide();
            }
        }

        private void CacheLogic()
        {
            var id = GetComponentInParent<IDHolder>()?.UniqueID;
            if (string.IsNullOrEmpty(id)) return;

            if (CreatureLogic.CreaturesCreatedThisGame.TryGetValue(id, out var logic))
                _logic = logic;
        }
    }
}