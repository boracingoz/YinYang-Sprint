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
    [SerializeField] private Vector3 initialPos;
    private float initialXpos;

    [Header("Rotation Settings")]
    public float tiltAngle = 15f;
    public float rotationSmoothness = 5f;

    private void Awake()
    {
       transform.position = initialPos;
       initialXpos = initialPos.x;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currentLane = 0;
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
        float targetX = initialXpos + (_currentLane * laneDistance);
        Vector3 targetPos = new Vector3(targetX, _rb.position.y, _rb.position.z);
        Vector3 newPos = Vector3.Lerp(_rb.position, targetPos, Time.fixedDeltaTime * laneChangeSpeed);
        _rb.MovePosition(newPos);
    }

    private void HandleLaneChange()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)&& _currentLane > -1)
        {
            _currentLane--;
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _currentLane < 1)
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
        float currentX = transform.position.x;
        float targetX = initialXpos + (_currentLane * laneDistance);

        if (Mathf.Abs(currentX - targetX) > 0.01f)
        {
            if (targetX > currentX)
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

    public void UpdateZpos(float zOffset)
    {
        Vector3 pos = _rb.position;
        pos.z = initialPos.z + zOffset;
        _rb.MovePosition(pos);
    }

    public int GetCurrentLane()
    {
        return _currentLane;
    }

}
