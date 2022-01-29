using System;
using System.Collections.Generic;
using System.Linq;
using Behaviours.Exploration;
using Models;
using UnityEngine;

namespace Controllers
{
    public class ExplorationPhaseController : Singleton<ExplorationPhaseController>
    {
        public static event Action ExplorationPhaseEnded;
        
        [SerializeField] private WorldBehaviour _world;
        [SerializeField] private GameTextDisplay _textDisplay;
        private Conflict _currentConflict;
        
        public void BeginExplorationPhase()
        {
            _world.EnableWorld();
            WorldBehaviour.KingdomSelected += OnKingdomSelected;
            _world.ExplorationPhaseEnded += OnExplorationPhaseEnded;

            _currentConflict = StageController.GetCurrentConflict();
        }

        public void EndExplorationPhase()
        {
            _world.DisableWorld();
            WorldBehaviour.KingdomSelected -= OnKingdomSelected;
            _world.ExplorationPhaseEnded -= OnExplorationPhaseEnded;
        }

        private void OnExplorationPhaseEnded()
        {
            ExplorationPhaseEnded?.Invoke();
            EndExplorationPhase();
        }

        private void OnKingdomSelected(Faction faction)
        {
            var flag = StageController.GetCurrentConflict().regionalTips.FirstOrDefault(rt => rt.faction == faction)?.unlockingFlag;
            if (!string.IsNullOrWhiteSpace(flag))
            {
                GlobalFlagsController.SetFlag(flag);
            }

            _world.CanSelectRegion = false;
            DisplayTips(faction);

            _textDisplay.Closed += () =>
            {
                _world.CanSelectRegion = true;
            };
        }

        private void DisplayTips(Faction faction)
        {
            _world.TweenInCharacters(faction);
            List<LocalizedText> tips = GetRegionTip(faction);
            if (tips.Count == 0)
            {
                tips.Add(AssetsController.Instance.GetDefaultRegionalTips());
            }
            
            int _dialogueIndex = 0;
            ShowDialogue();
            
            void ShowDialogue()
            {
                var index = _dialogueIndex;

                _textDisplay.DisplayText(tips[index]);
                _textDisplay.Next = () =>
                {
                    if (index < tips.Count - 1)
                    {
                        ShowDialogue();
                    }
                    else
                    {
                        OnDialoguesFinished();
                    }
                };
                    
                _dialogueIndex++;
            }        
        }

        private void OnDialoguesFinished()
        {
            _textDisplay.Close();
            _world.TweenOutCharacters();

            _world.ShowButton();
        }

        private List<LocalizedText> GetRegionTip(Faction faction)
        {
            List<LocalizedText> tipList = new List<LocalizedText>();
            foreach (OptionalDialogueTips regionalTip in _currentConflict.regionalTips)
            {
                if (regionalTip.faction == faction)
                {
                    tipList.Add(regionalTip.line);
                }
            }

            return tipList;
        }
    }
}
