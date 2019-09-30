using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerScript : MonoBehaviour
{
    private CharacterController _characterController;
 
    // Start is called before the first frame update

    private Vector3 _velocity;

    [Header("Locomotion Parameters")]
    [SerializeField] private float _mass = 75; // [kg]

    [SerializeField] private float _acceleration = 30; // [m/s^2]

    [Header("Dependencies")]
    [SerializeField, Tooltip("What should determine the absolute forward when a player presses forward.")]
    private Transform _absoluteForward;

    private Vector3 _movement;
    private float _dragOnGround = 5;

    void Start()
    {
        _characterController = this.GetComponent<CharacterController>();
    }

    private void Update()
    {
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(_velocity.magnitude);
        ApplyGravity();
        ApplyGround();
        ApplyMovement();
        ApplyGroundDrag();

        _characterController.Move(_velocity * Time.deltaTime);//or Time.FixedDeltaTime
        if (_movement.magnitude > 0.3f)
        {
            transform.forward = -_movement;
            GetComponentInChildren<Transform>().forward = -_movement;
        }
    }

    private void ApplyGroundDrag()
    {
        if (_characterController.isGrounded)
        {
            _velocity = _velocity * (1 - Time.deltaTime * _dragOnGround);
        }
    }

    private void ApplyMovement()
    {
        if(_characterController.isGrounded)
        {
            Vector3 xzAbsoluteForward = Vector3.Scale(_absoluteForward.forward, new Vector3(1, 0, 1));
            Quaternion forwardRotation = Quaternion.LookRotation(xzAbsoluteForward);
            Vector3 relativeMovement = forwardRotation * _movement;
            _velocity += relativeMovement * _acceleration * Time.deltaTime; // F(= m.a) [m/s^2] * t [s]
        }
    }

    private void ApplyGround()
    {
        if(_characterController.isGrounded)
        {
            _velocity -= Vector3.Project(_velocity, Physics.gravity.normalized); //kleine pressure om character op de grond te houden
        }
    }

    private void ApplyGravity()
    {
        if(!_characterController.isGrounded)
        {
            _velocity += Physics.gravity * Time.deltaTime; //zwaartekracht adden
        }
    }
}
