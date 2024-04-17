using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        AmmoManager.updateAmmo += UpdateUI;
    }

    private void OnDisable()
    {
        AmmoManager.updateAmmo -= UpdateUI;
    }

    private void UpdateUI(int clipCount, int currentAmmo)
    {
        text.text = $"{clipCount} / {currentAmmo}";
    }
}
