//using UnityEditor;
//using UnityEngine;
//using Kocmoca;
//using System.Linq;

//[CanEditMultipleObjects]
//[CustomEditor(typeof(Prototype))]
//public class PrototypeEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        var scripts = targets.OfType<Prototype>();
//        if (GUILayout.Button("Create"))
//        {
//            foreach (var script in scripts)
//                script.Create();
//        }
//        DrawDefaultInspector();
//    }
//}