using UnityEngine;
using TMP_InputField = TMPro.TMP_InputField;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header("UI Panels")]
    [SerializeField] private GameObject Kodpanel;

    [Header("Input")]
    [SerializeField] private TMP_InputField roomCodeInput;

    [Header("Nickname")]
    [SerializeField] private TMP_InputField nicknameInput;

    private Dictionary<string, RoomInfo> _cachedRooms = new Dictionary<string, RoomInfo>();

    private void Start()
    {
        if (Kodpanel != null) Kodpanel.SetActive(false);

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = "1.0";
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        if (nicknameInput != null)
            nicknameInput.text = PlayerPrefs.GetString("PlayerName", "");
    }

    public void ToggleKodpanel()
    {
        if (Kodpanel == null) return;

        if (!Kodpanel.activeSelf)
        {
            Kodpanel.SetActive(true);
        }
        else
        {
            bool inputIsEmpty = roomCodeInput == null || string.IsNullOrWhiteSpace(roomCodeInput.text);
            if (inputIsEmpty)
                Kodpanel.SetActive(false);
        }
    }

    public void SaveNickname()
    {
        if (nicknameInput == null) return;

        string nick = nicknameInput.text.Trim();
        if (string.IsNullOrEmpty(nick)) return;

        PlayerPrefs.SetString("PlayerName", nick);
        PhotonNetwork.NickName = nick;
    }
}