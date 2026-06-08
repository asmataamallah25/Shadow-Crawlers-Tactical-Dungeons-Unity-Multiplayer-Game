using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionStatus : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private GameObject[] connectedOnlyObjects;

    private void Start()
    {
        UpdateStatus();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        bool connected = false;
        switch (PhotonNetwork.NetworkClientState)
        {
            case ClientState.Disconnected:
                if (statusText != null)
                {
                    statusText.text = "No Connection";
                    statusText.color = Color.red;
                }
                break;
            case ClientState.ConnectingToNameServer:
            case ClientState.ConnectingToMasterServer:
            case ClientState.Authenticating:
                if (statusText != null)
                {
                    statusText.text = "Connecting...";
                    statusText.color = Color.yellow;
                }
                break;
            case ClientState.ConnectedToMasterServer:
            case ClientState.JoinedLobby:
                if (statusText != null)
                {
                    statusText.text = "Connected";
                    statusText.color = Color.green;
                }
                connected = true;
                break;
            default:
                if (statusText != null) statusText.text = "";
                break;
        }

        SetConnectedOnlyObjects(connected);
    }

    public override void OnConnectedToMaster() => UpdateStatus();
    public override void OnJoinedLobby() => UpdateStatus();
    
    public override void OnDisconnected(DisconnectCause cause)
    {
        if (statusText != null)
        {
            statusText.text = "Connection Error";
            statusText.color = Color.red;
        }
        SetConnectedOnlyObjects(false);
    }

    private void SetConnectedOnlyObjects(bool visible)
    {
        if (connectedOnlyObjects == null) return;
        foreach (var obj in connectedOnlyObjects)
            if (obj) obj.SetActive(visible);
    }
}