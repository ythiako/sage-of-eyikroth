using System;
using UnityEngine;

namespace Controllers
{
    public class GameController : Singleton<GameController>
    {
        private void Awake()
        {
            MainMenuBehaviour.Instance.gameObject.SetActive(true);
            DecisionPhaseController.Instance.Dispose();
        }

        public void StartNewGame()
        {
            SageStandingController.StartNew();
            GlobalFlagsController.NewFlagCollection();
        }

        public void ContinueGame()
        {
            SageStandingController.Load();
            GlobalFlagsController.LoadFlagCollection();
        }

        public void ShowCredits()
        {
            
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.ExitPlaymode();
            }
            #else
            Application.Quit();
            #endif
        }
    }
}