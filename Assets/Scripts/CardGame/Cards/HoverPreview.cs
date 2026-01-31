using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cards
{
    public class HoverPreview : MonoBehaviour
    {
        // PUBLIC FIELDS
        public GameObject turnThisOffWhenPreviewing; // if this is null, will not turn off anything 
        public Vector3 targetPosition;
        public float targetScale;
        public GameObject previewGameObject;
        public bool activateInAwake = false;

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
                _previewsAllowed = value;
                if (!_previewsAllowed)
                    StopAllPreviews();
            }
        }

        private bool _thisPreviewEnabled = false;

        private Coroutine _previewCoroutine;

        public bool ThisPreviewEnabled
        {
            get => _thisPreviewEnabled;

            set
            {
                _thisPreviewEnabled = value;
                if (!_thisPreviewEnabled)
                    StopThisPreview();
            }
        }

        private bool OverCollider { get; set; }

        void Awake()
        {
            ThisPreviewEnabled = activateInAwake;
        }

        void OnMouseEnter()
        {
            OverCollider = true;

            if (PreviewsAllowed && ThisPreviewEnabled)
            {
                _previewCoroutine = StartCoroutine(DelayedPreview());
            }
        }

        void OnMouseExit()
        {
            OverCollider = false;

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
            yield return new WaitForSeconds(0.5f);

            if (OverCollider && PreviewsAllowed && ThisPreviewEnabled)
            {
                PreviewThisObject();
            }
        }

        // OTHER METHODS
        void PreviewThisObject()
        {
            StopAllPreviews();
            _currentlyViewing = this;
            previewGameObject.SetActive(true);
            if (turnThisOffWhenPreviewing != null)
                turnThisOffWhenPreviewing.SetActive(false);
            previewGameObject.transform.localPosition = Vector3.zero;
            previewGameObject.transform.localScale = Vector3.one;
            // tween to target position
            previewGameObject.transform.DOLocalMove(targetPosition, 1f).SetEase(Ease.OutQuint);
            previewGameObject.transform.DOScale(targetScale, 1f).SetEase(Ease.OutQuint);
        }

        void StopThisPreview()
        {
            previewGameObject.SetActive(false);
            previewGameObject.transform.localScale = Vector3.one;
            previewGameObject.transform.localPosition = Vector3.zero;
            if (turnThisOffWhenPreviewing != null)
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
                if (_currentlyViewing.turnThisOffWhenPreviewing != null)
                    _currentlyViewing.turnThisOffWhenPreviewing.SetActive(true);
            }
        }

        private static bool PreviewingSomeCard()
        {
            if (!PreviewsAllowed)
                return false;

            var allHoverBlowups = GameObject.FindObjectsOfType<HoverPreview>();

            foreach (var hb in allHoverBlowups)
            {
                if (hb.OverCollider && hb.ThisPreviewEnabled)
                    return true;
            }

            return false;
        }
    }
}