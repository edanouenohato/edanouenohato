// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class BuildCommand
{
    private static string[] ScenePaths => EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();

    /// <summary>
    /// </summary>
    public static void PerformBuild()
    {
        var buildTarget = GetBuildTarget();
        var buildPath = GetBuildPath();
        var buildName = GetBuildName();

        if (buildTarget == BuildTarget.Android)
        {
            // Bundle version code
            if (TryGetEnv("ANDROID_BUNDLE_VERSION_CODE", out var value))
            {
                if (int.TryParse(value, out var version))
                {
                    PlayerSettings.Android.bundleVersionCode = version;
                    Console.WriteLine(
                        $":: ANDROID_BUNDLE_VERSION_CODE env var detected, set the bundle version code to {value}.");
                }
                else
                {
                    Console.WriteLine(
                        $":: ANDROID_BUNDLE_VERSION_CODE env var detected but the version value \"{value}\" is not an integer.");
                }
            }

            // Key store password
            if (TryGetEnv("UNITY_KEYSTORE_PASS", out var keyStorePass))
            {
                PlayerSettings.Android.keystorePass = keyStorePass;
            }
            else
            {
                Console.WriteLine(
                    ":: UNITY_KEYSTORE_PASS env var not set, skipping setup, using Unity's default keystore");
            }

            if (TryGetEnv("UNITY_KEYALIAS_PASS", out var keyAliasPass))
            {
                PlayerSettings.Android.keyaliasPass = keyAliasPass;
            }
            else
            {
                Console.WriteLine(
                    ":: UNITY_KEYALIAS_PASS env var not set, skipping setup, using Unity's default keystore");
            }
        }

        // ToDo: Android (Quest) 向けの適当な設定をやっています
        var options = BuildOptions.None;
        // Compression method
        options |= BuildOptions.CompressWithLz4;

        var buildReport = BuildPipeline.BuildPlayer(ScenePaths, GetFixedBuildPath(buildTarget, buildPath, buildName),
            buildTarget, options);

        if (buildReport.summary.result != BuildResult.Succeeded)
        {
            throw new Exception($"Build ended with {buildReport.summary.result} status");
        }

        Console.WriteLine(":: Done with build");
    }

    private static bool TryGetEnv(string key, out string value)
    {
        value = Environment.GetEnvironmentVariable(key) ?? string.Empty;
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private static string GetArgument(string name)
    {
        string[] args = Environment.GetCommandLineArgs();
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i].Contains(name))
            {
                return args[i + 1];
            }
        }

        return string.Empty;
    }

    private static BuildTarget GetBuildTarget()
    {
        var buildTargetName = GetArgument("customBuildTarget");
        Console.WriteLine(":: Received customBuildTarget " + buildTargetName);

        if (buildTargetName.TryConvertToEnum(out BuildTarget target))
        {
            return target;
        }

        Console.WriteLine(
            $":: {nameof(buildTargetName)} \"{buildTargetName}\" not defined on enum {nameof(BuildTarget)}, using {nameof(BuildTarget.NoTarget)} enum to build");
        return BuildTarget.NoTarget;
    }

    private static string GetBuildPath()
    {
        var buildPath = GetArgument("customBuildPath");
        Console.WriteLine(":: Received customBuildPath " + buildPath);
        if (buildPath == "")
        {
            throw new Exception("customBuildPath argument is missing");
        }

        return buildPath;
    }

    private static string GetBuildName()
    {
        var buildName = GetArgument("customBuildName");
        Console.WriteLine(":: Received customBuildName " + buildName);
        if (buildName == "")
        {
            throw new Exception("customBuildName argument is missing");
        }

        return buildName;
    }

    private static string GetFixedBuildPath(BuildTarget buildTarget, string buildPath, string buildName)
    {
        if (buildTarget.ToString().ToLower().Contains("windows"))
        {
            buildName += ".exe";
        }
        else if (buildTarget == BuildTarget.Android)
        {
            buildName += EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
        }

        return buildPath + buildName;
    }

    private static bool TryConvertToEnum<TEnum>(this string strEnumValue, out TEnum? value)
    {
        if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
        {
            value = default;
            return false;
        }

        value = (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
        return true;
    }
}