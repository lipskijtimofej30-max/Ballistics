using Game.Scripts.Core;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class TelemetryPanelView : MonoBehaviour, IUiPanel
    {
        [SerializeField] private GameObject _telemetryPanel;
        [SerializeField] private TMP_Text _titleText;
        public void Show()
        {
            _titleText.text = "Телеметрия";
            _telemetryPanel.SetActive(true);
        }
        public void Hide() => _telemetryPanel.SetActive(false);
    }
}