using System;
using System.IO;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public static class ForceLanguageVersion {

    private const string _targetText = "<LangVersion>6</LangVersion>";
    private const string _replacementText = "<LangVersion>latest</LangVersion>";

    private static Thread _currentThread;
    private static string[] _targetFiles;

    [DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        int indexOfLastDash = Application.dataPath.LastIndexOf('/');
        string projectRootFolder = Application.dataPath.Substring(0, indexOfLastDash);

        _targetFiles = new string[2]
        {
            projectRootFolder + "/Assembly-CSharp.csproj",
            projectRootFolder + "/Assembly-CSharp-Editor.csproj",
        };
        
        if(_currentThread == null)
        {
            CreateNewThread();
        }
        else if (!_currentThread.IsAlive)
        {
            CreateNewThread();
        }
    }
    private static void CreateNewThread()
    {
        _currentThread = new Thread(new ThreadStart(ForceVersion));
        _currentThread.Start();
    }

    private static void ForceVersion()
    {
        try
        {
            //Debug.Log("Creating process");

            while (true)
            {
                Thread.Sleep(10);

                for (int i = 0; i < _targetFiles.Length; i++)
                {
                    string targetFile = _targetFiles[i];
                    bool shouldWrite = false;

                    if (File.Exists(targetFile))
                    {
                        using (FileStream fileStream = new FileStream(targetFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (StreamReader streamReader = new StreamReader(fileStream))
                            {
                                while (streamReader.Peek() >= 0)
                                {
                                    if (streamReader.ReadLine().Contains(_targetText))
                                    {
                                        shouldWrite = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (shouldWrite)
                            ReplaceString(targetFile);
                    }
                    else
                    {
                        //Debug.LogWarning("Couldn't find " + targetFile);
                    }
                }
            }
        }
        catch (Exception e)
        {
            if(!(e is ThreadAbortException))
                Debug.LogException(e);

            throw;
        }
    }
    private static void ReplaceString(string filePath)
    {
        //Debug.Log("Replacing " + filePath);

        string[] allText = File.ReadAllLines(filePath);
                
        for (int i = 0; i < allText.Length; i++)
        {
            if(allText[i].Contains(_targetText))
            {
                //Debug.Log("Found line");
                allText[i] = allText[i].Replace(_targetText, _replacementText);
                break;
            }
        }

        File.Delete(filePath);
        File.WriteAllLines(filePath, allText);
    }
}
