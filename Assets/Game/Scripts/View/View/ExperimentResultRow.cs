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
        [SerializeField] private TMP_Text _maxSpeedText;
        [SerializeField] private TMP_Text _timeText;

        public void Setup(ExperimentRunResult result, IExperimentParameter parameter)
        {
            _numberText.text = result.RunId.ToString("F1");
            //_valueText.text = $"{}{parameter.Unit}";
        }
    }
}