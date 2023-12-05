// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using UnityEngine;

[ExecuteInEditMode]
internal partial class HeuUtilityEditorOnly : MonoBehaviour
{
#if UNITY_EDITOR

    private static string[] SplitArgs(string args)
    {
        return args.Split(',');
    }

#endif
}