using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Explorer : MonoBehaviour
{
    public string path;
    // Start is called before the first frame update
    public void OpenExplorer()
    {
        EditorUtility.OpenFilePanel("Panel", "", "tmx");
        GetImage();
    }

    void GetImage()
    {
        if (path != null)
        {
           
            Debug.Log("Path");
            gameObject.GetComponent<GameField>().mapPath = path;
            gameObject.GetComponent<GameField>().MapLoad();
        }
        else
        {
            Debug.Log("Path Error!");
        }
    }
}
