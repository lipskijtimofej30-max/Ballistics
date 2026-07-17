using System;
using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
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
        private string _currentText;

        private void Awake()
        {
            _saveCsvButton.onClick.AddListener(() => SaveCsvRequested?.Invoke());
            Hide();
        } 

        public void Show()
        {
            _titleText.text = "Результаты полета";
            _root.SetActive(true);
        }
        public void Hide() => _root.SetActive(false);

        public void SetSimulationSummary(SimulationSummary summary)
        {
            _saveCsvButton.gameObject.SetActive(true);
            _currentText = "";
            SetSummary(summary);
        }

        public void SetExperimentSummary(ExperimentRunResult result, IExperimentParameter parameter)
        {
            _saveCsvButton.gameObject.SetActive(false);
            _currentText = "";
            _currentText += $"Эсперимент №{result.RunId}\n\n" +
                            $"{parameter.DisplayName}: {result.ParameterValue} {parameter.Unit}\n\n";
            SetSummary(result.Summary);
        }
        
        private void SetSummary(SimulationSummary summary)
        {
            _currentText +=
                $"Время полёта: {summary.FlightTime:F2} с\n\n" +
                $"Дальность: {summary.Range:F2} м\n\n" +
                $"Путь: {summary.TotalPath:F2} м\n\n" +
                $"Перемещение: {summary.Displacement:F2} м\n\n" +
                $"Макс. высота: {summary.MaxHeight:F3} м\n\n" +
                $"<size=100%>Время до вершины: {summary.TimeForMaxHeight:F2} с\n\n</size>" +
                $"Макс. скорость: {summary.MaxSpeed:F3} м/с";
            _summaryText.text = _currentText;
        }
    }
}