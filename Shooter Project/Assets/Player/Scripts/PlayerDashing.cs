using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class PlayerDashing : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerData _playerData;

    private float _dashSpeed;
    private float _dashDuration;
    private float _dashCooldown;
    private int _dashCount;

    private Vector3 dashDirection;
    private bool isDashing;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerData = GetComponent<PlayerData>();
    }

    private void Start()
    {
        RetrievePlayerData();
    }

    private void OnEnable()
    {
        GetComponent<PlayerManager>().DashInput += PlayerDashing_StoreInput;
    }

    private void OnDisable()
    {
        GetComponent<PlayerManager>().DashInput -= PlayerDashing_StoreInput;
    }

    private void PlayerDashing_StoreInput()
    {
        if (CanDash() && !isDashing) StartCoroutine(Dash());
    }

    private void RetrievePlayerData()
    {
        _dashSpeed = _playerData.GetDashSpeed();
        _dashDuration = _playerData.GetDashDuration();
        _dashCooldown = _playerData.GetDashCooldown();
        _dashCount = _playerData.GetDashCount();    
    }

    // Note to self. Yield return null prevents Unity from instantly crashing. Don't know why.
    private IEnumerator Dash()
    {
        isDashing = true;
        _dashCount--;

        float startTime = Time.time;

        while (Time.time < startTime + _dashDuration)
        {
            _characterController.Move(_dashSpeed * Time.deltaTime * transform.forward);
            yield return null;
        }

        isDashing = false;

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
