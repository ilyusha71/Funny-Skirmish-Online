using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(KocmocraftModule))]
public class KocmocraftModuleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var scripts = targets.OfType<KocmocraftModule>();
        if (GUILayout.Button("Calculate and Save Database"))
        {
            foreach (var script in scripts)
            {
                script.Calculate();
                Debug.Log("<color=lime>" + script.name + "</color>  data has been save.");
                EditorUtility.SetDirty(script);
                AssetDatabase.SaveAssets();
            }
        }
        /* 以下無法觸發多項目批量編輯 */
        //var myTarget = (KocmocraftModule)target;
        //if (GUILayout.Button("Save Database"))
        //{
        //    Debug.Log(myTarget.name);
        //    EditorUtility.SetDirty(myTarget);
        //    AssetDatabase.SaveAssets();
        //}
        DrawDefaultInspector();
    }
}