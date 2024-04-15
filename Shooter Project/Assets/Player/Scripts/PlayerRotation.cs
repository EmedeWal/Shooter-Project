using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;

    private PlayerData _playerData;
    private Vector2 _rotation;
    private float _horizontalRotation;
    private float _verticalRotation;

    private float _horizontalSensitivity = 300;
    private float _verticalSensitivity = 300;
    private float _verticalClamp = 75;

    private void Awake()
    {
        _playerData = GetComponent<PlayerData>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        RetrievePlayerData();
    }

    private void Update()
    {
        CalculateRotation();
        RotatePlayer();
        RotateCamera();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().RotationInput += PlayerRotation_StoreInput;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().RotationInput -= PlayerRotation_StoreInput;
    }

    private void PlayerRotation_StoreInput(Vector2 rotation)
    {
        _rotation = rotation;
    }

    private void RetrievePlayerData()
    {
        _horizontalSensitivity = _playerData.GetHorizontalSensitivity();
        _verticalSensitivity = _playerData.GetVerticalSensitivity();
        _verticalClamp = _playerData.GetVerticalClamp();    
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
