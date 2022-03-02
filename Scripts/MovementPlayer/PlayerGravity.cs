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
    float SphereRadiusCollision;
    Rigidbody _rigidbody;
    CapsuleCollider capsuleCollider;
    float offsetPlayerFeet;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        _rigidbody.freezeRotation = true;
        SphereRadiusCollision = capsuleCollider.radius;
        // 0.1f is the padding so it doenst line perfectly with the mesh
        offsetPlayerFeet = capsuleCollider.height / 2f - capsuleCollider.radius + 0.1f; 
    }
    void Update()
    {
        ManageGravity();
    }
    private void ManageGravity()
    {
        isGrounded =Physics.CheckSphere(FindFeetPlayer() , SphereRadiusCollision, LayerToCollide);

        //ExtraGravity
        if(! isGrounded)
        {
            _rigidbody.AddForce(Vector3.down * ExtraGravity*Time.deltaTime);
        }

        ControllDrag();


        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
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
                transform.position.y - offsetPlayerFeet,
                transform.position.z);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(FindFeetPlayer(),SphereRadiusCollision);
    }
}