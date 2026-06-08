using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomCodeDisplay : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text roomCodeText;

    public override void OnEnable()
    {
        base.OnEnable();
        UpdateDisplay();
    }

    public override void OnJoinedRoom() => UpdateDisplay();

    public override void OnLeftRoom() => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!roomCodeText) return;

        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom != null)
        {
            roomCodeText.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
        }
        else
        {
            roomCodeText.text = "Not in a room";
        }
    }
}