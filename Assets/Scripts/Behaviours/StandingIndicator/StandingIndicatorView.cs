using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.StandingIndicator
{
    public class StandingIndicatorView : MonoBehaviour
    {
        public SerializableDictionary<Faction, StandingIdicatorElement> Indicators;

        public void UpdateIndicators()
        {
            foreach (KeyValuePair<Faction, StandingIdicatorElement> indicatorElement  in Indicators)
            {
                int standing = SageStandingController.GetStanding(indicatorElement.Key);
                indicatorElement.Value.SetStanding(standing);
            }
        }
        
    }
    
}
