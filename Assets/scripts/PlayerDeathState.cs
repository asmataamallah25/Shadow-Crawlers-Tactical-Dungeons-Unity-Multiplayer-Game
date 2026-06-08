using UnityEngine;

namespace Platformer
{
    public class PlayerDeathState : MonoBehaviour
    {
        public float jumpForce = 5f;

        private Rigidbody rigidbody3D;

        void Start()
        {
            rigidbody3D = GetComponent<Rigidbody>();
            if (rigidbody3D == null)
            {
                rigidbody3D = gameObject.AddComponent<Rigidbody>();
            }

            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
            }

            rigidbody3D.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}