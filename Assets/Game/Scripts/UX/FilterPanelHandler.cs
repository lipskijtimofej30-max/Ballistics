using System;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Graphics;
using Assets.Game.Scripts.Infrastructure.GameStateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UX
{
    public class FilterPanelHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _button;
        
        private ModeController _modeController;

        [Inject]
        private void Construct(ModeController modeController)
        {
            _modeController = modeController;
        }
        
        private void Start()
        {
            _button.onClick.AddListener(TogglePanel);
            _button.gameObject.SetActive(false);
            SetInteractable(false);
        }

        private void Update()
        {
            bool isExperiment = _modeController.CurrentMode == AppMode.Experiment;
            _button.gameObject.SetActive(isExperiment);
        }
        
        public void SetInteractable(bool interactable) => _button.interactable = interactable;

        private void TogglePanel()
        {
            _panel.SetActive(!_panel.activeSelf);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}