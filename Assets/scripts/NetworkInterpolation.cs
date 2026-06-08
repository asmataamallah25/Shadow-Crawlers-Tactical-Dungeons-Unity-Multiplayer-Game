using UnityEngine;
using Photon.Pun;

public class NetworkInterpolation : MonoBehaviourPun, IPunObservable
{
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    [SerializeField] public float positionLerp = 15f;
    [SerializeField] public float rotationLerp = 10f;
    [SerializeField] private float maxPositionError = 1f;
    private float lastUpdateTime;

    private void Awake()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            targetPosition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
            lastUpdateTime = Time.time;

            if (Vector3.Distance(transform.position, targetPosition) > maxPositionError)
            {
                transform.position = targetPosition;
            }
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            float timeSinceUpdate = Time.time - lastUpdateTime;
            float factor = Mathf.Clamp01(timeSinceUpdate * positionLerp);

            transform.position = Vector3.Lerp(transform.position, targetPosition, factor);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, factor);
        }
    }
}