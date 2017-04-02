using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class ApplyMeshColliders : Editor 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    [MenuItem("Custom/ApplyColliders/Add Mesh Colliders to all Children of the selected object")]
    static void AddMeshColliders()
    {
        Transform[] selection = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);

        if (selection.Length == 0)
        {
            EditorUtility.DisplayDialog("No source object selected!", "Please select one or more target objects", "");
            return;
        }

        int exportedObjects = 0;

        for (int i = 0; i < selection.Length; i++)
        {
            MeshFilter[] meshfilter = selection[i].GetComponentsInChildren<MeshFilter>();

            for (int m = 0; m < meshfilter.Length; m++)
            {
                if (!meshfilter[m].GetComponent<MeshCollider>())
                {
                    ++exportedObjects;
                    MeshCollider temp = meshfilter[m].gameObject.AddComponent<MeshCollider>();
                    temp.sharedMesh = meshfilter[m].sharedMesh;
                }

            }
        }

        if (exportedObjects > 0)
            EditorUtility.DisplayDialog("Colliders added", "Added " + exportedObjects + " colliders", "");
        else
            EditorUtility.DisplayDialog("Colliders not added", "Make sure at least some of your selected objects have mesh filters!", "");
    }
}
