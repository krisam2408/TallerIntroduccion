using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region References
    private CharacterController m_controller;
    [Header("References")]
    [SerializeField] private Animator m_animator;
    #endregion

    #region Inputs
    private Vector3 m_appliedMovement;
    private float AppliedMovementX { get { return m_appliedMovement.x; } set { m_appliedMovement.x = value; } }
    private float AppliedMovementY { get { return m_appliedMovement.y; } set { m_appliedMovement.y = value; } }
    private bool m_isJumping;
    private bool m_isCurrentJumping;
    #endregion

    #region Variables
    [SerializeField] private float m_movementFactor;
    [SerializeField] private float m_rotationFactor;
    [SerializeField] private float m_jumpTime;
    [SerializeField] private float m_jumpHeight;
    private float m_minGravity = -0.1f;
    private float m_initialJumpForce;
    private float m_jumpForce;
    private float m_fallForce;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        m_controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        m_appliedMovement = Vector3.zero;

        float timeToApex = m_jumpHeight * 0.5f;
        m_initialJumpForce = 2f * m_jumpHeight / timeToApex;
        m_jumpForce = -2f * m_jumpHeight / Mathf.Pow(timeToApex, 2);
        m_fallForce = 2f * m_jumpForce;
    }


    private void Update()
    {
        HandleInputs();

        HandleRun();

        HandleGravity();

        m_controller.Move(m_appliedMovement * Time.deltaTime * m_movementFactor);
    }
    #endregion

    private void HandleInputs()
    {
        AppliedMovementX = Input.GetAxisRaw("Horizontal");

        m_isJumping = Input.GetButtonDown("Jump");
    }

    private void HandleRun()
    {
        bool isMoving = AppliedMovementX != 0;

        if(isMoving)
        {
            Quaternion targetRotation = SetTargetRotation(AppliedMovementX);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationFactor * Time.deltaTime);
        }

        m_animator.SetBool("IsMoving", isMoving);
    }

    private Quaternion SetTargetRotation(float xDirection)
    {
        if(xDirection < 0)
            return Quaternion.Euler(0f, 180f, 0f);
        return Quaternion.identity;
    }

    private void HandleGravity()
    {
        if(m_controller.isGrounded && !m_isJumping)
        {
            m_isCurrentJumping = false;
            AppliedMovementY = m_minGravity;
        }
        else if(m_isCurrentJumping && AppliedMovementY < 0.1f)
        {
            float previousYSpeed = AppliedMovementY;
            float nextYSpeed = AppliedMovementY + (m_fallForce * Time.deltaTime);
            float avgYSpeed = (previousYSpeed + nextYSpeed) * 0.5f;
            AppliedMovementY = avgYSpeed;
        }
        else if(m_isJumping && m_controller.isGrounded)
        {
            m_isCurrentJumping = true;
            float previousYSpeed = AppliedMovementY;
            float nextYSpeed = AppliedMovementY + m_initialJumpForce;
            float avgYSpeed = (previousYSpeed + nextYSpeed) * 0.5f;
            AppliedMovementY = avgYSpeed;
        }
        else if(m_isCurrentJumping)
        {
            float previousYSpeed = AppliedMovementY;
            float nextYSpeed = AppliedMovementY + (m_jumpForce * Time.deltaTime);
            float avgYSpeed = (previousYSpeed + nextYSpeed) * 0.5f;
            AppliedMovementY = avgYSpeed;
        }
        else if(!m_isCurrentJumping && !m_controller.isGrounded)
        {
            float previousYSpeed = AppliedMovementY;
            float nextYSpeed = AppliedMovementY + (m_fallForce * Time.deltaTime);
            float avgYSpeed = (previousYSpeed + nextYSpeed) * 0.5f;
            AppliedMovementY = avgYSpeed;
        }

        m_animator.SetBool("IsJumping", m_isCurrentJumping);
    }

}