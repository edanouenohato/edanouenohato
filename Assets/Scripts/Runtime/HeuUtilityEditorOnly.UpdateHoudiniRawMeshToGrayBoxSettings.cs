// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

internal partial class HeuUtilityEditorOnly
{
#if UNITY_EDITOR


    private void UpdateHoudiniRawMeshToGrayBoxSettings(string args)
    {
        // Gets the arguments
        var argsArray = SplitArgs(args);

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

        // 特別なルールの HDA, Mesh / Collider 2つ Output がある GameObject を "いい感じに" 結合する
        CombineMeshAndCollider(hdaRoot);

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

            // ついでにリネームしておく
            var goName = go.name;
            if (goName.Contains("_output0_output0_0"))
            {
                go.name = goName.Replace("_output0_output0_0", "");
            }

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
        UpdateIsStatic(targetGameObject, argsArray[0] == "on");
        UpdateLayer(targetGameObject, argsArray[1]);
        UpdateMaterial(targetGameObject, argsArray[2], argsArray[3]);
        AddMeshCollider(targetGameObject);

        // ログを出す
        foreach (var go in targetGameObject)
        {
            Debug.Log($"[HeuUtility] {go.name} settings auto update for GrayBox.", go);
        }
    }

    /// <summary>
    /// HDA で Output が2つあり, Mesh / Collider という名前のばあいは結合します
    /// </summary>
    /// <param name="hdaRoot"></param>
    private static void CombineMeshAndCollider(Transform hdaRoot)
    {
        var targetColliderGo = new List<GameObject>();

        // Note: ここらへん HDA の仕様変わったら変える必要があります
        // 特別なルールの HDA, Mesh / Collider 2つ Output がある GameObject を "いい感じに" 結合する
        foreach (Transform child in hdaRoot)
        {
            // _Collider_Collider_0 という名前を探す
            if (child.gameObject.name.Contains("_Collider_Collider_0"))
            {
                targetColliderGo.Add(child.gameObject);
            }
        }

        foreach (var colliderGo in targetColliderGo)
        {
            var baseName = colliderGo.name.Replace("_Collider_Collider_0", "");
            var oldCollider = colliderGo.GetComponent<Collider>();

            // Mesh の方を検索する
            var targetMeshTrs = hdaRoot.Find(baseName + "_Mesh_Mesh_0");
            if (targetMeshTrs == null)
            {
                continue;
            }

            // Mesh の方に Collider をコピーする
            ComponentUtility.CopyComponent(oldCollider);
            ComponentUtility.PasteComponentAsNew(targetMeshTrs.gameObject);

            Debug.Log($"[HeuUtility] Founded Collider and Mesh pair. Combined: {baseName}", targetMeshTrs.gameObject);
            // ついでにリネームしとく
            targetMeshTrs.gameObject.name = baseName;
        }

        // Collider はもういらないので削除する
        foreach (var colliderGo in targetColliderGo)
        {
            DestroyImmediate(colliderGo);
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

    private static void UpdateMaterial(in IReadOnlyList<GameObject> targetGos, string defaultMaterialPath,
        string vertexColorMaterialPath)
    {
        foreach (var go in targetGos)
        {
            // Renderer を取得しておく
            var renderer = go.GetComponent<MeshRenderer>();

            // Mesh が VertexColor をもっているかどうかを確認する
            var filter = go.GetComponent<MeshFilter>();
            var mesh = filter.sharedMesh;
            var vertexColor = mesh.colors;
            if (vertexColor.Length == 0)
            {
                // VertexColor を持っていないなら, 通常のマテリアルを設定する
                renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(defaultMaterialPath);
            }
            else
            {
                // VertexColor を持っているなら, 専用のマテリアルを設定する
                renderer.sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(vertexColorMaterialPath);
            }
        }
    }

    private static void AddMeshCollider(in IReadOnlyList<GameObject> targetGos)
    {
        foreach (var go in targetGos)
        {
            // 既になにかの Collider がついているなら無視する
            if (go.GetComponent<Collider>() != null)
            {
                continue;
            }

            // MeshFilter が持つ SharedMesh を MeshCollider に設定する
            var filter = go.GetComponent<MeshFilter>();
            var col = go.AddComponent<MeshCollider>();
            col.sharedMesh = filter.sharedMesh;
        }
    }


#endif
}