using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(KocmocraftModule))]
public class KocmocraftDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myTarget = (KocmocraftModule)target;

        if (GUILayout.Button("Save Database"))
        {
            EditorUtility.SetDirty(myTarget);
            AssetDatabase.SaveAssets();
        }
        DrawDefaultInspector();
    }
}