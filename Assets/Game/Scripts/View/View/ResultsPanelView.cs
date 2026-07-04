using System;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    public class ResultsPanelView : MonoBehaviour, IUiPanel
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _summaryText;
        [SerializeField] private Button _saveCsvButton;

        public event Action SaveCsvRequested;

        private void Awake() => _saveCsvButton.onClick.AddListener(() => SaveCsvRequested?.Invoke());

        public void Show()
        {
            _root.SetActive(true);
            _titleText.text = "Результаты полета";
        }
        public void Hide() => _root.SetActive(false);

        public void SetSummary(SimulationSummary summary)
        {
            _summaryText.text =
                $"Макс. высота: {summary.MaxHeight:F2} м\n\n" +
                $"Макс. скорость: {summary.MaxSpeed:F2} м/с\n\n" +
                $"Дальность: {summary.Range:F2} м\n\n" +
                $"Время полёта: {summary.FlightTime:F2} с";
        }
    }
}