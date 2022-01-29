using System;
using System.Collections.Generic;
using System.Numerics;
using Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Behaviours.Exploration
{
    public class WorldBehaviour : MonoBehaviour
    {
        public static event Action<Faction> KingdomSelected;
        public event Action ExplorationPhaseEnded;

        [SerializeField] private Transform _container;
        [SerializeField] private List<KingdomMapElement> _kingdoms;
        [SerializeField] private SpriteRenderer _leftSprite;
        [SerializeField] private SpriteRenderer _rightSprite;

        [SerializeField] private Button _doneButton;

        private bool _explorationPhaseActive = false;
        private KingdomMapElement _currentKingdom;

        public void EnableWorld()
        {
            _container.gameObject.SetActive(true);
            _explorationPhaseActive = true;
            _doneButton.gameObject.SetActive(false);
        }

        public void DisableWorld()
        {
            _container.gameObject.SetActive(false);
            _explorationPhaseActive = false;
        }

        private void Update()
        {
            if (_explorationPhaseActive == false)
                return;
            
            if (Input.GetMouseButtonDown(0))
                SelectKingdom();
            
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;

            Vector3 screenPos = CameraController.Instance.MainCamera.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector3.zero);

            if (hit)
            {
                HighlightKingdom(hit.collider.gameObject);
            }
            else
            {
                Deselect();
            }
            
        }

        public void OnDoneButtonClicked()
        {
            ExplorationPhaseEnded?.Invoke();
        }
        private void HighlightKingdom(GameObject colliderGameObject)
        {
            KingdomMapElement mapElement = colliderGameObject.GetComponentInParent<KingdomMapElement>();

            if (mapElement == null)
            {
                Debug.LogError("Cannot find kingdom, this should not happen.");
                return;
            }

            if (_currentKingdom != null)
            {
                if (_currentKingdom._factionType == mapElement._factionType)
                {
                    return;                                                 
                }
                
                Deselect();
            }
            
            _currentKingdom = mapElement;
            mapElement.Highlight();
        }

        public void TweenInCharacters(Faction faction)
        {
            var sprite = AssetsController.Instance.GetLeaderSprite(faction);

            if (sprite != null)
                _leftSprite.sprite = sprite;

            _leftSprite.transform.DOLocalMoveX(-8.5f, 0.5f)
                .SetEase(Ease.InOutElastic);
            _rightSprite.transform.DOLocalMoveX(8.5f, 0.5f)
                .SetEase(Ease.InOutElastic);
        }

        public void TweenOutCharacters()
        {
            _leftSprite.transform.DOLocalMoveX(-12f, 0.5f)
                .SetEase(Ease.InOutElastic);
            _rightSprite.transform.DOLocalMoveX(12f, 0.5f)
                .SetEase(Ease.InOutElastic);
        }

        private void SelectKingdom()
        {
            if (_currentKingdom != null)
            {
                KingdomSelected?.Invoke(_currentKingdom._factionType);
            }
        }

        private void Deselect()
        {
            if (_currentKingdom != null)
            {
                _currentKingdom.StopHighlight();
                _currentKingdom = null;    
            }
        }
    }
}
