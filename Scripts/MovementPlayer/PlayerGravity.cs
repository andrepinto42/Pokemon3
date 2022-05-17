using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGravity : MonoBehaviour
{
    public float ExtraGravity = 1000f;
    public float jumpHeight = 10f;
    public bool isGrounded = false;
    [Header("Drag")]
    public float DragGround = 4.5f;
    public float DragAir = 1f;
    [SerializeField] LayerMask LayerToCollide;
    [Header("Player Configurations")]
    public float paddingYBox = 0.75f;
    public float maxDistanceToGround = 0.2f;
    public float increaseHeightForward = 0.4f;

    float SphereRadiusCollision;
    Rigidbody _rigidbody;
    CapsuleCollider capsuleCollider;
    float offsetPlayerFeet;

    Vector3 vectorSizeFeet;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody.freezeRotation = true;
        SphereRadiusCollision = capsuleCollider.radius;
        // 0.1f is the padding so it doenst line perfectly with the mesh
        offsetPlayerFeet = capsuleCollider.height / 2f - capsuleCollider.radius + 0.1f; 
        vectorSizeFeet = new Vector3(SphereRadiusCollision*1.25f,0.2f,SphereRadiusCollision*1.25f) ;
    }
    public virtual void Update()
    {
        ManageGravity();
    }
    private void ManageGravity()
    {
        //Checking if a box near the player feet is colling with a specific type of Layer
        //Wrong implementation, should use instead a raycast, duh...
        // isGrounded = Physics.CheckBox(FindFeetPlayer(),vectorSizeFeet,Quaternion.identity,LayerToCollide);
        //Work for now
        Vector3 feetPlayer = FindFeetPlayer();        
        
        isGrounded =
            Physics.Raycast(feetPlayer,      Vector3.down,maxDistanceToGround,LayerToCollide) ;
        
        //ExtraGravity
        if(! isGrounded)
        {
            _rigidbody.AddForce(Vector3.down * ExtraGravity*Time.deltaTime);
        }

        ControllDrag();


        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            AddJumpForce();
        }
    }
    private void ControllDrag()
    {
        _rigidbody.drag = (isGrounded) ? DragGround : DragAir;
    }
    
    private Vector3 FindFeetPlayer()
    {
        return new Vector3
                (transform.position.x,
                transform.position.y - offsetPlayerFeet + paddingYBox,
                transform.position.z);
    }
 
    private void OnDrawGizmos()
    {
        Vector3 feetPlayer = FindFeetPlayer();
      
        Gizmos.DrawRay(feetPlayer,Vector3.down * maxDistanceToGround);
    }

    protected virtual void AddJumpForce()
    {
        _rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }
}