using UnityEngine;
using UnityEngine.UI;

public class CoinsDisplay : MonoBehaviour
{
    [SerializeField] private Text coinsText;

    void Start()
    {
        UpdateCoinsText();
    }

    void OnEnable()
    {
        UpdateCoinsText();
    }

    void Update()
    {
        UpdateCoinsText();
    }

    void UpdateCoinsText()
    {
        if (coinsText == null) return;
        coinsText.text = PlayerPrefs.GetInt("TotalCoins", 0).ToString();
    }
}