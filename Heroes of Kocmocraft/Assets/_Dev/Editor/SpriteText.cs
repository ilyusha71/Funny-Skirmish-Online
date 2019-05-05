using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteText : EditorWindow
{
    public Transform[] apron;
    public GameObject Sticker;
    public int count;




    public Texture2D spriteSheet;           //Sprite Atlas to copy from settings
    public int frameHeight;
    public int frameWidth;
    public Texture2D pasteTo;           //Sprite atlas where to paste settings

    private Sprite[] _sprites;           //Collection of sprites from source texture for faster referencing
    private string fileName; // 直接將Texture名稱拿來命名Anim

    [MenuItem("Hangar/Sprite Text")]
    static void Init()
    {
        // Window Set-Up
        SpriteText window = EditorWindow.GetWindow(typeof(SpriteText), false, "AnimationGenerator", true) as SpriteText;
        window.minSize = new Vector2(260, 170); window.maxSize = new Vector2(260, 170);
        window.Show();
    }

    //Show UI
    void OnGUI()
    {
        Sticker = (GameObject)EditorGUILayout.ObjectField("Sticker", Sticker, typeof(GameObject), true);
        count = EditorGUILayout.IntField(count, GUILayout.Width(50));



        spriteSheet = (Texture2D)EditorGUILayout.ObjectField("SpriteSheet", spriteSheet, typeof(Texture2D), true);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Animation"))
        {
            if (spriteSheet != null)
            {
                cutSprites();
                //makeIdle();
                //makeAnimation(0, "DOWN");
                //makeAnimation(4, "LEFT");
                //makeAnimation(8, "RIGHT");
                //makeAnimation(12, "UP");
            }
        }

        Repaint();
    }

    void CreateSpriteRender()
    {
        GameObject column = new GameObject();
        column.name = "Stickers";
        //float origin = sizeY * unitColumn * 0.5f;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = PrefabUtility.InstantiatePrefab(Sticker) as GameObject;
            obj.transform.SetParent(column.transform);
            //obj.transform.position = new Vector3(0, 0, -origin + j * sizeY);
        }
    }

    private void cutSprites()
    {
        if (!IsAtlas(spriteSheet))
        {
            Debug.LogWarning("Unable to proceed, the source texture is not a sprite atlas.");
            return;
        }
        //Proceed to read all sprites from CopyFrom texture and reassign to a TextureImporter for the end result
        UnityEngine.Object[] _objects = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(spriteSheet));

        if (_objects != null && _objects.Length > 0)
            _sprites = new Sprite[_objects.Length];

        for (int i = 0; i < _objects.Length; i++)
        {
            _sprites[i] = _objects[i] as Sprite;
        }
    }

    private void makeAnimation(int frame, string direction)
    {
        //http://forum.unity3d.com/threads/lack-of-scripting-functionality-for-creating-2d-animation-clips-by-code.212615/
        AnimationClip animClip = new AnimationClip();
        animClip.wrapMode = WrapMode.Loop;
        animClip.frameRate = 6;   // FPS
        // First you need to create e Editor Curve Binding
        EditorCurveBinding curveBinding = new EditorCurveBinding();

        // I want to change the sprites of the sprite renderer, so I put the typeof(SpriteRenderer) as the binding type.
        curveBinding.type = typeof(SpriteRenderer);
        // Regular path to the gameobject that will be changed (empty string means root)
        curveBinding.path = "";
        // This is the property name to change the sprite of a sprite renderer
        curveBinding.propertyName = "m_Sprite";

        // An array to hold the object keyframes
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[4];
        for (int i = 0; i < 4; i++)
        {
            keyFrames[i] = new ObjectReferenceKeyframe();
            // set the time
            keyFrames[i].time = (float)i / 6;
            // set reference for the sprite you want
            keyFrames[i].value = _sprites[i + frame];
            //Debug.LogWarning(_sprites[i + frame]);
        }
        AnimationClipSettings animationClipSettings = new AnimationClipSettings();
        animationClipSettings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(animClip, animationClipSettings);
        AnimationUtility.SetObjectReferenceCurve(animClip, curveBinding, keyFrames);

        string path = "Assets/Doraemons/Animations/" + spriteSheet.name + direction + ".anim";
        AnimationClip outputAnimClip = AssetDatabase.LoadMainAssetAtPath(path) as AnimationClip;
        if (outputAnimClip != null)
        {
            EditorUtility.CopySerialized(animClip, outputAnimClip);
            AssetDatabase.SaveAssets();
        }
        else // 防止覆蓋後失去reference
        {
            outputAnimClip = new AnimationClip();
            EditorUtility.CopySerialized(animClip, outputAnimClip);
            AssetDatabase.CreateAsset(outputAnimClip, path);
        }
    }

    private void makeIdle()
    {
        //http://forum.unity3d.com/threads/lack-of-scripting-functionality-for-creating-2d-animation-clips-by-code.212615/
        AnimationClip animClip = new AnimationClip();
        animClip.wrapMode = WrapMode.Loop;
        animClip.frameRate = 6;   // FPS
        // First you need to create e Editor Curve Binding
        EditorCurveBinding curveBinding = new EditorCurveBinding();

        // I want to change the sprites of the sprite renderer, so I put the typeof(SpriteRenderer) as the binding type.
        curveBinding.type = typeof(SpriteRenderer);
        // Regular path to the gameobject that will be changed (empty string means root)
        curveBinding.path = "";
        // This is the property name to change the sprite of a sprite renderer
        curveBinding.propertyName = "m_Sprite";

        // An array to hold the object keyframes
        ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[1];
        keyFrames[0] = new ObjectReferenceKeyframe();
        // set the time
        keyFrames[0].time = 0;
        // set reference for the sprite you want
        keyFrames[0].value = _sprites[0];
        AnimationClipSettings animationClipSettings = new AnimationClipSettings();
        animationClipSettings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(animClip, animationClipSettings);
        AnimationUtility.SetObjectReferenceCurve(animClip, curveBinding, keyFrames);

        string path = "Assets/Doraemons/Animations/" + spriteSheet.name + "IDLE.anim";
        AnimationClip outputAnimClip = AssetDatabase.LoadMainAssetAtPath(path) as AnimationClip;
        if (outputAnimClip != null)
        {
            EditorUtility.CopySerialized(animClip, outputAnimClip);
            AssetDatabase.SaveAssets();
        }
        else // 防止覆蓋後失去reference
        {
            outputAnimClip = new AnimationClip();
            EditorUtility.CopySerialized(animClip, outputAnimClip);
            AssetDatabase.CreateAsset(outputAnimClip, path);
        }
    }

    //Check that the texture is an actual atlas and not a normal texture
    private bool IsAtlas(Texture2D tex)
    {
        string _path = AssetDatabase.GetAssetPath(tex);
        TextureImporter _importer = AssetImporter.GetAtPath(_path) as TextureImporter;

        return _importer.textureType == TextureImporterType.Sprite && _importer.spriteImportMode == SpriteImportMode.Multiple;
    }

}
