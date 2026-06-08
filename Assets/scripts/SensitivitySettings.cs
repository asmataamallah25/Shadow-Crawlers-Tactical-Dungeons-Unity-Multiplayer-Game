using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySettings : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityText;

    private const string SENSITIVITY_KEY = "MouseSensitivity";

    private void Awake()
    {
        if (!sensitivitySlider) sensitivitySlider = GetComponentInChildren<Slider>();
        if (!sensitivityText) sensitivityText = GetComponentInChildren<TMP_Text>();
        sensitivitySlider.minValue = 100f;
        sensitivitySlider.maxValue = 2000f;
        sensitivitySlider.wholeNumbers = true;

        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    private void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        float savedValue = PlayerPrefs.GetFloat(SENSITIVITY_KEY, 800f);
        sensitivitySlider.value = savedValue;
        UpdateUI(savedValue);
        ApplySensitivityToPlayers(savedValue);
    }

    private void UpdateSensitivity(float value)
    {
        UpdateUI(value);
        SaveSettings(value);
        ApplySensitivityToPlayers(value);
    }

    private void UpdateUI(float value)
    {
        if (sensitivityText != null)
        {
            sensitivityText.text = Mathf.RoundToInt(value).ToString();
            LayoutRebuilder.ForceRebuildLayoutImmediate(sensitivityText.rectTransform);
        }
    }

    private void ApplySensitivityToPlayers(float value)
    {
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController pc in players)
        {
            if (pc.photonView != null && pc.photonView.IsMine)
            {
                pc.SetMouseSensitivity(value);
            }
        }
    }

    private void SaveSettings(float value)
    {
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, value);
        PlayerPrefs.Save();
    }
}