using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Models;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Controllers
{
    public class ConflictController : Singleton<ConflictController>
    {
        public Action PhaseFinished;
        
        private Conflict _conflict;

        [SerializeField] private PlayableDirector director;
        [SerializeField] private Button exploreButton;

        [Title("Cameras")] 
        [SerializeField] private CinemachineVirtualCamera initialCamera;
        [SerializeField] private CinemachineVirtualCamera stageCamera;

        [Title("Characters")] 
        [SerializeField] private Transform leftCharacterRoot;
        [SerializeField] private Transform rightCharacterRoot;

        [Title("Text Displays")] 
        [SerializeField] private TextAnimatorInputBehaviour textInput;
        [SerializeField] private GameTextDisplay leftText;
        [SerializeField] private GameTextDisplay rightText;

        private FactionRepresentativeBehaviour _leftRepresentative;
        private FactionRepresentativeBehaviour _rightRepresentative;

        private bool _leftTextCompleted;
        private bool _rightTextCompleted;
        private bool _allDialoguesFinished;
        
        public void PrepareConflict(Conflict conflict)
        {
            _conflict = conflict;

            _allDialoguesFinished = false;
            _leftRepresentative = Instantiate(AssetsController.Instance.GetFactionRepresentativePrefab(_conflict.aSummary.faction), leftCharacterRoot);
            _rightRepresentative = Instantiate(AssetsController.Instance.GetFactionRepresentativePrefab(_conflict.bSummary.faction), rightCharacterRoot);

            leftText.CloseImmediate();
            rightText.CloseImmediate();
            textInput.gameObject.SetActive(false);
            exploreButton.gameObject.SetActive(false);

            stageCamera.Priority = 0;
            initialCamera.Priority = 10;
        }

        public void PlayConflict()
        {
            director.Play();
        }

        public void OnActorsBegunMoving()
        {
            stageCamera.Priority = 10;
            initialCamera.Priority = 0;

        }

        public void OnActorsInPlace()
        {
            director.Pause();
            
            var left = _conflict.descriptionLines.Where(dl => dl.faction == _conflict.aSummary.faction).ToList();
            var right = _conflict.descriptionLines.Where(dl => dl.faction == _conflict.bSummary.faction).ToList();

            ShowDialogue();
            
            
            void ShowDialogue()
            {
                _leftTextCompleted = false;
                _rightTextCompleted = false;
                
                textInput.gameObject.SetActive(true);
                
                leftText.DisplayText(left[0].line);
                rightText.DisplayText(right[0].line);
                
                left.RemoveAt(0);
                right.RemoveAt(0);

                leftText.Next = () =>
                {
                    if (left.Count > 0)
                    {
                        _leftTextCompleted = true;
                        if (_leftTextCompleted && _rightTextCompleted)
                        {
                            ShowDialogue();
                        }
                    }
                    else
                    {
                        _leftTextCompleted = true;
                    }
                };

                rightText.Next = () =>
                {
                    if (right.Count > 0)
                    {
                        _rightTextCompleted = true;
                        if (_leftTextCompleted && _rightTextCompleted)
                        {
                            ShowDialogue();
                        }
                    }
                    else
                    {
                        _rightTextCompleted = true;
                    }
                };

                if (_leftTextCompleted && _rightTextCompleted)
                {
                    OnDialoguesFinished();
                }
            }
        }

        private void OnDialoguesFinished()
        {
            if (_allDialoguesFinished) return;

            _allDialoguesFinished = true;
            
            leftText.Close();
            rightText.Close();
            
            director.Resume();
        }

        public void OnActorsLeftScene()
        {
            PhaseFinished?.Invoke();
        }

        public void Dispose()
        {
            if (_leftRepresentative) Destroy(_leftRepresentative);
            if (_rightRepresentative) Destroy(_rightRepresentative);

            gameObject.SetActive(false);
        }
    }
}