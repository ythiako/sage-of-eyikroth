using UnityEngine;

namespace Behavoiurs
{
    public class FactionRepresentativeBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform dialoguePopupTransform;

        public Transform DialoguePopupTransform => dialoguePopupTransform;
    }
}