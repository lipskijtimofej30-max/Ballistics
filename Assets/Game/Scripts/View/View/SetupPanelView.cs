using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class SetupPanelView : MonoBehaviour, IUiPanel
    {
        [SerializeField] private GameObject _root;
        public void Show() => _root.SetActive(true);
        public void Hide() => _root.SetActive(false);
    }
}