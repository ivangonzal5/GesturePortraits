using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    List<string> fileList = new List<string>();
    int selectedImage = 0;

    public GameObject framePrefab;
    // Start is called before the first frame update
    void Start()
    {
        GetImages(fileList, "Drawing Pairs");
        GenerateNewPair(RandomInt(fileList.Count/2));

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetImages(List<string> fileList, string pathDirection)
    {
        string AssetsFolderPath = Application.dataPath;
        string spriteFolder = AssetsFolderPath + "/Resources/" + pathDirection;

        DirectoryInfo dir = new DirectoryInfo(spriteFolder);
        FileInfo[] info = dir.GetFiles("*.png");
        foreach(FileInfo file in info)
        {
            string newName = file.Name;
            newName = newName.Remove(newName.Length - 4, 4);
            fileList.Add(newName);
        }
    }

    int RandomInt(int intPool)
    {
        int selectionNum;
        selectionNum = Random.Range(0, fileList.Count);
        return selectionNum;
    }

    void GenerateNewPair(int imageNumber)
    {
        selectedImage = imageNumber;
        var tempFrame = Instantiate(framePrefab, new Vector3(0, 1.85f, 0), Quaternion.identity);
        SpriteRenderer sr = GameObject.Find("ReferenceLine").GetComponentInChildren<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Drawing Pairs/" + imageNumber + "B");
    }
}
