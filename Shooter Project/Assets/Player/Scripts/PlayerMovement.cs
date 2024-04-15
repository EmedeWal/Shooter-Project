using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerData _playerData;
    private Vector3 _moveDirection;
    private float horizontalInput;
    private float verticalInput;

    private float _moveSpeed;

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
        CalculateMoveDirection();
        MovePlayer();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().MovementInput += PlayerMovement_StoreInput;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().MovementInput -= PlayerMovement_StoreInput;
    }

    private void PlayerMovement_StoreInput(Vector2 movement)
    {
        horizontalInput = movement.x;
        verticalInput = movement.y;
    }

    private void RetrievePlayerData()
    {
        _moveSpeed = _playerData.GetMoveSpeed();
    }

    private void CalculateMoveDirection()
    {
        _moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
    }

    private void MovePlayer()
    {
        _characterController.Move(_moveDirection * _moveSpeed * Time.deltaTime);
    }
}
