using System;
using System.Linq;
using Behaviours.Exploration;
using Models;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Controllers
{
    public class ExplorationPhaseController : Singleton<ExplorationPhaseController>
    {
        public static event Action ExplorationPhaseEnded;
        
        [SerializeField] private WorldBehaviour _world;
        [SerializeField] private GameTextDisplay _textDisplay;
        private Conflict _currentConflict;
        
        [Button("begin")]
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
            var flag = StageController.GetCurrentConflict().regionalTips.First(rt => rt.faction == faction).unlockingFlag;
            if (!string.IsNullOrWhiteSpace(flag))
            {
                GlobalFlagsController.SetFlag(flag);
            }
            
            var tip = GetRegionTip(faction);
            _textDisplay.DisplayText(tip);
            _world.TweenInCharacters(faction);
        }

        private LocalizedText GetRegionTip(Faction faction)
        {
            foreach (OptionalDialogueTips regionalTip in _currentConflict.regionalTips)
            {
                if (regionalTip.faction == faction)
                {
                    return regionalTip.line;
                }
            }

            return null;
        }
    }
}
