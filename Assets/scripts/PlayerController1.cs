using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector] public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        [SerializeField] private TMP_FontAsset nicknameFont;

        private Rigidbody2D rigidbody;
        private SpriteRenderer spriteRenderer;
        private GameManager gameManager;
        private PhotonView photonView;
        private GameObject nicknameObj;
        private static Transform localPlayerTransform;
        private bool IsLocalPlayer => photonView == null || photonView.IsMine;

        private Sprite[] idleSprites;
        private Sprite[] runSprites;
        private Sprite jumpSprite;

        private int currentFrame = 0;
        private float frameTimer = 0f;
        private float frameRate = 0.12f;
        private int playerState = 0;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            photonView = GetComponent<PhotonView>();

            LoadSkinSprites();

            nicknameObj = new GameObject("Nickname");
            nicknameObj.transform.position = transform.position + new Vector3(0, 1.2f, 0);
            var tmp = nicknameObj.AddComponent<TextMeshPro>();
            tmp.text = photonView != null ? photonView.Owner.NickName : "Player";
            tmp.fontSize = 2f;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.sortingOrder = 10;
            if (nicknameFont != null) tmp.font = nicknameFont;
        }

        void Start()
        {
            if (IsLocalPlayer)
            {
                localPlayerTransform = transform;
                GameObject gmObj = GameObject.Find("GameManager");
                if (gmObj != null)
                {
                    gameManager = gmObj.GetComponent<GameManager>();
                }
            }
        }

        private void FixedUpdate()
        {
            if (!IsLocalPlayer) return;
            CheckGround();
        }

        void Update()
        {
            if (nicknameObj != null)
                nicknameObj.transform.position = transform.position + new Vector3(0, 1.2f, 0);

            if (!IsLocalPlayer) return;

            if (Input.GetButton("Horizontal"))
            {
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                playerState = 1;
            }
            else
            {
                if (isGrounded) playerState = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }

            if (!isGrounded) playerState = 2;

            if (facingRight == false && moveInput > 0) Flip();
            else if (facingRight == true && moveInput < 0) Flip();

            AnimateSprite();
        }

        void OnDestroy()
        {
            if (nicknameObj != null)
                Destroy(nicknameObj);
        }

        private void AnimateSprite()
        {
            if (playerState == 2)
            {
                if (jumpSprite != null) spriteRenderer.sprite = jumpSprite;
                return;
            }

            Sprite[] frames = playerState == 1 ? runSprites : idleSprites;
            if (frames == null || frames.Length == 0) return;

            frameTimer += Time.deltaTime;
            if (frameTimer >= frameRate)
            {
                frameTimer = 0f;
                currentFrame = (currentFrame + 1) % frames.Length;
                spriteRenderer.sprite = frames[currentFrame];
            }
        }

        private void LoadSkinSprites()
        {
            int skinIndex = PlayerSkinNetwork.GetSkinIndex(photonView);
            if (skinIndex == CustomSkinUtility.CustomSkinIndex)
            {
                Color bodyA = PlayerSkinNetwork.GetBodyA(photonView);
                Color bodyB = PlayerSkinNetwork.GetBodyB(photonView);
                Color accent = PlayerSkinNetwork.GetAccent(photonView);
                idleSprites = CustomSkinUtility.BuildCustomSprites(Resources.LoadAll<Sprite>("Skin 1/idle"), bodyA, bodyB, accent);
                runSprites = CustomSkinUtility.BuildCustomSprites(Resources.LoadAll<Sprite>("Skin 1/run"), bodyA, bodyB, accent);
                Sprite[] jumpArr = CustomSkinUtility.BuildCustomSprites(Resources.LoadAll<Sprite>("Skin 1/jump"), bodyA, bodyB, accent);
                jumpSprite = jumpArr.Length > 0 ? jumpArr[0] : null;
                return;
            }

            string skinFolder = $"Skin {skinIndex}";
            idleSprites = Resources.LoadAll<Sprite>($"{skinFolder}/idle");
            runSprites = Resources.LoadAll<Sprite>($"{skinFolder}/run");
            Sprite[] defaultJumpArr = Resources.LoadAll<Sprite>($"{skinFolder}/jump");
            jumpSprite = defaultJumpArr.Length > 0 ? defaultJumpArr[0] : null;
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!IsLocalPlayer) return;
            if (other.gameObject.CompareTag("Enemy")) deathState = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsLocalPlayer) return;

            if (other.CompareTag("Enemy"))
            {
                deathState = true;
                return;
            }

            if (other.CompareTag("Coin") && gameManager != null)
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
        }
    }
}