using System;
using System.Globalization;
using Assets.Game.Scripts.Infrastructure.Signals;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class FloatParameterBinder : IDisposable
    {
        private readonly ParameterView _view;
        private readonly float _min;
        private readonly float _max;
        private readonly string _format;
        private readonly Action<float> _setter;
        private readonly Func<float> _getter;
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
            _view.InputField.SetTextWithoutNotify($"{value.ToString(_format)} {_view.Unit}");

            _onChanged?.Invoke();
        }

        public void Dispose()
        {
            _view.Slider.onValueChanged.RemoveListener(OnSliderChanged);
            _view.InputField.onEndEdit.RemoveListener(OnInputChanged);
        }
    }
}