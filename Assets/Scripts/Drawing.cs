using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    public int drawingNumber = 0;
    Vector3 placementPos = new Vector3(0, 1.85f, 0);
    Vector3 dragPosOffset;
    Rigidbody2D rigidbody2D;
    public GameManager gm;
    void Start()
    {
        dragPosOffset = new Vector3(0,0,10);
        rigidbody2D = GetComponent<Rigidbody2D>();
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
        rigidbody2D.isKinematic = true;
    }

    private void OnMouseUp() 
    {
        rigidbody2D.isKinematic = false;
    }

    /// <summary>
    /// OnMouseDrag is called when the user has clicked on a GUIElement or Collider
    /// and is still holding down the mouse.
    /// </summary>
    void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position += dragPosOffset;

        if(Mathf.Abs(Vector3.Distance(transform.position, placementPos)) < 0.5f)
        {
            if(gm.selectedImage == drawingNumber)
            {
                Debug.Log("Imagen Correta");
            }
            else
            {
                Debug.Log("Imagen Incorrecta");
            }
        }
    }

}
