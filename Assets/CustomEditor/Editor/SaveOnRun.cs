using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class SaveOnRun
{
    static string sceneIndexPrefs = "sceneIndexPrefs";
    static string hasChangedScenePrefs = "hasChangedScenePrefs";

    static SaveOnRun()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChangedProcess;
    }

    static void PlayModeStateChangedProcess(PlayModeStateChange state)
    {
        UnityEngine.SceneManagement.Scene activeScene = EditorSceneManager.GetActiveScene();

        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                //if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying || !activeScene.isDirty)
                //    return;

                if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying)
                    return;

                //Save scene if modifed (good bye Mr.Crash)
                if (activeScene.isDirty)
                {
                    EditorSceneManager.SaveScene(activeScene);
                    AssetDatabase.SaveAssets();
                }

                //Check run scene in index 0 first

                //if (!string.Equals(activeScene.name, NameFromIndex(0)))
                //{
                //    //Cache index of active scene
                //    PlayerPrefs.SetInt(sceneIndexPrefs, SceneIndexFromName(activeScene.name));
                //    PlayerPrefs.SetString(hasChangedScenePrefs, "true");

                //    //Load scene 0
                //    EditorSceneManager.OpenScene(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(0));
                //}
                break;

            case PlayModeStateChange.EnteredEditMode:
                //Return active scene in edit mode

                //if (PlayerPrefs.GetString(hasChangedScenePrefs) == "true")
                //{
                //    PlayerPrefs.SetString(hasChangedScenePrefs, "false");
                //    EditorSceneManager.OpenScene(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(PlayerPrefs.GetInt(sceneIndexPrefs)));
                //}
                break;
        }
    }

    static string NameFromIndex(int index)
    {
        string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(index);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

    static int SceneIndexFromName(string sceneName)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            string scene = NameFromIndex(i);
            if (scene == sceneName)
                return i;
        }
        return -1;
    }
}