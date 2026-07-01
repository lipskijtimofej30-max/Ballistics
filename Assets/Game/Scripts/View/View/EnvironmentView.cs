using DG.Tweening;
using Game.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class EnvironmentView : MonoBehaviour
    {
        [field:SerializeField] public ParameterView GravityParameter { get; private set; }
        [field: SerializeField] public ParameterView AirSpeedParameter { get; private set; }
        [field: SerializeField] public ParameterView AirDensityParameter { get; private set; }
        [field:SerializeField] public Toggle AirResistanceToggle {get; private set;}
        
        [SerializeField] private GameObject _container;
        
        public void ShowContainer()
        {
            _container.SetActive(true);
            _container.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack);
        }

        public void HideContainer()
        {
            _container.SetActive(false);
            _container.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack);
        }
    }
}