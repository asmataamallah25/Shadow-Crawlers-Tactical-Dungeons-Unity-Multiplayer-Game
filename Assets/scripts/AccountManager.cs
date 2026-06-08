using System;
using Photon.Pun;
using UnityEngine;

public static class AccountManager
{
    public static bool IsLoggedIn => true;
    public static string CurrentUsername => PlayerPrefs.GetString("PlayerName", "Player");
    public static event Action ProgressChanged;

    public static void Configure(string accountServerUrl, MonoBehaviour coroutineRunner)
    {
        EnsureRuntimeDefaults();
    }

    public static void AutoLoad() => EnsureRuntimeDefaults();

    public static void Logout()
    {
        ResetGuestProgress();
        PlayerPrefs.Save();
        NotifyProgressChanged();
    }

    public static void SaveProgress()
    {
        PlayerPrefs.Save();
    }

    public static void EnsureRuntimeDefaults()
    {
        PlayerPrefs.SetInt("SkinOwned_1", 1);
        if (!PlayerPrefs.HasKey("SelectedSkin"))
            PlayerPrefs.SetInt("SelectedSkin", 1);
        
        PlayerPrefs.Save();
    }

    private static void ResetGuestProgress()
    {
        PlayerPrefs.SetInt("TotalCoins", 0);
        PlayerPrefs.SetInt("SelectedSkin", 1);
        PlayerPrefs.SetInt("SkinOwned_1", 1);
        PlayerPrefs.DeleteKey("SkinOwned_2");
        PlayerPrefs.DeleteKey("SkinOwned_3");
        PlayerPrefs.DeleteKey("SkinBoughtAt_2");
        PlayerPrefs.DeleteKey("SkinBoughtAt_3");
        CustomSkinUtility.ClearSkins();
        PlayerPrefs.DeleteKey("Promo_TEST_Used");
        PlayerPrefs.DeleteKey("PlayerName");
        PhotonNetwork.NickName = "";
        PlayerSkinNetwork.ApplyLocalPlayerProperties();
    }

    public static void NotifyProgressChanged()
    {
        ProgressChanged?.Invoke();
    }
}