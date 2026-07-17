using System;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Scripts.View.View
{
    public class TelemetryPanelView : MonoBehaviour, IUiPanel
    {
        [SerializeField] private GameObject _telemetryPanel;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _pointText;

        private VelocityCalculator _velocityCalculator;
        private string _currentText;

        [Inject]
        private void Construct(VelocityCalculator velocityCalculator)
        {
            _velocityCalculator = velocityCalculator;
        }
        
        private void Awake()
        {
            Show();
            SetSimulationPoint(new SimulationPoint());
        }

        public void Show()
        {
            _titleText.text = "Телеметрия";
            _telemetryPanel.SetActive(true);
        }
        public void Hide() => _telemetryPanel.SetActive(false);

        public void SetSimulationPoint(SimulationPoint point)
        {
            _currentText = "";
            SetPoint(point);
        }
        
        public void SetExperimentPoint(int index, SimulationPoint point)
        {
            _currentText = "";
            _currentText += $"Эксперимент №{index}\n\n";
           SetPoint(point);
        }

        public void SetPointForSimulationPause(SimulationPoint point)
        {
            _currentText = "";
            SetPointForPause(point);
        }
        public void SetPointForExperimentPause(int index, SimulationPoint point)
        {
            _currentText = "";
            _currentText += $"Эксперимент №{index}\n\n";
            SetPointForPause(point);
        }
        
        private void SetPointForPause(SimulationPoint point)
        {
            _currentText += 
                $"Высота: {point.Position.y:F3} м\n\n" +
                $"Дальность: {point.Position.x:F3} м\n\n" +
                $"Скорость:\n\n" +
                $"\t Модуль: {point.Velocity.magnitude:F3} м/c\n\n" +
                $"\t Проекция X: {point.Velocity.x:F3} м/c\n\n" +
                $"\t Проекция Y: {point.Velocity.y:F3} м/c\n\n" +
                $"\t Угол: {_velocityCalculator.GetAngleForVelocity(point.Velocity):F2}°\n\n" +
                $"Ускорение: {point.Acceleration.magnitude:F3} м/с^2\n\n" +
                $"Сила: {point.TotalForce.magnitude:F2} H";
            _pointText.text = _currentText;
        }
        
        private void SetPoint(SimulationPoint point)
        {
            _currentText +=  
                $"Высота: {point.Position.y:F2} м\n\n" +
                $"Дальность: {point.Position.x:F2} м\n\n" +
                $"Скорость:\n\n" +
                $"\t Модуль: {point.Velocity.magnitude:F2} м/c\n\n" +
                $"\t Угол: {_velocityCalculator.GetAngleForVelocity(point.Velocity):F2}°\n\n" +
                $"Ускорение: {point.Acceleration.magnitude:F2} м/с^2\n\n" +
                $"Сила: {point.TotalForce.magnitude:F2} H";
            _pointText.text = _currentText;
        }
    }
}