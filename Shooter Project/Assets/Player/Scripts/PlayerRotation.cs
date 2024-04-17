using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _horizontalSensitivity = 125;
    [SerializeField] private float _verticalSensitivity = 125;
    [SerializeField] private float _verticalClamp = 70;

    [Header("REFERENCES")]
    [SerializeField] private Transform _playerCamera;

    private Vector2 _rotation;
    private float _horizontalRotation;
    private float _verticalRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CalculateRotation();
        RotatePlayer();
        RotateCamera();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().RotationInputValue += PlayerRotation_RotationInputValue;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().RotationInputValue -= PlayerRotation_RotationInputValue;
    }

    private void PlayerRotation_RotationInputValue(Vector2 rotation)
    {
        _rotation = rotation;
    }

    private void CalculateRotation()
    {
        float rotationX = _rotation.x * Time.deltaTime * _horizontalSensitivity;
        float rotationY = _rotation.y * Time.deltaTime * _verticalSensitivity;

        _horizontalRotation += rotationX;
        _verticalRotation -= rotationY;

        _verticalRotation = Mathf.Clamp(_verticalRotation, -_verticalClamp, _verticalClamp);
    }

    private void RotatePlayer()
    {
        transform.rotation = Quaternion.Euler(0f, _horizontalRotation, 0f);

    }

    private void RotateCamera()
    {
        _playerCamera.rotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0f);
    }
}
