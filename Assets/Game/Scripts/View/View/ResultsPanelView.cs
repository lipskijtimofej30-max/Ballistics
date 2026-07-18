using System;
using System.Collections.Generic;
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
            _titleText.text = "Сравнения результатов";
            _currentText = "";
            _currentText += $"Эсперимент №{result.RunId}\n\n" +
                            $"{parameter.DisplayName}: {result.ParameterValue} {parameter.Unit}\n\n";
            SetSummary(result.Summary);
        }

        public void SetSimulationComparisons(IReadOnlyList<SimulationComparisons>  comparisons)
        {
            _saveCsvButton.gameObject.SetActive(false);
            _currentText = "";
            foreach (var comparison in comparisons)
            {
                _currentText += 
                    $"{comparison.DisplayName}:\n" +
                    $"\t{comparison.PreviousValue:F2} -> {comparison.CurrentValue:F2} {comparison.Unit}\n" +
                    $"\t(Δ = {comparison.GetDifference():+0.00;-#0.00} {comparison.Unit})\n\n";
            }
            _summaryText.text = _currentText;
        }
        
        private void SetSummary(SimulationSummary summary)
        {
            _titleText.text = "Результаты полета";
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