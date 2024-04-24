using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    private Color newColor;

    private Image crosshair;

    private void Awake()
    {
        crosshair = GetComponent<Image>();
    }

    private void OnEnable()
    {
        GunStateManager.GunStateUpdate += CrosshairUI_GunStateUpdate;
    }

    private void OnDisable()
    {
        GunStateManager.GunStateUpdate -= CrosshairUI_GunStateUpdate;
    }

    private void CrosshairUI_GunStateUpdate(GunStateManager.GunState newState)
    {
        DetermineColor(newState);
    }

    private void DetermineColor(GunStateManager.GunState newState)
    {
        // Green coloured states
        if (newState == GunStateManager.GunState.Idle)
        {
            newColor = colors[0];
        }
        // Yellow coloured states
        else if (newState== GunStateManager.GunState.Recharging)
        {
            newColor = colors[1];
        }
        // Red coloured states is all else
        else
        {
            newColor= colors[2];
        }

        ApplyColor();
    }

    private void ApplyColor()
    {
        crosshair.color = newColor;
    }
}
