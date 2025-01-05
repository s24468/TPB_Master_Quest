using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards
{
    public class HoverPreview: MonoBehaviour
    {
        // PUBLIC FIELDS
        [FormerlySerializedAs("TurnThisOffWhenPreviewing")] public GameObject turnThisOffWhenPreviewing;  // if this is null, will not turn off anything 
        [FormerlySerializedAs("TargetPosition")] public Vector3 targetPosition;
        [FormerlySerializedAs("TargetScale")] public float targetScale;
        public GameObject previewGameObject;
        [FormerlySerializedAs("ActivateInAwake")] public bool activateInAwake = false;

        // PRIVATE FIELDS
        private static HoverPreview _currentlyViewing = null;

        // PROPERTIES WITH UNDERLYING PRIVATE FIELDS
        private static bool _previewsAllowed = true;
        public static bool PreviewsAllowed
        {
            get => _previewsAllowed;

            set 
            { 
                //Debug.Log("Hover Previews Allowed is now: " + value);
                _previewsAllowed= value;
                if (!_previewsAllowed)
                    StopAllPreviews();
            }
        }

        private bool _thisPreviewEnabled = false;

        private Coroutine _previewCoroutine;
        private bool ThisPreviewEnabled
        {
            get => _thisPreviewEnabled;

            set 
            { 
                _thisPreviewEnabled = value;
                if (!_thisPreviewEnabled)
                    StopThisPreview();
            }
        }

        private bool OverCollider { get; set;}
 
        void Awake()
        {
            ThisPreviewEnabled = activateInAwake;
        }
            
        void OnMouseEnter()
        {
            OverCollider = true;
            
            if (PreviewsAllowed && ThisPreviewEnabled)
            {
                // Start a coroutine to delay the preview
                _previewCoroutine = StartCoroutine(DelayedPreview());
            }
            // if (PreviewsAllowed && ThisPreviewEnabled)
            //     PreviewThisObject();
        }
        
        void OnMouseExit()
        {
            OverCollider = false;

            // if (!PreviewingSomeCard())
            //     StopAllPreviews();
            // Stop the preview coroutine if it's running
            if (_previewCoroutine != null)
            {
                StopCoroutine(_previewCoroutine);
                _previewCoroutine = null;
            }

            if (!PreviewingSomeCard())
                StopAllPreviews();
        }
        private IEnumerator DelayedPreview()
        {
            // Wait for 2 seconds
            yield return new WaitForSeconds(0.5f);

            // Ensure the mouse is still over the collider before previewing
            if (OverCollider && PreviewsAllowed && ThisPreviewEnabled)
            {
                PreviewThisObject();
            }
        }
        // OTHER METHODS
        void PreviewThisObject()
        {
            // 1) clone this card 
            // first disable the previous preview if there is one already
            StopAllPreviews();
            // 2) save this HoverPreview as current
            _currentlyViewing = this;
            // 3) enable Preview game object
            previewGameObject.SetActive(true);
            // 4) disable if we have what to disable
            if (turnThisOffWhenPreviewing!=null)
                turnThisOffWhenPreviewing.SetActive(false); 
            // 5) tween to target position
            previewGameObject.transform.localPosition = Vector3.zero;
            previewGameObject.transform.localScale = Vector3.one;

            previewGameObject.transform.DOLocalMove(targetPosition, 1f).SetEase(Ease.OutQuint);
            previewGameObject.transform.DOScale(targetScale, 1f).SetEase(Ease.OutQuint);
        }

        void StopThisPreview()
        {
            previewGameObject.SetActive(false);
            previewGameObject.transform.localScale = Vector3.one;
            previewGameObject.transform.localPosition = Vector3.zero;
            if (turnThisOffWhenPreviewing!=null)
                turnThisOffWhenPreviewing.SetActive(true); 
        }

        // STATIC METHODS
        private static void StopAllPreviews()
        {
            if (_currentlyViewing != null)
            {
                _currentlyViewing.previewGameObject.SetActive(false);
                _currentlyViewing.previewGameObject.transform.localScale = Vector3.one;
                _currentlyViewing.previewGameObject.transform.localPosition = Vector3.zero;
                if (_currentlyViewing.turnThisOffWhenPreviewing!=null)
                    _currentlyViewing.turnThisOffWhenPreviewing.SetActive(true); 
            }
         
        }

        private static bool PreviewingSomeCard()
        {
            if (!PreviewsAllowed)
                return false;

            HoverPreview[] allHoverBlowups = GameObject.FindObjectsOfType<HoverPreview>();

            foreach (HoverPreview hb in allHoverBlowups)
            {
                if (hb.OverCollider && hb.ThisPreviewEnabled)
                    return true;
            }

            return false;
        }

   
    }
}
