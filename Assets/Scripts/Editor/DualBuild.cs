using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;
using Scorm.Editor;
using UnityEditor.Build.Reporting;
using System.Linq;
using Data.ScenarioSettings;
using Managers;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using BuildOptions = UnityEditor.BuildOptions;
using Graph = NodeEditor.Graphs.Graph;

public class DualBuild
{
  private const string DualBuildPath = "Build";
  private const string DesktopBuildName = "WebGL_Build";
  private const string MobileBuildName = "WebGL_Mobile";
  private const string PRODUCTNAME = "SRIW_Scenario{0}_{1}";

  [MenuItem("McD/Dual Build Development")]
  public static void Build()
  {
    Debug.Log("Building WebGL development");
    var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).ToList();
    Build(BuildOptions.Development, scenes);
  }

  [MenuItem("McD/Dual Build Release")]
  public static void BuildRelease()
  {
    Debug.Log("Building WebGL release");
    var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).ToList();
    Build(BuildOptions.None, scenes);
  }
  [MenuItem("McD/Build All Scenarios (Release)")]
  public static void BuildAllScenariosRelease()
  {
    Debug.Log("Building All Scenarios");
    BuildScenario1(BuildOptions.None);
    BuildScenario2(BuildOptions.None);
    BuildScenario3(BuildOptions.None);
    BuildScenario4(BuildOptions.None);
    BuildScenario5(BuildOptions.None);
  }
  [MenuItem("McD/SetUpScenario/1")]
  public static void Scenario1()
  {
    SetupScenario1();
  }
  [MenuItem("McD/SetUpScenario/2")]
  public static void Scenario2()
  {
    SetupScenario2();
  }
  [MenuItem("McD/SetUpScenario/3")]
  public static void Scenario3()
  {
    SetupScenario3();
  }
  [MenuItem("McD/SetUpScenario/4")]
  public static void Scenario4()
  {
    SetupScenario4();
  }
  [MenuItem("McD/SetUpScenario/5")]
  public static void Scenario5()
  {
    SetupScenario5();
  }

  private static List<EditorBuildSettingsScene> SetupScenario1()
  {
    Debug.Log("Setup Scenario 1");
    List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/LoadingScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/McScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene( "Assets/Scenes/Kitchen & BreakRoom_Scenario01.unity", true));
    EditorBuildSettings.scenes = scenes.ToArray();
    AssetDatabase.SaveAssets();
    string flowGraph = "Assets/Scripts/NodeEditor/Graphs/Scenario01_Steps_FlowGraph.asset";
    string scenarioSettings = "Assets/Scripts/Data/ScenarioSettings/Scenario01_Settings.asset";
    SetupScenario("01", flowGraph,scenarioSettings);
    return scenes;
  }
  private static List<EditorBuildSettingsScene> SetupScenario2()
  {
    Debug.Log("Setup Scenario 2");
    List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/LoadingScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/McScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene( "Assets/Scenes/Kitchen & BreakRoom_Scenario02.unity", true));
    EditorBuildSettings.scenes = scenes.ToArray();
    AssetDatabase.SaveAssets();
    string flowGraph = "Assets/Scripts/NodeEditor/Graphs/Scenario02_Steps_FlowGraph.asset";
    string scenarioSettings = "Assets/Scripts/Data/ScenarioSettings/Scenario02_Settings.asset";
    SetupScenario("02", flowGraph, scenarioSettings);
    return scenes;
  }
  private static List<EditorBuildSettingsScene> SetupScenario3()
  {
    Debug.Log("Setup Scenario 3");
    List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/LoadingScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/McScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Kitchen & BreakRoom_Scenario02.unity", true));
    scenes.Add(new EditorBuildSettingsScene( "Assets/Scenes/LivingRoom1_Scenario03.unity", true));
    EditorBuildSettings.scenes = scenes.ToArray();
    AssetDatabase.SaveAssets();
    string flowGraph = "Assets/Scripts/NodeEditor/Graphs/Scenario03_Steps_FlowGraph.asset";
    string scenarioSettings = "Assets/Scripts/Data/ScenarioSettings/Scenario03_Settings.asset";
    SetupScenario("03", flowGraph, scenarioSettings);
    return scenes;
  }
  private static List<EditorBuildSettingsScene> SetupScenario4()
  {
    Debug.Log("Setup Scenario 4");
    List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/LoadingScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/McScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Kitchen & BreakRoom_Scenario02.unity", true));
    EditorBuildSettings.scenes = scenes.ToArray();
    AssetDatabase.SaveAssets();
    string flowGraph = "Assets/Scripts/NodeEditor/Graphs/Scenario04_Steps_FlowGraph.asset";
    string scenarioSettings = "Assets/Scripts/Data/ScenarioSettings/Scenario04_Settings.asset";
    SetupScenario("04", flowGraph, scenarioSettings);
    return scenes;
  }
  private static List<EditorBuildSettingsScene> SetupScenario5()
  {
    Debug.Log("Setup Scenario 5");
    List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/LoadingScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/McScene.unity", true));
    scenes.Add(new EditorBuildSettingsScene("Assets/Scenes/Kitchen & BreakRoom_Scenario05.unity", true));
    EditorBuildSettings.scenes = scenes.ToArray();
    AssetDatabase.SaveAssets();
    string flowGraph = "Assets/Scripts/NodeEditor/Graphs/Scenario05_Steps_FlowGraph.asset";
    string scenarioSettings = "Assets/Scripts/Data/ScenarioSettings/Scenario05_Settings.asset";
    SetupScenario("05", flowGraph, scenarioSettings);
    return scenes;
  }

  public static void BuildScenario1(BuildOptions options)
  {
    var scenes = SetupScenario1();
    string zipPath = Build(options, scenes);
    MoveScormZip(zipPath, "Scenario1.zip");
    Debug.Log("Built Scenario 1");
  }
  
  public static void BuildScenario2(BuildOptions options)
  {
    var scenes = SetupScenario2();
    string zipPath = Build(options, scenes);
    MoveScormZip(zipPath, "Scenario2.zip");
    Debug.Log("Built Scenario 2");
  }
  public static void BuildScenario3(BuildOptions options)
  {
    List<EditorBuildSettingsScene> scenes = SetupScenario3();
    string zipPath = Build(options, scenes);
    MoveScormZip(zipPath, "Scenario3.zip");
    Debug.Log("Built Scenario 3");
  }
  public static void BuildScenario4(BuildOptions options)
  {
    List<EditorBuildSettingsScene> scenes = SetupScenario4();
    string zipPath = Build(options, scenes);
    MoveScormZip(zipPath, "Scenario4.zip");
    Debug.Log("Built Scenario 4");
  }
  
  public static void BuildScenario5(BuildOptions options)
  {
    List<EditorBuildSettingsScene> scenes = SetupScenario5();
    string zipPath = Build(options, scenes);
    MoveScormZip(zipPath, "Scenario5.zip");
    Debug.Log("Built Scenario 5");
  }
  
  public static void SetupScenario(string scenarioNumber, string flowGraph, string scenarioSettings)
  {
    string productName = string.Format(PRODUCTNAME, scenarioNumber, DateTime.Now.ToString("ddMMyy"));
    PlayerSettings.productName = productName;

    
    string scenePath = "Assets/Scenes/McScene.unity";

    // Open the scene
    Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

    if (scene.IsValid())
    {
      var gameManager = GameObject.FindObjectOfType<GameManager>();
      gameManager.m_FlowGraph = AssetDatabase.LoadAssetAtPath<Graph>(flowGraph);
      gameManager.m_ScenarioSettings = AssetDatabase.LoadAssetAtPath<ScenarioSettings>(scenarioSettings);

        // Mark the scene as dirty to indicate that it has been modified
      EditorUtility.SetDirty(gameManager);
      EditorSceneManager.MarkSceneDirty(scene);
      EditorSceneManager.SaveScene(scene);
    }
    else
    {
      throw new Exception("Failed to open scene");
    }
    
    ScormPublishSettings settings = Resources.Load<ScormPublishSettings>(ScormPublishSettings.RelativePath);
    settings.CourseTitle = productName;
    EditorUtility.SetDirty(settings);
    AssetDatabase.SaveAssets();
    Debug.Log("Scene Setup Complete");
  }

  private static void MoveScormZip(string buildPath, string fileName)
  {
    if(string.IsNullOrEmpty(buildPath))
      return;

    FileInfo file = new FileInfo(buildPath);

    FileInfo newFile = new FileInfo(file.Directory.Parent.Parent.Parent.FullName + '/' + fileName);
    newFile.Delete();
    file.MoveTo(newFile.FullName);
  }
  
  private static string Build(BuildOptions options, List<EditorBuildSettingsScene> scenes)
  {
    //This builds the player twice: a build with desktop-specific texture settings (WebGL_Build) as well as mobile-specific texture settings (WebGL_Mobile), and combines the necessary files into one directory (WebGL_Build)

    // Delete existing build directory to start fresh
    if (Directory.Exists(DualBuildPath))
    {
      Debug.Log($"Deleting existing build directory at {DualBuildPath} to start fresh.");
      try
      {
        Directory.Delete(DualBuildPath, true); // true for recursive delete
      }
      catch (Exception ex)
      {
        Debug.LogError($"Failed to delete build directory: {ex.Message}");
        return null; // Abort the build process if we can't delete the old build directory
      }
    }

    string desktopPath = Path.Combine(DualBuildPath, DesktopBuildName);
    string mobilePath = Path.Combine(DualBuildPath, MobileBuildName);

    EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.DXT;
    var desktopBuildReport = BuildPipeline.BuildPlayer(scenes.ToArray(), desktopPath, BuildTarget.WebGL, options);
    if (desktopBuildReport.summary.result != BuildResult.Succeeded)
    {
      Debug.LogError("Desktop build failed. Aborting process.");
      return null; // Abort the entire process if the desktop build fails
    }

    EditorUserBuildSettings.webGLBuildSubtarget = WebGLTextureSubtarget.ASTC;
    var mobileBuildReport = BuildPipeline.BuildPlayer(scenes.ToArray(), mobilePath, BuildTarget.WebGL, options);
    if (mobileBuildReport.summary.result != BuildResult.Succeeded)
    {
      Debug.LogError("Mobile build failed. Aborting process.");
      return null; // Abort the entire process if the mobile build fails
    }

    try
    {
        var dataFile = options == BuildOptions.Development ? MobileBuildName + ".data" : MobileBuildName + ".data.unityweb";
        string sourceFile = Path.Combine(mobilePath, "Build", dataFile);
        string destinationFile = Path.Combine(desktopPath, "Build", dataFile);

        if (File.Exists(destinationFile))
        File.Delete(destinationFile);

      FileUtil.CopyFileOrDirectory(sourceFile, destinationFile);

      // Ensure we can delete the directory by removing read-only attributes
      SetAttributesNormal(new DirectoryInfo(mobilePath));
      Directory.Delete(mobilePath, true);
    }
    catch (Exception ex)
    {
      Debug.LogError("Failed to copy files or delete directory: " + ex.Message);
      return null; // Abort if the file copy or directory deletion fails
    }

    var spp = new ScormPostprocessor();
    string desktopPathAbsolute = Path.GetFullPath(desktopPath);
    return spp.CreateScormPackage(desktopPathAbsolute);
  }
  private static void SetAttributesNormal(DirectoryInfo dir)
  {
    foreach (var subDir in dir.GetDirectories())
    {
      SetAttributesNormal(subDir);
    }
    foreach (var file in dir.GetFiles())
    {
      file.Attributes = FileAttributes.Normal;
    }
  }
}