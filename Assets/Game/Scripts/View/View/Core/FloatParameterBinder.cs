using System;
using System.Globalization;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class FloatParameterBinder : IDisposable
    {
        private readonly ParameterView _view;
        private readonly string _format;
        private readonly Action<float> _setter;
        private readonly Func<float> _getter;
        
        private float _min;
        private float _max;
        private string _unit;
        private Action _onChanged;
        
        public FloatParameterBinder(
            ParameterView view,
            float min,
            float max,
            string format,
            Func<float> getter,
            Action<float> setter,
            Action onChanged)
        {

            _view = view;
            _min = min;
            _max = max;
            _format = format;

            _getter = getter;
            _setter = setter;
            _onChanged = onChanged;

            _view.Slider.minValue = min;
            _view.Slider.maxValue = max;
            _unit = _view.Unit;

            _view.Slider.onValueChanged.AddListener(OnSliderChanged);
            _view.InputField.onEndEdit.AddListener(OnInputChanged);

            Apply(_getter());
        }

        private void OnSliderChanged(float value)
        {
            Apply(value);
        }

        private void OnInputChanged(string text)
        {
            text = text.Replace(',', '.');

            if (!float.TryParse(
                    text,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out float value))
            {
                value = _getter();
            }

            Apply(Mathf.Clamp(value, _min, _max));
        }

        private void Apply(float value)
        {
            _setter(value);

            _view.Slider.SetValueWithoutNotify(value);
            _view.InputField.SetTextWithoutNotify($"{value.ToString(_format)} {_unit}");

            _onChanged?.Invoke();
        }
        
        public void UpdateBounds(float newMin, float newMax, string newUnit, float? newValueToApply = null)
        {
            _min = newMin;
            _max = newMax;
            _unit = newUnit;
        
            _view.Slider.minValue = newMin;
            _view.Slider.maxValue = newMax;
        
            if (newValueToApply.HasValue)
            {
                Apply(Mathf.Clamp(newValueToApply.Value, _min, _max));
            }
            else
            {
                Apply(Mathf.Clamp(_getter(), _min, _max));
            }
        }

        public void Dispose()
        {
            _view.Slider.onValueChanged.RemoveListener(OnSliderChanged);
            _view.InputField.onEndEdit.RemoveListener(OnInputChanged);
        }
    }
}