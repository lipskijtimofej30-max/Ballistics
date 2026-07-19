using UnityEngine;

namespace Game.Scripts.Core.Tooltip
{
    public interface ITooltipView
    {
        void Show(string text, Vector2 screenPosition);
        void Hide();
    }
}