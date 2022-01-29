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
        public static event Action KingdomSelected;
        
        [SerializeField] private List<KingdomMapElement> _kingdoms;
        private KingdomMapElement _currentKingdom;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 10;

                Vector3 screenPos = CameraController.Instance.MainCamera.ScreenToWorldPoint(mousePos);

                RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector3.zero);

                if (hit)
                {
                    Debug.Log("Hit. Name: " + hit.collider.transform.parent.name );

                    SelectKingdom(hit.collider.gameObject);
                }
            }
        }

        private void SelectKingdom(GameObject colliderGameObject)
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
                    Deselect(_currentKingdom);
                    return;
                }
                
                Deselect(_currentKingdom);
            }
            
            _currentKingdom = mapElement;
            mapElement.Highlight();
        }

        private void Deselect(KingdomMapElement currentKingdom)
        {
            currentKingdom.StopHighlight();
            _currentKingdom = null;
        }
    }
}
