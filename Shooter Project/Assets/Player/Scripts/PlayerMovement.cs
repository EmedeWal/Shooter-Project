using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _moveSpeed = 15;

    private CharacterController _characterController;
    private PlayerData _playerData;
    private Vector3 _movementDirection;
    private float horizontalInput;
    private float verticalInput;

    public Vector3 MoveDirection => _movementDirection;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerData = GetComponent<PlayerData>();
    }

    private void Update()
    {
        CalculateMoveDirection();
        MovePlayer();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().MovementInputValue += PlayerMovement_MovementInputValue;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().MovementInputValue -= PlayerMovement_MovementInputValue;
    }

    private void PlayerMovement_MovementInputValue(Vector2 movement)
    {
        horizontalInput = movement.x;
        verticalInput = movement.y;
    }

    private void CalculateMoveDirection()
    {
        _movementDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        _movementDirection = _movementDirection.normalized;
        _playerData.SetMovementDirection(_movementDirection);
    }

    private void MovePlayer()
    {
        _characterController.Move(_moveSpeed * Time.deltaTime * _movementDirection);
    }
}
