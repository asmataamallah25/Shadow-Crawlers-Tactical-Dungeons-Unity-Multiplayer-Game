using UnityEngine;
using Photon.Pun;

public class PlayerDeathDetector : MonoBehaviour
{
    private PlayerController _pc;
    private PhotonView _pv;

    void Start()
    {
        _pc = GetComponent<PlayerController>();
        _pv = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_pc == null || _pv == null) return;

        if (!_pv.IsMine) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            var deathStateComponent = GetComponent<Platformer.PlayerDeathState>();
            if (deathStateComponent == null)
            {
                gameObject.AddComponent<Platformer.PlayerDeathState>();
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_pc == null || _pv == null) return;

        if (!_pv.IsMine) return;

        if (hit.gameObject.CompareTag("Enemy"))
        {
            var deathStateComponent = GetComponent<Platformer.PlayerDeathState>();
            if (deathStateComponent == null)
            {
                gameObject.AddComponent<Platformer.PlayerDeathState>();
            }
        }
    }
}