using System;
using System.Collections.Generic;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    public class ExperimentTableView : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private ExperimentResultRow _prefabRow;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Transform _contentContainer;
        
        public event Action SaveCsvRequested;

        private void Start()
        {
            Hide();
        }

        public void Show(IExperimentParameter parameter, IReadOnlyList<ExperimentRunResult> results)
        {
            _root.SetActive(true);

            foreach (Transform child in _contentContainer)
                Destroy(child.gameObject);
            
            var header = Instantiate(_prefabRow, _contentContainer);
            header.SetupHeader(parameter);

            foreach (var result in results)
            {
                var row = Instantiate(_prefabRow, _contentContainer);
                row.SetupData(result);
            }
            
            var btn = Instantiate(_saveButton, _contentContainer);
            btn.onClick.AddListener(() => SaveCsvRequested?.Invoke());
        }

        public void Hide()
        {
            _root.SetActive(false);
        }
    }
}