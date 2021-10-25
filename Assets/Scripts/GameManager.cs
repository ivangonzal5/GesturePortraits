using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    List<string> fileList = new List<string>();
    List<int> imagesNumbers = new List<int>();
    int selectedImage = 0;

    public GameObject framePrefab;
    public GameObject drawingPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GetImages(fileList, "Drawing Pairs");
        for(int i = 1; i < fileList.Count/2 ; i++)
        {
            imagesNumbers.Add(i);
        }
        GenerateNewPair(RandomInt(fileList.Count/2));
        GenerateRandomDrawings();

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
        imagesNumbers.Remove(selectionNum);
        return selectionNum;
    }

    void GenerateNewPair(int imageNumber)
    {
        selectedImage = imageNumber;
        var tempFrame = Instantiate(framePrefab, new Vector3(0, 1.85f, 0), Quaternion.identity);
        SpriteRenderer sr = GameObject.Find("ReferenceLine").GetComponentInChildren<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Drawing Pairs/" + imageNumber + "B");
    }

    void GenerateRandomDrawings()
    {
        for(int i = 1; i < 4; i++)
        {
            var tempDrawing = Instantiate(drawingPrefab, new Vector3(Random.Range(-7.0f, 7.0f), 9, 0), Quaternion.identity);
            tempDrawing.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Drawing Pairs/" + i + "A");
            tempDrawing.GetComponent<Drawing>().drawingNumber = 0;
        }
        
    }
}
