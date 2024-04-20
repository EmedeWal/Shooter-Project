using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _moveSpeed = 15;

    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private float horizontalInput;
    private float verticalInput;

    public Vector3 MoveDirection => _moveDirection;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
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
        _moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
    }

    private void MovePlayer()
    {
        _characterController.Move(_moveSpeed * Time.deltaTime * _moveDirection);
    }
}
