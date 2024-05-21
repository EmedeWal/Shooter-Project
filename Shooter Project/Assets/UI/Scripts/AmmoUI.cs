using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        AmmoManager.UpdateAmmo += UpdateUI;
    }

    private void OnDisable()
    {
        AmmoManager.UpdateAmmo -= UpdateUI;
    }

    private void UpdateUI(int clipCount, int currentAmmo)
    {
        _text.text = $"{clipCount} / {currentAmmo}";
    }
}
