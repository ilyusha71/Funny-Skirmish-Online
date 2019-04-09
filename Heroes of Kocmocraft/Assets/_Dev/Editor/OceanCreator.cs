using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

//Copy and paste atlas settings to another atlas editor
public class OceanCreator : EditorWindow
{
    GameObject container;
    public GameObject tile;
    public GameObject tileColumn;
    public float sizeX;
    public float sizeY;
    public float unitRow;
    public float unitColumn;
    public bool save = true;
    [MenuItem("Window/Ocean Creator")]
    static void Init()
    {
        // Window Set-Up
        OceanCreator window = EditorWindow.GetWindow(typeof(OceanCreator), false, "Ocean Creator", true) as OceanCreator;
        window.minSize = new Vector2(260, 170); window.maxSize = new Vector2(260, 170);
        window.Show();
    }

    void OnGUI()
    {
        tile = (GameObject)EditorGUILayout.ObjectField("Tile", tile, typeof(GameObject), true);
        tileColumn = (GameObject)EditorGUILayout.ObjectField("Tile Column", tileColumn, typeof(GameObject), true);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Size X: ");
        sizeX = EditorGUILayout.FloatField(sizeX);
        GUILayout.Label("Size Y: ");
        sizeY = EditorGUILayout.FloatField(sizeY);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Unit Row: ");
        unitRow = EditorGUILayout.FloatField(unitRow);
        GUILayout.Label("Unit Column: ");
        unitColumn = EditorGUILayout.FloatField(unitColumn);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        save = EditorGUILayout.Toggle("by Column", save);
        GUILayout.EndHorizontal();

        Handles.BeginGUI();
        if (GUI.Button(new Rect(10, Screen.height - 60, 300, 20), "Generate Ocean Column"))
        {
            GenerateOceanColumn();
        }
        if (GUI.Button(new Rect(Screen.width - 310, Screen.height - 60, 300, 20), "Generate"))
        {
            GenerateOcean();
        }
        Handles.EndGUI();
    }

    public void GenerateOceanColumn()
    {
        GameObject column = new GameObject();
        column.name = "Ocean Column";
        float origin = sizeY * unitColumn * 0.5f;
        for (int j = 0; j < unitColumn; j++)
        {
            GameObject obj = PrefabUtility.InstantiatePrefab(tile) as GameObject;
            obj.transform.SetParent(column.transform);
            obj.transform.position = new Vector3(0, 0, -origin+ j * sizeY);
        }
        string path = "Assets/_Dev/Resources/Ocean Column.prefab";
        PrefabUtility.SaveAsPrefabAssetAndConnect(column, path, InteractionMode.UserAction);
        DestroyImmediate(column);
        Object columnPrefab = Resources.Load("Ocean Column");
        tileColumn = columnPrefab as GameObject;
    }

    public void GenerateOcean()
    {
        container = GameObject.Find("Ocean");
        if (container) DestroyImmediate(container);

        container = new GameObject();
        container.name = "Ocean";
        float origin = sizeX * unitRow * 0.5f;
        BoxCollider collider = container.AddComponent<BoxCollider>();
        collider.size = new Vector3(unitRow * sizeX, 0, unitColumn * sizeY);

        for (int i = 0; i < unitRow; i++)
        {
            //GameObject obj = PrefabUtility.InstantiateAttachedAsset(column) as GameObject; // Clone
            GameObject obj = PrefabUtility.InstantiatePrefab(tileColumn) as GameObject;
            obj.transform.SetParent(container.transform);
            obj.transform.position = new Vector3(-origin + sizeX + i * sizeX, 0, 0);
        }
        string path = "Assets/_Dev/Resources/Ocean.prefab";
        PrefabUtility.SaveAsPrefabAssetAndConnect(container, path, InteractionMode.UserAction);
    }

}