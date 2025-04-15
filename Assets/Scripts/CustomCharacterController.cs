using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;

    [Header("Rotation Settings")]
    public float tiltAngle = 15f;
    public float rotationSmoothness = 5f;

    private int _currentLane = 1;
    private Rigidbody _rb;
    private bool isGrounded = true;
    private bool isJump = false;
    private Animator animator;

    [SerializeField] private Vector3 initialPos;
    private float initialXpos;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (initialPos == Vector3.zero)
            initialPos = transform.position;

        initialXpos = initialPos.x;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        _currentLane = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLaneChange();
        HandleJump();
        ApplyTiltRotation();
        CharacterAnim();
    }

    private void FixedUpdate()
    {
        float targetX = initialXpos + (_currentLane * laneDistance);
        float syncedZ = initialPos.z + SyncManager.Instance.ZProgress;

        float newX = Mathf.Lerp(_rb.position.x, targetX, Time.fixedDeltaTime * laneChangeSpeed);
        float newZ = Mathf.Lerp(_rb.position.z, syncedZ, Time.fixedDeltaTime * 10f);

        Vector3 newPos = new Vector3(newX, _rb.position.y, newZ);
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
            targetYrotation = (targetX > currentX) ? tiltAngle : -tiltAngle;
        }

        Quaternion targetRotation = Quaternion.Euler(0, targetYrotation, 0);
        Quaternion smoothedRotation = Quaternion.Lerp(_rb.rotation, targetRotation, Time.deltaTime * rotationSmoothness);
        _rb.MoveRotation(smoothedRotation);
    }

    public int GetCurrentLane()
    {
        return _currentLane;
    }

    public void CharacterAnim()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("SwordAttack");
        }
    }
}
