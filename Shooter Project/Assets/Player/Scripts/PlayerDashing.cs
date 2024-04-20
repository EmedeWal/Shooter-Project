using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerDashing : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _dashSpeed = 50;
    [SerializeField] private float _dashDuration = 0.3f;
    [SerializeField] private float _dashCooldown = 3;
    [SerializeField] private int _dashCount = 2;

    private CharacterController _characterController;

    private PlayerMovement _playerMovement;
    private Vector3 _dashDirection;
    private bool _isDashing;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().DashInputPerformed += PlayerDashing_DashInputPerformed;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().DashInputPerformed -= PlayerDashing_DashInputPerformed;
    }

    private void PlayerDashing_DashInputPerformed()
    {
        if (CanDash() && !_isDashing) StartCoroutine(Dash());
    }

    // Note to self. Yield return null prevents Unity from instantly crashing. Don't know why.
    private IEnumerator Dash()
    {
        _isDashing = true;
        _dashCount--;

        float startTime = Time.time;

        while (Time.time < startTime + _dashDuration)
        {
            _dashDirection = _playerMovement.MoveDirection;
            if (_playerMovement.MoveDirection == new Vector3(0, 0, 0)) _dashDirection = transform.forward;
            _characterController.Move(_dashSpeed * Time.deltaTime * _dashDirection);
            yield return null;
        }

        _isDashing = false;

        //Invoke(nameof(EndDash), _dashDuration);
        Invoke(nameof(ResetDash), _dashCooldown);
    }

    //private void EndDash()
    //{
    //    _isDashing = false;
    //}

    private void ResetDash()
    {
        _dashCount++;
    }

    private bool CanDash()
    {
        if (_dashCount > 0) return true;
        return false;
    }
}
