using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool DRAG_ABLE = true;


    List<string> fileList = new List<string>();
    public List<int> imagesNumbers = new List<int>();
    public int selectedImage = 0;

    public GameObject framePrefab;
    public GameObject drawingPrefab;

    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    // Start is called before the first frame update


    private bool startGame = false;
    private bool gameOver = false;
    private bool reloadGame = true;
    public bool generateFrame = false;

    public AudioClip win;
    public AudioClip lose;

    AudioSource soundFX;
    public GameObject actualFrame;
    void Start()
    {
        soundFX = GetComponent<AudioSource>();
        GetImages(fileList, "Drawing Pairs");
        Debug.Log("Test1");
        StartCoroutine(WaitSecond(4.0f));
    }


    bool drawInPlace = false;
    // Update is called once per frame
    void Update()
    {
        if(startGame && reloadGame)
        {
            Debug.Log("Existen " + fileList.Count/2 + " Imagenes");
            for(int i = 1; i <= fileList.Count/2 ; i++)
            {
                imagesNumbers.Add(i);
                //Debug.Log(i);
            }
            generateFrame = true;
            reloadGame = false;
            remainingTime = initialTime;
        }

        if(generateFrame && imagesNumbers.Count >0 && !gameOver)
        {
            
            GenerateNewPair(RandomIndex(imagesNumbers.Count));
            if(!drawInPlace)
            {
                GenerateRandomDrawings();
                drawInPlace = true;
            }
            generateFrame = false;
            Debug.Log(imagesNumbers.Count);
        }

        if(startGame && !gameOver)
        {
            if(remainingTime >0)
            {
                updateTime();
            }
            else
            {
                GameOver(1);
            }
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

    int RandomIndex(int listIndex)
    {
        int lIndex;
        lIndex = Random.Range(0, listIndex);
        int selectedNum = imagesNumbers[lIndex];
        //imagesNumbers.Remove(lIndex);
        return selectedNum;
    }

    void GenerateNewPair(int imageNumber)
    {
        selectedImage = imageNumber;
        actualFrame = Instantiate(framePrefab, new Vector3(0, 1.2f, 0), Quaternion.identity);
        SpriteRenderer sr = GameObject.Find("ReferenceLine").GetComponentInChildren<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Drawing Pairs/" + imageNumber + "B");
        imagesNumbers.Remove(imageNumber);
    }


    List<GameObject> drawingInScene = new List<GameObject>();
    void GenerateRandomDrawings()
    {
        for(int i = 1; i < 7; i++)
        {
            var tempDrawing = Instantiate(drawingPrefab, new Vector3(Random.Range(-7.0f, 7.0f), 9, 0), Quaternion.identity);
            tempDrawing.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Drawing Pairs/" + i + "A");
            tempDrawing.GetComponent<Drawing>().drawingNumber = i;
            tempDrawing.GetComponent<Drawing>().gm = this;
            drawingInScene.Add(tempDrawing);
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

    Vector3 placementPos = new Vector3(-20, 1.2f , 0f);
    IEnumerator GoToPlace()
    {
        yield return new WaitForSeconds(1.5f);
        while(actualFrame.transform.position != placementPos)
        {
            actualFrame.transform.position = Vector3.MoveTowards(actualFrame.transform.position, placementPos, 0.2f);
            yield return null;
        }
        actualFrame.GetComponent<FrameBehaviour>().ChangeName(selectedImage.ToString());
        generateFrame = true;
    }


    public void GameOver(int ending)
    {
        soundFX.Stop();
        gameOver = true;

        StartCoroutine(GameOverCoroutine(ending));

    }

    IEnumerator GameOverCoroutine(int ending)
    {
        if(ending == 0)
        {
            yield return new WaitForSeconds(2.0f);
        }


        gameOverPanel.SetActive(true);
        if(ending == 0)
        {
            soundFX.PlayOneShot(win);
            winPanel.SetActive(true);
            //ganaste
        }
        if(ending == 1)
        {
            actualFrame.SetActive(false);
            soundFX.PlayOneShot(lose);
            losePanel.SetActive(true);

            foreach(var draw in drawingInScene)
            {
                draw.SetActive(false);
            }
            //perdiste
        }
    }


    #region UI
    

    public TextMeshProUGUI timer;
    public float initialTime;
    private float remainingTime;
    void updateTime()
    {
        remainingTime -= Time.deltaTime;

        if(remainingTime >= 10)
        {
            timer.text = "00:" + (int)remainingTime;
        }
        else
        {
            timer.text = "00:0" + (int)remainingTime;
        }
    }

    #endregion
}
