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

    private Vector3 dashDirection;
    private bool isDashing;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
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
        if (CanDash() && !isDashing) StartCoroutine(Dash());
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
