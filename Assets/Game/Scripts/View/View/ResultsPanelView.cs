using System;
using Assets.Game.Scripts.Core.Experiment;
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

        public void SetSummary(SimulationSummary summary)
        {
            _saveCsvButton.gameObject.SetActive(true);
            _summaryText.text =
                $"Время полёта: {summary.FlightTime:F3} с\n\n" +
                $"Макс. высота: {summary.MaxHeight:F3} м\n\n" +
                $"Макс. скорость: {summary.MaxSpeed:F3} м/с\n\n" +
                $"Дальность: {summary.Range:F3} м";
        }

        public void SetExperimentSummary(ExperimentRunResult result)
        {
            _saveCsvButton.gameObject.SetActive(false);
            _summaryText.text =
                $"Эсперимент №{result.RunId}\n\n" +
                $"Время полёта: {result.Summary.FlightTime:F3} с\n\n" +
                $"Макс. высота: {result.Summary.MaxHeight:F3} м\n\n" +
                $"Макс. скорость: {result.Summary.MaxSpeed:F3} м/с\n\n" +
                $"Дальность: {result.Summary.Range:F3} м";
        }
    }
}