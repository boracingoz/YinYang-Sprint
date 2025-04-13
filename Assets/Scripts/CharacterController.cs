using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;


    private int _currentLane = 1;
    private Rigidbody _rb;
    private bool isGrounded = true;
    private bool isJump = false;
    private Vector3 _targetPosition;


    [Header("Rotation Settings")]
    public float tiltAngle = 15f;
    public float rotationSmoothness = 5f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _targetPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLaneChange();
        HandleJump();
        ApplyTiltRotation();
    }

    private void FixedUpdate()
    {
        Vector3 forwardMove = Vector3.forward * forwardSpeed;
        Vector3 targetPos = new Vector3((_currentLane - 1) * laneDistance, _rb.position.y, _rb.position.z) + forwardMove * Time.fixedDeltaTime;
        Vector3 newPos = Vector3.Lerp(_rb.position, targetPos, Time.fixedDeltaTime * laneChangeSpeed);
        _rb.MovePosition(newPos);
    }

    private void HandleLaneChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)&& _currentLane > 0)
        {
            _currentLane--;
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _currentLane < 2)
        {
            _currentLane++;
        }
    }

    private void HandleJump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space)&& isGrounded && !isJump)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJump = true;
        }

        if (isGrounded && _rb.velocity.y <= 0.1f)
        {
            isJump = false;
        }
    }

    private void ApplyTiltRotation()
    {
        float targetYrotation = 0;

        if (Mathf.Abs(transform.position.x - _targetPosition.x)>0.01f)
        {
            if (_targetPosition.x > transform.position.x)
            {
                targetYrotation = tiltAngle;
            }
            else
            {
                targetYrotation = -tiltAngle;
            }
        }

        Quaternion targetRotation = Quaternion.Euler(0, targetYrotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness);
    }
}
