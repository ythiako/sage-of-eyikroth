using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
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

        private int _dialogueIndex;
        private bool _allDialoguesFinished;

        private void OnEnable()
        {
            exploreButton.onClick.AddListener(OnExploreButtonClick);
        }

        private void OnDisable()
        {
            exploreButton.onClick.RemoveListener(OnExploreButtonClick);
        }

        [Button]
        public void PrepareConflict(Conflict conflict)
        {
            gameObject.SetActive(true);
            
            _conflict = conflict;

            _dialogueIndex = 0;
            _allDialoguesFinished = false;
            _leftRepresentative = Instantiate(AssetsController.Instance.GetFactionRepresentativePrefab(_conflict.aSummary.faction), leftCharacterRoot);
            _rightRepresentative = Instantiate(AssetsController.Instance.GetFactionRepresentativePrefab(_conflict.bSummary.faction), rightCharacterRoot);

            leftText.CloseImmediate();
            rightText.CloseImmediate();
            textInput.gameObject.SetActive(false);
            exploreButton.gameObject.SetActive(false);

            director.Stop();
            director.time = 0;
            stageCamera.Priority = 10;
        }

        [Button(ButtonSizes.Large)]
        public void PlayConflict()
        {
            director.Play();
        }

        public void OnActorsInPlace()
        {
            director.Pause();
            _dialogueIndex = 0;
            ShowDialogue();
            
            
            void ShowDialogue()
            {
                textInput.gameObject.SetActive(true);
                var index = _dialogueIndex;

                if (_conflict.descriptionLines[_dialogueIndex].faction == _conflict.aSummary.faction)
                {
                    var line = _conflict.descriptionLines[_dialogueIndex].line;
                    leftText.DisplayText(line);
                    
                    rightText.Next = () => { };
                    leftText.Next = () =>
                    {
                        if (index < _conflict.descriptionLines.Count - 1)
                        {
                            ShowDialogue();
                        }
                        else
                        {
                            OnDialoguesFinished();
                        }
                    };
                }
                else if (_conflict.descriptionLines[_dialogueIndex].faction == _conflict.bSummary.faction)
                {
                    var line = _conflict.descriptionLines[_dialogueIndex].line;
                    rightText.DisplayText(line);
                    
                    leftText.Next = () => { };
                    rightText.Next = () =>
                    {
                        if (index < _conflict.descriptionLines.Count - 1)   ShowDialogue();
                        else                                                OnDialoguesFinished();
                    };
                }
                
                _dialogueIndex++;
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
            exploreButton.gameObject.SetActive(true);
            exploreButton.transform.localScale = Vector3.zero;
            exploreButton.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
        }

        private void OnExploreButtonClick()
        {
            exploreButton.onClick.RemoveListener(OnExploreButtonClick);
            exploreButton.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InBack).OnComplete(() =>
            {
                PhaseFinished?.Invoke();
            });
        }

        public void Dispose()
        {
            if (_leftRepresentative)    Destroy(_leftRepresentative.gameObject);
            if (_rightRepresentative)   Destroy(_rightRepresentative.gameObject);

            gameObject.SetActive(false);
        }
    }
}