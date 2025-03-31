using System.IO;
using UnityEditor;

[InitializeOnLoad]
public class GitHooksInstaller
{
    const string HOOKS_SOURCE = "Assets/ProjectQQ/Scripts/etc/GitHooks/hooks";

    static GitHooksInstaller()
    {
        InstallGitHooks();
    }

    [MenuItem("Util/Git Hooks ¼³Ä¡", false, priority: 1)]
    static void InstallGitHooks()
    {
        string projectPath = Directory.GetCurrentDirectory();
        string hooksSource = Path.GetFullPath(HOOKS_SOURCE);
        string hooksTarget = Path.Combine(projectPath, ".git", "hooks");

        if (Directory.Exists(hooksSource) && Directory.Exists(Path.Combine(projectPath, ".git")))
        {
            foreach (var file in Directory.GetFiles(hooksSource))
            {
                string fileName = Path.GetFileName(file);
                string targetFile = Path.Combine(hooksTarget, fileName);

                File.Copy(file, targetFile, overwrite: true);
                File.SetAttributes(targetFile, FileAttributes.Normal);
            }
        }
    }
}