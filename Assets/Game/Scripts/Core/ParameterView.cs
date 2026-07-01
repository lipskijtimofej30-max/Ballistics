using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Core
{
    [Serializable]
    public class ParameterView
    {
        [field: SerializeField] public Slider Slider { get; private set; }
        [field: SerializeField] public TMP_InputField InputField { get; private set; }

        [field:SerializeField] public string Unit { get;private set; }

        public void SetValueWithoutNotify(float value)
        {
            Slider.SetValueWithoutNotify(value);
        }

        public void SetInputWithoutNotify(string text)
        {
            InputField.SetTextWithoutNotify($"{text} {Unit}");
        }
    }
}