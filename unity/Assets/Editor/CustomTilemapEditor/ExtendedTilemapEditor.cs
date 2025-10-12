using UnityEditor;
using UnityEngine;

namespace Framework.ExtendedGrid.Editor
{
    [CustomEditor(typeof(ExtendedTilemap))]
    public class ExtendedTilemapEditor : UnityEditor.Editor
    {
        private ExtendedTilemap tilemap;
        private bool paintMode;

        private void OnEnable()
        {
            tilemap = (ExtendedTilemap)target;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(10);
            
            GUI.backgroundColor = paintMode ? Color.green : Color.white;
            if (GUILayout.Button(paintMode ? "Exit Paint Mode" : "Enter Paint Mode"))
                paintMode = !paintMode;

            GUI.backgroundColor = Color.white;

            if (GUILayout.Button("Clear All Tiles"))
            {
                if (EditorUtility.DisplayDialog("Clear Tiles", "Remove all tiles?", "Yes", "Cancel"))
                {
                    Undo.RegisterFullObjectHierarchyUndo(tilemap.gameObject, "Clear Tiles");
                    tilemap.Clear();
                }
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!paintMode) return;

            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                Vector3 hitPos = hit.point;
                Handles.color = Color.green;
                Handles.DrawWireCube(hitPos, Vector3.one * 0.9f);
            }

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                HandleClick(e);
                e.Use();
            }
            else if (e.type == EventType.MouseDown && e.button == 1 && !e.alt)
            {
                HandleRightClick(e);
                e.Use();
            }
        }

        private void HandleClick(Event e)
        {
            if (!tilemap) return;

            var grid = tilemap.GetComponentInParent<Grid>();
            if (!grid) return;

            Vector3 world = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            Vector3Int cell = grid.WorldToCell(world);
            Vector2Int cell2D = new(cell.x, cell.y);

            Undo.RegisterFullObjectHierarchyUndo(tilemap.gameObject, "Place Tile");
            tilemap.PlaceTile(cell2D);
        }

        private void HandleRightClick(Event e)
        {
            var grid = tilemap.GetComponentInParent<Grid>();
            if (!grid) return;

            Vector3 world = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            Vector3Int cell = grid.WorldToCell(world);
            Vector2Int cell2D = new(cell.x, cell.y);

            Undo.RegisterFullObjectHierarchyUndo(tilemap.gameObject, "Remove Tile");
            tilemap.RemoveTile(cell2D);
        }
    }
}
