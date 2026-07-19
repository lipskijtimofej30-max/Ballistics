using UnityEngine;

namespace Game.Scripts.Infrastructure.Signals
{
    public class ShowTooltipSignal
    {
        public string Text;
        public Vector2 ScreenPosition;
        public ShowTooltipSignal(string text, Vector2 screenPosition)
        {
            Text = text;
            ScreenPosition = screenPosition;
        }
    }
}