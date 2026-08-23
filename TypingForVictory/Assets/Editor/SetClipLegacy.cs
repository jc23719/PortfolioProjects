using UnityEngine;
using UnityEditor;

public class SetClipLegacy : EditorWindow {

    private AnimationClip clipToFix;

    [MenuItem("Tools/Animation/Set Clip to Legacy")]
    public static void ShowWindow() {
        GetWindow<SetClipLegacy>("Set Clip Legacy");
    }

    void OnGUI() {
        GUILayout.Label("Force Animation Clip to Legacy", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Drag your camera animation clip here, then click 'Set Legacy'.", MessageType.Info);

        clipToFix = (AnimationClip)EditorGUILayout.ObjectField("Camera Clip:", clipToFix, typeof(AnimationClip), false);

        if (clipToFix != null)
        {
            if (GUILayout.Button("Set " + clipToFix.name + " to Legacy"))
            {
                SetLegacyFlag();
            }
        }
    }

    private void SetLegacyFlag() {
        if (clipToFix != null) {
            clipToFix.legacy = true;

            EditorUtility.SetDirty(clipToFix);
            AssetDatabase.SaveAssets(); 

            Debug.Log(clipToFix.name + " has been marked as Legacy");
        }
    }
}