using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class TelemetryPanelView : MonoBehaviour, IUiPanel
    {
        [SerializeField] private GameObject _telemetryPanel;
        public void Show() => _telemetryPanel.SetActive(true);

        public void Hide() => _telemetryPanel.SetActive(false);
    }
}