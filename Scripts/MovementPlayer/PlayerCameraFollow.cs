using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour
{
    public float radius = 0.05f;
    public float offsetY = 0.5f;
    public float sensitivy = 1f;
    public bool invertX = true;
    public bool canMove = true;

    Transform playerMainCamera;
    private float invert = -1f;

    // Start is called before the first frame update
    void Awake()
    {
        playerMainCamera = GetComponentInChildren<Camera>().transform;

        if (playerMainCamera == null)
            playerMainCamera = Camera.main.transform;
    }
    void Start()
    {
        playerMainCamera.SetParent(null);
        //Ensure that if mouse goes from window start to finish then 1 lap will be made
        sensitivy *= (float) ((2f * Math.PI)/ (float) Screen.width);
        
        invert = (invertX) ? -1f : 1f;
    }

    void Update()
    {
        //This public variable could be changed
        if (!canMove)
            return;

        float mouseX = Input.mousePosition.x * sensitivy;
        var mouseRotateOffset = GetCirclePosition(mouseX);
        playerMainCamera.position = transform.position +  mouseRotateOffset;
        playerMainCamera.LookAt(transform,Vector3.up);
    }
    //SIN(between 0 and 2PI)
    //
    private Vector3 GetCirclePosition(float mouseX)
    {
        float y =(float) Math.Sin(mouseX * invert) * radius;
        float x =(float) Math.Cos(mouseX * invert) * radius;
        return new Vector3(x,offsetY,y);
    }

    public void MoveCamera(Vector3 v)
    {
        playerMainCamera.position = v;
    }

    public void LookCamera(Vector3 v)
    {
        playerMainCamera.LookAt(v,Vector3.up);
    }
}
