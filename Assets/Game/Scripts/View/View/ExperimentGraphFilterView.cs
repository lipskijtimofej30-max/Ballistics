using System;
using System.Collections.Generic;
using Assets.Game.Scripts.Core.Experiment;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    public class ExperimentGraphFilterView : MonoBehaviour
    {
        [SerializeField] private Button _hideButton;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private GameObject _root;
        [SerializeField] private Transform _container;
        [SerializeField] private FilterToggleItem _itemPrefab;
        
        private readonly List<FilterToggleItem> _items = new();
        
        public event Action<int, bool> ToggleChanged;
        public event Action AcceptRequested;

        private void Start()
        {
            Hide();
            _hideButton.onClick.AddListener(Hide);
            _acceptButton.onClick.AddListener(() => AcceptRequested?.Invoke());
        }

        public void Show() => _root.SetActive(true);
        public void Hide() => _root.SetActive(false);

        public void BuildList(IReadOnlyList<ExperimentRunResult> results, string unit, string parameterName)
        {
            Clear();
            foreach (var result in results)
            {
                var item = Instantiate(_itemPrefab, _container);
                
                int id = result.RunId;

                item.Label.text = $"Эксперимент {id}:\n" +
                                  $"{parameterName} {result.ParameterValue} {unit}";
                item.Toggle.SetIsOnWithoutNotify(false);
                
                item.Toggle.onValueChanged.AddListener(isOn => ToggleChanged?.Invoke(id, isOn));
                
                _items.Add(item);
            }
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                item.Toggle.onValueChanged.RemoveAllListeners();
                Destroy(item.gameObject);
            }
            _items.Clear();
        }

        private void OnDestroy()
        {
            _hideButton.onClick.RemoveAllListeners();
            _acceptButton.onClick.RemoveAllListeners();
        }
    }
}