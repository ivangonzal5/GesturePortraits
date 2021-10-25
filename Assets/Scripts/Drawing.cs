using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    public int drawingNumber = 0;
    Vector3 dragPosOffset;
    void Start()
    {
        dragPosOffset = new Vector3(0,0,10);
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
        Debug.Log("Clickeando");
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
    }

}
