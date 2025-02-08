// using UnityEngine;
// using UnityEditor;
//
// public class SpriteEditorAdjuster : EditorWindow
// {
//     private float newX = 600;
//     private float newY = 150;
//     private float newWidth = 800;
//     private float newHeight = 1000f;
//
//     [MenuItem("Tools/Sprite Editor Adjuster")]
//     public static void ShowWindow()
//     {
//         GetWindow<SpriteEditorAdjuster>("Sprite Editor Adjuster");
//     }
//
//     private void OnGUI()
//     {
//         GUILayout.Label("Set Sprite Slice Values", EditorStyles.boldLabel);
//
//         newX = EditorGUILayout.FloatField("X Position:", newX);
//         newY = EditorGUILayout.FloatField("Y Position:", newY);
//         newWidth = EditorGUILayout.FloatField("Width:", newWidth);
//         newHeight = EditorGUILayout.FloatField("Height:", newHeight);
//
//         if (GUILayout.Button("Apply to All Selected Textures"))
//         {
//             ApplyValuesToSelectedTextures(new Rect(newX, newY, newWidth, newHeight));
//         }
//     }
//
//     private void ApplyValuesToSelectedTextures(Rect rect)
//     {
//         Object[] selectedObjects = Selection.objects; // Get all selected objects
//         int modifiedCount = 0;
//
//         foreach (Object obj in selectedObjects)
//         {
//             Texture2D selectedTexture = obj as Texture2D;
//
//             if (selectedTexture == null)
//             {
//                 Debug.LogWarning($"Skipped {obj.name}: Not a texture.");
//                 continue;
//             }
//
//             string path = AssetDatabase.GetAssetPath(selectedTexture);
//             TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
//
//             if (importer == null || importer.spriteImportMode != SpriteImportMode.Multiple)
//             {
//                 Debug.LogWarning($"Skipped {selectedTexture.name}: Not in Multiple Sprite mode.");
//                 continue;
//             }
//
//             // Retrieve sprite metadata
//             var spriteMetaDataArray = importer.spritesheet;
//
//             // Apply the new rect to all slices in the texture
//             for (int i = 0; i < spriteMetaDataArray.Length; i++)
//             {
//                 var meta = spriteMetaDataArray[i];
//                 meta.rect = rect;
//                 spriteMetaDataArray[i] = meta;
//             }
//
//             // Save changes
//             importer.spritesheet = spriteMetaDataArray;
//             EditorUtility.SetDirty(importer);
//             AssetDatabase.WriteImportSettingsIfDirty(path);
//             AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
//
//             Debug.Log($"Updated {selectedTexture.name}: X={rect.x}, Y={rect.y}, Width={rect.width}, Height={rect.height}");
//             modifiedCount++;
//         }
//
//         Debug.Log($"Processed {modifiedCount} textures.");
//     }
// }
