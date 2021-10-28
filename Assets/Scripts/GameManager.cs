using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    List<string> fileList = new List<string>();
    List<int> imagesNumbers = new List<int>();
    public int selectedImage = 0;

    public GameObject framePrefab;
    public GameObject drawingPrefab;
    // Start is called before the first frame update


    private bool startGame = false;
    private bool reloadGame = true;
    public bool generateFrame = false;


    public GameObject actualFrame;
    void Start()
    {
        GetImages(fileList, "Drawing Pairs");
        Debug.Log("Test1");
        StartCoroutine(WaitSecond(3.0f));
    }


    bool drawInPlace = false;
    // Update is called once per frame
    void Update()
    {
        if(startGame && reloadGame)
        {
            for(int i = 1; i <= fileList.Count/2 ; i++)
            {
                imagesNumbers.Add(i);
                Debug.Log(i);
            }
            generateFrame = true;
            reloadGame = false;
        }

        if(generateFrame)
        {
            GenerateNewPair(RandomInt(imagesNumbers.Count));
            if(!drawInPlace)
            {
                GenerateRandomDrawings();
                drawInPlace = true;
            }
            generateFrame = false;
        }

        
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

    int RandomInt(int listIndex)
    {
        int lIndex;
        lIndex = Random.Range(0, listIndex);
        int selectedNum = imagesNumbers[lIndex];
        imagesNumbers.Remove(lIndex);
        return selectedNum;
    }

    void GenerateNewPair(int imageNumber)
    {
        selectedImage = imageNumber;
        actualFrame = Instantiate(framePrefab, new Vector3(0, 1.85f, 0), Quaternion.identity);
        SpriteRenderer sr = GameObject.Find("ReferenceLine").GetComponentInChildren<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Drawing Pairs/" + imageNumber + "B");
        imagesNumbers.Remove(imageNumber);
    }

    void GenerateRandomDrawings()
    {
        for(int i = 1; i < 4; i++)
        {
            var tempDrawing = Instantiate(drawingPrefab, new Vector3(Random.Range(-7.0f, 7.0f), 9, 0), Quaternion.identity);
            tempDrawing.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Drawing Pairs/" + i + "A");
            tempDrawing.GetComponent<Drawing>().drawingNumber = i;
            tempDrawing.GetComponent<Drawing>().gm = this;
        }
        
    }


    float waitTimer = 0;
    bool WaitTime(float time, ref bool checkBool)
    {
        waitTimer += Time.deltaTime;

        if(waitTimer >= time)
        {
            checkBool = true;
            return true;
        }
        else
        {
            checkBool = false;
            return false;
        }

    }

    IEnumerator WaitSecond(float time)
    {
        yield return new WaitForSeconds(time);
        startGame = true;
    }


    public void NextFrame()
    {
        StartCoroutine(GoToPlace());
    }

    Vector3 placementPos = new Vector3(-20, 1.85f , 0f);
    IEnumerator GoToPlace()
    {
        yield return new WaitForSeconds(1.5f);
        while(actualFrame.transform.position != placementPos)
        {
            actualFrame.transform.position = Vector3.MoveTowards(actualFrame.transform.position, placementPos, 0.04f);
            yield return null;
        }
        actualFrame.GetComponent<FrameBehaviour>().ChangeName(selectedImage.ToString());
        generateFrame = true;
    }
}
