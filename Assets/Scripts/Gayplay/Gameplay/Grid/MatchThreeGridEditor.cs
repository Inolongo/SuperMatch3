using System.Reflection;
using Gayplay.GayplayGrid;
using UnityEditor;
using UnityEngine;

namespace Gayplay.Gameplay.Grid
{
    [CustomEditor(typeof(MatchThreeGrid))]
    public class MatchThreeGridEditor : Editor
    {
        private MatchThreeGrid _matchThreeGrid;

        public override async void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (_matchThreeGrid == null)
            {
                _matchThreeGrid = FindObjectOfType<MatchThreeGrid>();
            }

            if (GUILayout.Button("Create cells"))
            {
                _matchThreeGrid.CreatCellsFromEditor();
            }
            
            if (GUILayout.Button("Destroy cells"))
            {
                _matchThreeGrid.DestroyCells();
                var assembly = Assembly.GetAssembly(typeof(SceneView));
                var type = assembly.GetType("UnityEditor.LogEntries");
                var method = type.GetMethod("Clear");
                method!.Invoke(new object(), null);
            }
        }
    }
}