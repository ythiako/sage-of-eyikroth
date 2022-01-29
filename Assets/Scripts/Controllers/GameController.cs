using System;
using UnityEngine;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        private void Awake()
        {
            StartNewGame();
        }

        public void StartNewGame()
        {
            GlobalFlagsController.NewFlagCollection();
        }
    }
}