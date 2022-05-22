using System;
using UnityEditor;
using UnityEngine;

namespace Gayplay.GayplayGrid
{
    [CustomEditor(typeof(MatchThreeGrid))]
    public class MatchThreeGridEditor : Editor
    {
        private MatchThreeGrid _matchThreeGrid;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (_matchThreeGrid == null)
            {
                _matchThreeGrid = FindObjectOfType<MatchThreeGrid>();
            }

            if (GUILayout.Button("Create cells"))
            {
                _matchThreeGrid.CreatCellsTest();
            }
            
            if (GUILayout.Button("Destroy cells"))
            {
                _matchThreeGrid.DestroyCells();
            }
        }
    }
}