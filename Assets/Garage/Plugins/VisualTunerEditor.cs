using UnityEditor;
using UnityEngine;
namespace Garage.Plugins
{
#if UNITY_EDITOR
    [CustomEditor(typeof(VisualTuneHelperSelection))]
    public class VisualTunerEditor : Editor
    {
        private void OnEnable()
        {
            Debug.Log("VisualTunerEditor enabled");
        }
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            VisualTuneHelperSelection current = (VisualTuneHelperSelection)target;
            if(GUILayout.Button("Дефалтнуть тюнинг"))
            {
                current.ResetToDefultTune();
            }
        }
    
    }
#endif
}
