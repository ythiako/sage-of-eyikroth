using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behavoiurs
{
    public class DecisionBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private Button button;

        private Decision _decision;

        private void OnEnable()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }

        public void Set(Decision decision)
        {
            _decision = decision;
            textMesh.text = _decision.text.GetText(LanguageController.GetCurrentCulture());
        }

        private void OnButtonClicked()
        {
            DecisionPhaseController.Instance.MakeDecision(_decision);
        }
    }
}