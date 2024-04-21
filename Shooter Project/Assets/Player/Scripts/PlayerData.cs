using System;
using UnityEngine;

public class PlayerData : MonoBehaviour 
{
    #region Movement
    private MovementData _movementData;

    public Vector3 GetMovementDirection()
    {
        return _movementData.MovementDirection;
    }

    public void SetMovementDirection(Vector3 newDirection)
    {
        _movementData.MovementDirection = newDirection;
    }

    //public float GetMoveSpeed()
    //{
    //    return _movementData.MoveSpeed;
    //}
    #endregion

    #region Guns
    [SerializeField] private GunData _gunData;

    public GameObject[] GetGuns()
    {
        return _gunData.Guns;
    }


    public GameObject GetCurrentGun()
    {
        return _gunData.CurrentGun;
    }

    public void SetCurrentGun(GameObject currentWeapon)
    {
        _gunData.CurrentGun = currentWeapon;
    }

    #endregion
}

public struct MovementData
{
    //[Range(10, 20)] public float MoveSpeed;
    public Vector3 MovementDirection;
}

[Serializable]
public struct GunData
{
    public GameObject[] Guns;

    [HideInInspector] public GameObject CurrentGun;
}

//[Serializable]
//public struct RotationData
//{
//    [Range(100, 400)] public float HorizontalSensitivity;
//    [Range(100, 400)] public float VerticalSensitivity;
//    [Range(50, 90)] public float VerticalClamp;    
//}

//[Serializable]
//public struct JumpData
//{
//    [Range(10, 30)] public float JumpForce;
//    [Range(10, 50)] public float Gravity;
//}

//[Serializable]
//public struct DashData
//{
//    [Range(25, 75)] public float DashSpeed;
//    [Range(0.1f, 0.5f)] public float DashDuration;
//    [Range(1, 5)] public float DashCooldown;
//    [Range(1, 3)] public int DashCount;
//}


//#region Rotating
//[SerializeField] private RotationData _rotationData;

//public float GetHorizontalSensitivity()
//{
//    return _rotationData.HorizontalSensitivity;
//}

//public float GetVerticalSensitivity()
//{
//    return _rotationData.VerticalSensitivity;
//}

//public float GetVerticalClamp()
//{
//    return _rotationData.VerticalClamp;
//}
//#endregion

//#region Jumping
//[SerializeField] private JumpData _jumpData;

//public float GetJumpForce()
//{
//    return _jumpData.JumpForce;
//}

//public float GetGravity()
//{
//    return _jumpData.Gravity;
//}
//#endregion

//#region Dashing
//[SerializeField] private DashData _dashData;

//public float GetDashSpeed()
//{
//    return (_dashData.DashSpeed);
//}

//public float GetDashDuration()
//{
//    return _dashData.DashDuration;
//}

//public float GetDashCooldown()
//{
//    return _dashData.DashCooldown;
//}

//public int GetDashCount()
//{
//    return _dashData.DashCount;
//}
//#endregion