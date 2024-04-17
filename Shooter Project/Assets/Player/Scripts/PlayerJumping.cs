using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerJumping : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _jumpForce = 20;
    [SerializeField] private float _gravity = 30;

    [Header("REFERENCES")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    private CharacterController _characterController;
    private Vector3 _velocity;
    private bool _canJump = true;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>(); 
    }

    private void Update()
    {
        ManageVerticalPosition();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().JumpInputPerformed += PlayerJumping_JumpInputPerformed;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().JumpInputPerformed -= PlayerJumping_JumpInputPerformed;
    }

    private void PlayerJumping_JumpInputPerformed()
    {
        if (_canJump) Jump();
    }

    private void Jump()
    {
        if (!IsGrounded()) _canJump = false;
        _velocity.y = _jumpForce;
    }

    private void ResetJump()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        bool isGrounded = Physics.CheckSphere(_groundCheck.position, 0.2f, _groundLayer);
        if (isGrounded && !_canJump) ResetJump();
        return isGrounded;  
    }

    private void ManageVerticalPosition()
    {
        if (IsGrounded() && _velocity.y < 0) _velocity.y = -0.2f;

        _velocity.y -= _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }
}
