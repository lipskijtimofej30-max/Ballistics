using Assets.Game.Scripts.Core.Experiment;
using Assets.Game.Scripts.Core.Experiment.Parameter;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class ExperimentResultRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private TMP_Text _maxHeightText;
        [SerializeField] private TMP_Text _rangeText;
        [SerializeField] private TMP_Text _maxSpeedText;
        [SerializeField] private TMP_Text _timeText;

        public void SetupHeader(IExperimentParameter parameter)
        {
            _numberText.text = "№";
            _valueText.text = $"{parameter.DisplayName},{parameter.Unit}";
            _maxHeightText.text = "Макс. высота, м";
            _maxSpeedText.text = "Макс. скорость, м/с";
            _rangeText.text = "Дистанция, м";
            _timeText.text = "Время, с";
        }
        
        public void SetupData(ExperimentRunResult result)
        {
            _numberText.text = result.RunId.ToString("F0");
            _valueText.text = result.ParameterValue.ToString("F2");
            _maxHeightText.text = result.Summary.MaxHeight.ToString("F2");
            _maxSpeedText.text = result.Summary.MaxSpeed.ToString("F2");
            _rangeText.text = result.Summary.Range.ToString("F2");
            _timeText.text = result.Summary.FlightTime.ToString("F2");
        }
    }
}