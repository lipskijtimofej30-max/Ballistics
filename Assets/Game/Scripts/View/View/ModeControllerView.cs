using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    public class ModeControllerView : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _objectsOnlySimulation = new();
        [SerializeField] private List<GameObject> _objectsOnlyExperiment = new();
        
        public void ShowObjectsForSimulation()
        {
            foreach (var obj in _objectsOnlySimulation)
                obj.SetActive(true);
        }
        
        public void ShowObjectsForExperiment()
        {
            foreach (var obj in _objectsOnlyExperiment)
                obj.SetActive(true);
        }
        
        public void HideObjectsForSimulation()
        {
            foreach (var obj in _objectsOnlySimulation)
                obj.SetActive(false);
        }
        public void HideObjectsForExperiment()
        {
            foreach (var obj in _objectsOnlyExperiment)
                obj.SetActive(false);
        }
    }
}