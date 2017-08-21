using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public static class XcodePostProcessBuild {
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string path) {
        if (target != BuildTarget.iOS) {
            return;
        }

        var project = new PBXProject();
        project.ReadFromFile(PBXProject.GetPBXProjectPath(path));

        //Search Framework pathを追加
        string target_name = project.TargetGuidByName("Unity-iPhone");
        project.AddBuildProperty(target_name, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks/Plugins/iOS");
        project.WriteToFile(PBXProject.GetPBXProjectPath(path));
    }
}