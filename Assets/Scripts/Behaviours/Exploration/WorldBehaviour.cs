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
        [SerializeField] private List<KingdomMapElement> _kingdoms;

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
                    Debug.DrawLine(hit.point, hit.point + Vector2.up, Color.red, 1f);

                }

                /*
                Ray ray = CameraController.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                
                if (hit.collider != null)
                {
                    Debug.Log(hit.point);                    
                    Debug.DrawRay(ray.origin, ray.direction, Color.blue, 1f);
                    Debug.DrawLine(hit.point, hit.point + Vector2.up, Color.red, 1f);
                }
                */
            }
        }
    }
}
