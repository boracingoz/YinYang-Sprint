using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour
{
    [Header("Character Settings")]
    public CharacterSettings characterSettings;
    public LayerMask groundLayer;

    

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
        float targetX = initialXpos + (_currentLane * characterSettings.laneDistance);
        float syncedZ = initialPos.z + SyncManager.Instance.ZProgress;

        float newX = Mathf.Lerp(_rb.position.x, targetX, Time.fixedDeltaTime * characterSettings.laneChangeSpeed);
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
            _rb.AddForce(Vector3.up * characterSettings.jumpingForce, ForceMode.Impulse);
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
        float targetX = initialXpos + (_currentLane * characterSettings.laneDistance);

        if (Mathf.Abs(currentX - targetX) > 0.01f)
        {
            targetYrotation = (targetX > currentX) ? characterSettings.tiltAngel : - characterSettings.tiltAngel;
        }

        Quaternion targetRotation = Quaternion.Euler(0, targetYrotation, 0);
        Quaternion smoothedRotation = Quaternion.Lerp(_rb.rotation, targetRotation, Time.deltaTime * characterSettings.rotationSmoothness);
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
