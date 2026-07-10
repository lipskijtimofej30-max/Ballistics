using UnityEngine;

namespace Game.Scripts.UX
{
    public class ParameterCanvasInteractable : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Toggle(bool toggle) => _canvasGroup.interactable = toggle;
    }
}