using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Radar))]
public class RadarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var scripts = targets.OfType<Radar>();
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