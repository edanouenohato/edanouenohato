// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using HoudiniEngineUnity;
using UnityEngine;

[ExecuteInEditMode]
internal class HeuUtilityEditorOnly : MonoBehaviour
{
#if UNITY_EDITOR

    private static readonly bool _CONVERT_TO_GAMMA = false;

    /// <summary>
    /// </summary>
    private void GenerateBakedLightsFromObj()
    {
        // Change Name
        gameObject.name = "(Editor Only) Baked Light Generator";

        // Remove HEU generated Mesh filter and Mesh Renderer
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

        // Generate lights
        var head = 0;
        head += CreatePointLights(head);
        head += CreateGridLights(head);
    }

    private int CreatePointLights(int head)
    {
        var attrStore = gameObject.GetComponent<HEU_OutputAttributesStore>();
        if (attrStore == null)
        {
            HEU_Logger.LogWarning("No HEU_OutputAttributesStore component found!");
            return 0;
        }

        // Gets the light intensity and light count
        var count = attrStore.GetAttribute("point_light_count")._intValues[0];
        if (count == 0)
        {
            return 0;
        }

        // Gets the light attributes
        var posAttr = attrStore.GetAttribute("light_pos")._floatValues;
        var lightColorAttr = attrStore.GetAttribute("light_color")._floatValues;
        var lightIntensityAttr = attrStore.GetAttribute("light_intensity")._floatValues;

        // Creates the lights in children
        for (var i = 0; i < count; i++)
        {
            // Create new GameObject
            var go = new GameObject($"Point Light_{i:D3}")
            {
                transform =
                {
                    parent = transform
                }
            };
            var lightCom = go.AddComponent<Light>();

            var ci = head + i;

            // Set position
            var lightPosRaw = new Vector3(posAttr[ci * 3], posAttr[ci * 3 + 1], posAttr[ci * 3 + 2]);
            // Convert Houdini coordinate to Unity coordinate
            lightPosRaw = new Vector3(-lightPosRaw.x, lightPosRaw.y, lightPosRaw.z);
            go.transform.localPosition = lightPosRaw;
            // Set intensity
            var intensity = lightIntensityAttr[ci];
            lightCom.intensity = intensity;

            // Set color
            {
                Color lightColor;
                if (_CONVERT_TO_GAMMA)
                {
                    lightColor = new Color(
                        Mathf.LinearToGammaSpace(lightColorAttr[ci * 3]),
                        Mathf.LinearToGammaSpace(lightColorAttr[ci * 3 + 1]),
                        Mathf.LinearToGammaSpace(lightColorAttr[ci * 3 + 2]));
                }
                else
                {
                    lightColor = new Color(lightColorAttr[ci * 3], lightColorAttr[ci * 3 + 1],
                        lightColorAttr[ci * 3 + 2]);
                }

                lightCom.color = lightColor;
            }

            // Set type and mode
            lightCom.type = LightType.Point;
            lightCom.lightmapBakeType = LightmapBakeType.Baked;

            // Shadow Type
            lightCom.shadows = LightShadows.Soft;
        }

        return count;
    }

    private int CreateGridLights(int head)
    {
        var attrStore = gameObject.GetComponent<HEU_OutputAttributesStore>();
        if (attrStore == null)
        {
            HEU_Logger.LogWarning("No HEU_OutputAttributesStore component found!");
            return 0;
        }

        // Gets the light intensity and light count
        var count = attrStore.GetAttribute("grid_light_count")._intValues[0];
        if (count == 0)
        {
            return 0;
        }

        // Gets the light attributes
        var posAttr = attrStore.GetAttribute("light_pos")._floatValues;
        var rotAttr = attrStore.GetAttribute("light_rot")._floatValues;
        var lightColorAttr = attrStore.GetAttribute("light_color")._floatValues;
        var lightIntensityAttr = attrStore.GetAttribute("light_intensity")._floatValues;
        var areaSizeAttr = attrStore.GetAttribute("light_area_size")._floatValues;

        // Creates the lights in children
        for (var i = 0; i < count; i++)
        {
            // Create new GameObject
            var go = new GameObject($"Grid Light_{i:D3}")
            {
                transform =
                {
                    parent = transform
                }
            };
            var lightCom = go.AddComponent<Light>();

            var ci = head + i;

            // Set position
            var lightPosRaw = new Vector3(posAttr[ci * 3], posAttr[ci * 3 + 1], posAttr[ci * 3 + 2]);
            // Convert Houdini coordinate to Unity coordinate
            lightPosRaw = new Vector3(-lightPosRaw.x, lightPosRaw.y, lightPosRaw.z);
            go.transform.localPosition = lightPosRaw;

            // Set rotation
            var eulerXYZ = new Vector3(rotAttr[ci * 3], rotAttr[ci * 3 + 1], rotAttr[ci * 3 + 2]);

            // Convert Houdini coordinate to Unity coordinate
            // Rotation Order: XYZ => ZYX
            var zxyRot = Quaternion.AngleAxis(eulerXYZ.z, Vector3.forward) *
                         Quaternion.AngleAxis(eulerXYZ.y, Vector3.up) *
                         Quaternion.AngleAxis(eulerXYZ.x, Vector3.right);
            var zxyEuler = zxyRot.eulerAngles;

            // Note: Houdini の Editor だと Grid Light の Front が180度回転しているので, 更に反転させる
            var lightRot = Quaternion.Euler(zxyEuler.x, -zxyEuler.y, -zxyEuler.z) * Quaternion.Euler(180, 0, 0);
            go.transform.localRotation = lightRot;

            // Set intensity
            var intensity = lightIntensityAttr[ci];
            lightCom.intensity = intensity;

            // Set color
            {
                Color lightColor;
                if (_CONVERT_TO_GAMMA)
                {
                    lightColor = new Color(
                        Mathf.LinearToGammaSpace(lightColorAttr[ci * 3]),
                        Mathf.LinearToGammaSpace(lightColorAttr[ci * 3 + 1]),
                        Mathf.LinearToGammaSpace(lightColorAttr[ci * 3 + 2]));
                }
                else
                {
                    lightColor = new Color(lightColorAttr[ci * 3], lightColorAttr[ci * 3 + 1],
                        lightColorAttr[ci * 3 + 2]);
                }

                lightCom.color = lightColor;
            }

            // Set type and mode
            lightCom.type = LightType.Rectangle;
            lightCom.lightmapBakeType = LightmapBakeType.Baked;

            // Set area size
            lightCom.areaSize = new Vector2(areaSizeAttr[ci * 2], areaSizeAttr[ci * 2 + 1]);

            // Cast shadow
            lightCom.shadows = LightShadows.Soft;
        }

        // Update head
        return count;
    }

#endif
}