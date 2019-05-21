using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Turret))]
public class TurretEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var scripts = targets.OfType<Turret>();
        if (GUILayout.Button("Calculate"))
        {
            foreach (var script in scripts)
            {
                Debug.Log("<color=lime>" + script.name + "</color>  data has been calculate.");
                script.Calculate();
                EditorUtility.SetDirty(script);
                AssetDatabase.SaveAssets();
            }
        }
        DrawDefaultInspector();
    }
}