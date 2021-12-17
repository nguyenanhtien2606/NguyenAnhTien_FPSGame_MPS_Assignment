using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BuildProjectFolders : ScriptableWizard
{
    public bool IncludeResourceFolder = false;
    //public bool UseNamespace = false;

    public bool UseParentFolder = false;
    public string NameOfParentFolder;

    //private string SFGUID;
    //public List<string> nsFolders = new List<string>();

    public List<string> folders = new List<string>() { "Animations", "Audio", "Prefabs", "Textures", "Models", "Materials", "Sprites", "Scripts", "Scenes" };

    [MenuItem("Edit/Create Project Folders...")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Create Project Folders", typeof(BuildProjectFolders), "Create");
    }

    //Called when the window first appears
    void OnEnable()
    {

    }

    //Create button click
    void OnWizardCreate()
    {
        //create all the folders required in a project
        //primary and sub folders
        if (UseParentFolder == true && !string.IsNullOrEmpty(NameOfParentFolder))
        {
            string guiParent = AssetDatabase.CreateFolder("Assets", NameOfParentFolder);
            AssetDatabase.GUIDToAssetPath(guiParent);
        }
        else if (UseParentFolder == true && string.IsNullOrEmpty(NameOfParentFolder))
        {
            string guiParent = AssetDatabase.CreateFolder("Assets", "NewParentFolder");
            AssetDatabase.GUIDToAssetPath(guiParent);
        }

        foreach (string folder in folders)
        {
            if (UseParentFolder == true)
            {
                if (!string.IsNullOrEmpty(NameOfParentFolder))
                {
                    string guid = AssetDatabase.CreateFolder("Assets/" + NameOfParentFolder, folder);
                    AssetDatabase.GUIDToAssetPath(guid);
                }
                else
                {
                    string guid = AssetDatabase.CreateFolder("Assets/NewParentFolder", folder);
                    AssetDatabase.GUIDToAssetPath(guid);
                }
            }
            else
            {
                string guid = AssetDatabase.CreateFolder("Assets", folder);
                AssetDatabase.GUIDToAssetPath(guid);
            }
        }

        AssetDatabase.Refresh();

        //if (UseNamespace == true)
        //{
        //    foreach (string nsf in nsFolders)
        //    {
        //        //AssetDatabase.Contain
        //        string guid = AssetDatabase.CreateFolder("Assets/Scripts", nsf);
        //        string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
        //    }
        //}
    }

    //Runs whenever something changes in the editor window...
    void OnWizardUpdate()
    {
        if (IncludeResourceFolder == true && !folders.Contains("Resources"))
            folders.Add("Resources");

        if (IncludeResourceFolder == false && folders.Contains("Resources"))
            folders.Remove("Resources");

        //if (UseNamespace == true)
        //    AddNamespaceFolders();

        //if (UseNamespace == false)
        //    RemoveNamespceFolders();
    }

    //void AddNamespaceFolders()
    //{
    //    if (!nsFolders.Contains("Interfaces"))
    //        nsFolders.Add("Interfaces");

    //    if (!nsFolders.Contains("Classes"))
    //        nsFolders.Add("Classes");

    //    if (!nsFolders.Contains("States"))
    //        nsFolders.Add("States");
    //}

    //void RemoveNamespceFolders()
    //{
    //    if (nsFolders.Count > 0) 
    //        nsFolders.Clear();
    //}
}