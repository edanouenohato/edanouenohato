// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal partial class HeuUtilityEditorOnly
{
#if UNITY_EDITOR

    private void UpdateHoudiniRawMeshToGrayBoxSettings()
    {
        // Change Name
        gameObject.name = "(Editor Only) Update Houdini Raw Mesh To GrayBox Settings";

        // Remove HEU generated Mesh filter and Mesh Renderer
        {
            var meshFilter = GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                DestroyImmediate(meshFilter);
            }

            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                DestroyImmediate(meshRenderer);
            }
        }

        // HDA にアクセスして, MeshRenderer がついている GameObject を起点とする
        var hdaRoot = transform.parent.transform.parent;

        // Null OBJ が書き出されがちなので, 名前決め打ちして削除する
        RemoveNullObj(hdaRoot);

        // HDA にアクセスして, MeshRenderer がついている GameObject を起点とする
        var meshRenderers = hdaRoot.GetComponentsInChildren<MeshRenderer>();

        // Note: 生成された Mesh のうち, 以下の条件を満たすものを Houdini でそのまま書き出された Object として扱う
        // 1. IsStatic = false である
        // 2. Layer = Default である
        // 3. Tag = Untagged である
        var targetGameObject = new List<GameObject>();
        foreach (var mr in meshRenderers)
        {
            var go = mr.gameObject;

            // Check1: IsStatic であるならスキップ
            if (go.isStatic)
            {
                continue;
            }

            // Check2: Layer が Default 以外ならスキップ
            if (go.layer != LayerMask.NameToLayer("Default"))
            {
                continue;
            }

            // Check3: Tag が Untagged 以外ならスキップ
            if (!go.CompareTag("Untagged"))
            {
                continue;
            }

            targetGameObject.Add(go);
        }

        // 更新する
        UpdateIsStatic(targetGameObject, true);
        UpdateLayer(targetGameObject, "PHY_FrameCommon");
        UpdateMaterial(targetGameObject,
            "Packages/com.edanoue.eh.base/Common/Materials/DebugGrid VertexColor (World Space).mat");
        AddMeshCollider(targetGameObject);

        // ログを出す
        foreach (var go in targetGameObject)
        {
            Debug.Log($"[HeuUtility] {go.name} settings auto update for GrayBox.", go);
        }
    }

    private static void RemoveNullObj(Transform root)
    {
        // 以下のような名前であることが多いので, それを削除する
        // foo_control1_control1_0
        var removeGo = new List<GameObject>();
        foreach (Transform child in root)
        {
            var go = child.gameObject;
            if (go.name.Contains("_control1_control1_"))
            {
                removeGo.Add(go);
            }
        }

        foreach (var go in removeGo)
        {
            Debug.Log($"[HeuUtility] Remove Null OBJ: {go.name}");
            DestroyImmediate(go);
        }
    }

    private static void UpdateIsStatic(in IReadOnlyList<GameObject> targetGos, bool isStatic)
    {
        foreach (var go in targetGos)
        {
            go.isStatic = isStatic;
        }
    }

    private static void UpdateLayer(in IReadOnlyList<GameObject> targetGos, string layerName)
    {
        foreach (var go in targetGos)
        {
            go.layer = LayerMask.NameToLayer(layerName);
        }
    }

    private static void UpdateMaterial(in IReadOnlyList<GameObject> targetGos, string path)
    {
        foreach (var go in targetGos)
        {
            var renderer = go.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(path);
        }
    }

    private static void AddMeshCollider(in IReadOnlyList<GameObject> targetGos)
    {
        foreach (var go in targetGos)
        {
            var filter = go.GetComponent<MeshFilter>();
            var col = go.AddComponent<MeshCollider>();
            col.sharedMesh = filter.sharedMesh;
        }
    }


#endif
}