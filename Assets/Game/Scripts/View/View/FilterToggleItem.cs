using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    public class FilterToggleItem : MonoBehaviour
    {
        [field: SerializeField] public Toggle Toggle { get; private set; }
        [field: SerializeField] public TMP_Text Label { get; private set; }
    }
}