using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof( Rigidbody),typeof(CapsuleCollider),typeof(PlayerGravity))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Speed")]
    public float GroundSpeed = 30f;
    public float MaxSpeed = 30f;
    public float turnSmoothTime = 10f;    
    Transform PlayerMainCamera;
    PlayerGravity playerGravity;
    Rigidbody _rigidbody;
    bool canMove = true;
    float offsetPlayerFeet;

    public delegate void ListenerActions();
    public event ListenerActions onRunEvent;
    public event ListenerActions onStopEvent;
    private void Awake()
    {
        PlayerMainCamera = Camera.main.transform;
        _rigidbody = GetComponent<Rigidbody>();
        playerGravity = GetComponent<PlayerGravity>();
    }
    void Update()
    {
        if (!canMove) return;
        
        MovePlayer();
        
        SpeedPlayer();
    }
    private void MovePlayer()
    {
        //Reading from the keyboard where to go
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        
        if (direction.magnitude < 0.1f)
        {
            //Raise event for listeners
            onStopEvent();

            if ( playerGravity.isGrounded)
            _rigidbody.velocity = new Vector3(0f,_rigidbody.velocity.y,0f);
            
            return;
        }
        
        //Raise Event for listeners
        onRunEvent();

        float targetAngle = FindNewRotationAngle(direction);
        
        Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized *  Time.deltaTime;
        if (playerGravity.isGrounded)
            moveDir *= GroundSpeed;

        _rigidbody.AddForce(moveDir, ForceMode.VelocityChange);
    }
    private float FindNewRotationAngle(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + PlayerMainCamera.eulerAngles.y;
        
        transform.rotation = Quaternion.Lerp(
        transform.rotation, 
        Quaternion.Euler(0f,targetAngle,0f),
        Time.deltaTime*turnSmoothTime);

        return targetAngle;
    }
    private void SpeedPlayer()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            GroundSpeed *= 3;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            GroundSpeed /= 3;
        if (_rigidbody.velocity.magnitude > MaxSpeed)
            _rigidbody.velocity = _rigidbody.velocity.normalized* MaxSpeed;
    }
}