using System;
using System.Collections.Generic;
using System.Numerics;
using Controllers;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Behaviours.Exploration
{
    public class WorldBehaviour : MonoBehaviour
    {
        public static event Action<Faction> KingdomSelected;
        
        [SerializeField] private List<KingdomMapElement> _kingdoms;

        private KingdomMapElement _currentKingdom;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectKingdom();
            }
            
            
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
