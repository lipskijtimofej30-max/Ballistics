using System;
using Game.Scripts.Core;
using Game.Scripts.Core.Simulation;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class TelemetryPanelView : MonoBehaviour, IUiPanel
    {
        [SerializeField] private GameObject _telemetryPanel;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _pointText;

        private void Awake()
        {
            Show();
            SetPoint(new SimulationPoint());
        }

        public void Show()
        {
            _titleText.text = "Телеметрия";
            _telemetryPanel.SetActive(true);
        }
        public void Hide() => _telemetryPanel.SetActive(false);

        public void SetPoint(SimulationPoint point)
        {
            _pointText.text = $"Высота: {point.Position.y:F2} м\n\n" +
                              $"Дальность: {point.Position.x:F2} м\n\n" +
                              $"Скорость: {point.Velocity.magnitude:F2} м/c\n\n" +
                              $"Ускорение: {point.Acceleration.magnitude:F2} м/с^2\n\n" +
                              $"Сила: {point.TotalForce.magnitude:F2} H";
        }
    }
}