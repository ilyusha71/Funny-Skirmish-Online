using System.Linq;
using UnityEditor;
using UnityEngine;

// [CanEditMultipleObjects]
// [CustomEditor (typeof (KocmocraftDatabase))]
public class KocmocraftDatabaseEditor : EditorWindow
{
    Vector2 scrollPos;
    private Editor editor;
    private KocmocraftDatabase database;
    // Add menu named "My Window" to the Window menu
    [MenuItem ("Kocmoca/Kocmocraft Database _g")]
    static void ShowDatabaseWindow ()
    {
        // EditorWindow.GetWindow(typeof(KocmocraftDatabaseEditor));
        var window = EditorWindow.GetWindow<KocmocraftDatabaseEditor> (false, "Kocmocraft Database", true);
        window.database = UnityEditor.AssetDatabase.LoadAssetAtPath<KocmocraftDatabase> ("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Kocmocraft Database.asset");
        // 直接根据ScriptableObject构造一个Editor
        window.editor = Editor.CreateEditor (window.database);
    }
    public void OnGUI ()
    {
        EditorGUILayout.BeginVertical (GUILayout.MinHeight (position.height));
        scrollPos = EditorGUILayout.BeginScrollView (scrollPos);

        // 直接调用Inspector的绘制显示
        this.editor.OnInspectorGUI ();
        DrawTypesInspector ();
        DrawKocmocraftInspector ();

        EditorGUILayout.EndScrollView ();
        EditorGUILayout.EndVertical ();
    }

    void DrawKocmocraftInspector ()
    {
        // GUILayout.Space (5);
        GUILayout.BeginHorizontal ();
        GUILayout.Label ("Kocmocraft", EditorStyles.boldLabel, GUILayout.Width (163));
        GUILayout.Label ("Radar", EditorStyles.boldLabel, GUILayout.Width (163));
        GUILayout.Label ("Turret", EditorStyles.boldLabel, GUILayout.Width (163));
        GUILayout.Label ("Hardpoint", EditorStyles.boldLabel, GUILayout.Width (163));
        GUILayout.Label ("Kocmomech", EditorStyles.boldLabel, GUILayout.Width (163));
        GUILayout.EndHorizontal ();
        for (int i = 0; i < database.kocmocraft.Count; i++)
        {
            DrawKocmoraft (i);
        }

        // DrawAddTypeButton ();
    }

    void DrawKocmoraft (int index)
    {
        if (index < 0 || index >= database.kocmocraft.Count)
            return;
        // BeginChangeCheck() 用來檢查在 BeginChangeCheck() 和 EndChangeCheck() 之間是否有 Inspector 變數改變
        EditorGUI.BeginChangeCheck ();
        GUILayout.BeginHorizontal ();
        {
            GUILayout.Label (database.kocmocraft[index].design.code.ToString (), GUILayout.Width (163));
            database.kocmocraft[index].type = (Kocmoca.Type) EditorGUILayout.EnumPopup (database.kocmocraft[index].type, GUILayout.Width (163));
            database.kocmocraft[index].turretOption = (TurretOption) EditorGUILayout.EnumPopup (database.kocmocraft[index].turretOption, GUILayout.Width (163));
            database.kocmocraft[index].type = (Kocmoca.Type) EditorGUILayout.EnumPopup (database.kocmocraft[index].type, GUILayout.Width (163));
            database.kocmocraft[index].type = (Kocmoca.Type) EditorGUILayout.EnumPopup (database.kocmocraft[index].type, GUILayout.Width (163));

            // 如果 Inspector 變數有改變，EndChangeCheck() 會回傳 True，才有必要去做變數存取
            if (EditorGUI.EndChangeCheck ())
            {
                // 在修改之前建立 Undo/Redo 記錄步驟
                Undo.RecordObject (database, "Modify Types");

                // database.Types[index].Name = newName;
                // database.Types[index].HitColor = newColor;

                // 每當直接修改 Inspector 變數，而不是使用 serializedObject 修改時，必須要告訴 Unity 這個 Compoent 已經修改過了
                // 在下一次存檔時，必須要儲存這個變數
                EditorUtility.SetDirty (database);
            }
            // if (GUILayout.Button ("Remove"))
            // {

            // }
        }
        GUILayout.EndHorizontal ();
    }

    void DrawTypesInspector ()
    {
        GUILayout.Space (5);
        GUILayout.Label ("State", EditorStyles.boldLabel);

        for (int i = 0; i < database.Types.Count; i++)
        {
            DrawType (i);
        }

        DrawAddTypeButton ();
    }

    void DrawType (int index)
    {
        if (index < 0 || index >= database.Types.Count)
            return;

        GUILayout.BeginHorizontal ();
        {
            GUILayout.Label ("Name", EditorStyles.label, GUILayout.Width (50));

            // BeginChangeCheck() 用來檢查在 BeginChangeCheck() 和 EndChangeCheck() 之間是否有 Inspector 變數改變
            EditorGUI.BeginChangeCheck ();
            string newName = GUILayout.TextField (database.Types[index].Name, GUILayout.Width (120));
            Color newColor = EditorGUILayout.ColorField (database.Types[index].HitColor);

            database.Types[index].Name = newName;
            database.Types[index].HitColor = newColor;

            // 如果 Inspector 變數有改變，EndChangeCheck() 會回傳 True，才有必要去做變數存取
            if (EditorGUI.EndChangeCheck ())
            {
                // 在修改之前建立 Undo/Redo 記錄步驟
                Undo.RecordObject (database, "Modify Types");

                database.Types[index].Name = newName;
                database.Types[index].HitColor = newColor;

                // 每當直接修改 Inspector 變數，而不是使用 serializedObject 修改時，必須要告訴 Unity 這個 Compoent 已經修改過了
                // 在下一次存檔時，必須要儲存這個變數
                EditorUtility.SetDirty (database);
            }

            if (GUILayout.Button ("Remove"))
            {
                // 系統會 "登" 一聲
                EditorApplication.Beep ();

                // 顯示對話框功能(帶有 OK 和 Cancel 兩個按鈕)
                if (EditorUtility.DisplayDialog ("Really?", "Do you really want to remove the state '" + database.Types[index].Name + "'?", "Yes", "No") == true)
                {
                    database.Types.RemoveAt (index);
                    EditorUtility.SetDirty (database);
                }

            }
        }
        GUILayout.EndHorizontal ();
    }

    void DrawAddTypeButton ()
    {
        if (GUILayout.Button ("Add new State", GUILayout.Height (30)))
        {
            Undo.RecordObject (database, "Add new Type");

            database.Types.Add (new BrickType { Name = "New State" });
            EditorUtility.SetDirty (database);
        }
    }
}