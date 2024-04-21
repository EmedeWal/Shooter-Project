using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(CharacterController))]
public class PlayerDashing : MonoBehaviour
{
    [Header("VARIABLES")]
    [SerializeField] private float _dashSpeed = 50;
    [SerializeField] private float _dashDuration = 0.3f;
    [SerializeField] private float _dashCooldown = 3;
    [SerializeField] private int _dashCount = 2;

    private CharacterController _characterController;
    private PlayerManager _playerManager;

    private PlayerData _playerData;
    private Vector3 _dashDirection;
    private bool _isDashing;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerManager = GetComponent<PlayerManager>();
        _playerData = GetComponent<PlayerData>();
    }

    private void OnEnable()
    {
        _playerManager.DashInputPerformed += PlayerDashing_DashInputPerformed;
    }

    private void OnDisable()
    {
        _playerManager.DashInputPerformed -= PlayerDashing_DashInputPerformed;
    }

    private void PlayerDashing_DashInputPerformed()
    {
        if (CanDash() && !_isDashing) StartCoroutine(Dash());
    }

    // Note to self. Yield return null prevents Unity from instantly crashing. Don't know why.
    private IEnumerator Dash()
    {
        _isDashing = true;
        _playerManager.State = PlayerManager.PlayerState.Dashing;

        _dashCount--;

        float startTime = Time.time;

        while (Time.time < startTime + _dashDuration)
        {
            _dashDirection = _playerData.GetMovementDirection();
            if (_dashDirection == Vector3.zero) _dashDirection = transform.forward;
            _characterController.Move(_dashSpeed * Time.deltaTime * _dashDirection);
            yield return null;
        }

        _isDashing = false;
        _playerManager.State = PlayerManager.PlayerState.Idle;

        Invoke(nameof(ResetDash), _dashCooldown);
    }

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
