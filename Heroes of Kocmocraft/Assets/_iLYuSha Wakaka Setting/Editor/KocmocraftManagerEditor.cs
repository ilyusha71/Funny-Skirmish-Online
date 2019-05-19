//using Kocmoca;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(KocmocraftManager))]
//public class KocmocraftManagerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {

//        //Sticker = (GameObject)EditorGUILayout.ObjectField("Sticker", Sticker, typeof(GameObject), true);
//        //WallStickerHangarNumber = (Texture2D)EditorGUILayout.ObjectField("Wall Sticker Hangar Number", WallStickerHangarNumber, typeof(Texture2D), true);
//        //GroundStickerHangarNumber = (Texture2D)EditorGUILayout.ObjectField("Ground Sticker Hangar Number", GroundStickerHangarNumber, typeof(Texture2D), true);
//        //GroundStickerKocmocraftName = (Texture2D)EditorGUILayout.ObjectField("Ground Sticker Kocmocraft Name", GroundStickerKocmocraftName, typeof(Texture2D), true);

//        var scripts = targets.OfType<KocmocraftManager>();
//        if (GUILayout.Button("Create"))
//        {
//            foreach (var script in scripts)
//                script.Initialize();
//        }
//        DrawDefaultInspector();
//    }
//}