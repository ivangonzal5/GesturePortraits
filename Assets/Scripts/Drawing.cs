using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    public int drawingNumber = 0;
    Vector3 placementPos = new Vector3(0, 1.2f, 0);
    Vector3 dragPosOffset;
    Rigidbody2D rb2d;
    public GameManager gm;
    AudioSource soundFX;
    public AudioClip grabSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    bool dragable = true;


    bool correctAnswer = false;
    void Start()
    {
        dragPosOffset = new Vector3(0,0,10);
        rb2d = GetComponent<Rigidbody2D>();
        soundFX = gm.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        rb2d.isKinematic = true;
    }

    private void OnMouseUp() 
    {
        if(!correctAnswer)
        {
            rb2d.isKinematic = false;
            soundFX.PlayOneShot(grabSound);
        }

    }

    /// <summary>
    /// OnMouseDrag is called when the user has clicked on a GUIElement or Collider
    /// and is still holding down the mouse.
    /// </summary>
    void OnMouseDrag()
    {
        if(dragable)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position += dragPosOffset;

            if(Mathf.Abs(Vector3.Distance(transform.position, placementPos)) < 0.5f)
            {
                if(gm.selectedImage == drawingNumber)
                {
                    Debug.Log("Imagen Correta");
                    soundFX.PlayOneShot(correctSound);
                    correctAnswer = true;
                    dragable = false;
                    rb2d.isKinematic = true;
                    StartCoroutine(GoToPlace());

                    if(gm.imagesNumbers.Count == 0)
                    {
                        gm.GameOver(0);
                    }
                }
                else
                {
                    Debug.Log("Imagen Incorrecta");
                    soundFX.PlayOneShot(wrongSound);
                }
            }
        }

    }

    IEnumerator GoToPlace()
    {
        while(transform.position != placementPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, placementPos, 0.1f);
            yield return null;
        }
        transform.SetParent(gm.actualFrame.transform);
        gm.NextFrame();
    }

}
