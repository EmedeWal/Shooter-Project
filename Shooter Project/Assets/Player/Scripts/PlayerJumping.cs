using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerJumping : MonoBehaviour
{
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    private CharacterController _characterController;
    private PlayerData _playerData;
    private Vector3 _velocity;
    private bool _canJump = true;

    private float _jumpForce;
    private float _gravity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerData = GetComponent<PlayerData>();   
    }

    private void Start()
    {
        RetrievePlayerData();
    }

    private void Update()
    {
        ManageVerticalPosition();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().JumpInput += PlayerJumping_StoreInput;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().JumpInput -= PlayerJumping_StoreInput;
    }

    private void PlayerJumping_StoreInput()
    {
        if (_canJump) Jump();
    }

    private void RetrievePlayerData()
    {
        _jumpForce = _playerData.GetJumpForce();
        _gravity = _playerData.GetGravity();
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
