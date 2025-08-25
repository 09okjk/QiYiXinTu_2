using System;
using Core;
using Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerCharacter
{
    [Serializable]
    public class PlayerData
    {
        public string playerName;
        public int Health = 5;
        public int Stamina = 5;
        public int Mana = 5;
        public float moveSpeed = 5f;
        public Vector3 position;
        public Vector3 rotation;
    }
    public class Player:MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        public Rigidbody2D rb;
        public Animator anim;
        public int FacingDirection { get; private set; } = 1;
        protected bool FacingRight = true;

        #region States

        public PlayerStateMachine StateMachine;
        public PlayerMoveState MoveState;
        public PlayerIdleState IdleState;

        #endregion

        #region 生命周期

        private void Awake()
        {
            StateMachine = new PlayerStateMachine();
            MoveState = new PlayerMoveState(this, StateMachine, "Move");
            IdleState = new PlayerIdleState(this, StateMachine, "Idle");
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }
        
        private void Start()
        {
            StateMachine.Initialize(IdleState);
            if (playerData == null)
            {
                LoggerManager.Instance.LogError("Player: Player data is not set.");
            }
            else
            {
                transform.position = playerData.position;
                transform.eulerAngles = playerData.rotation;
            }
        }

        #endregion

        #region Values

        public void SetPlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                LoggerManager.Instance.LogError("Player name cannot be empty.");
                return;
            }
            playerData.playerName = name;
        }
        
        public string GetPlayerName()
        {
            return playerData.playerName;
        }
        
        public void SetData(PlayerData data)
        {
            if (data == null)
            {
                LoggerManager.Instance.LogError("Player data cannot be null.");
                return;
            }
            playerData = data;
        }
        
        public PlayerData GetPlayerData()
        {
            return playerData;
        }
        
        #endregion

        #region 运动相关
        
        /// <summary>
        /// 设置速度 0
        /// </summary>
        public void SetZeroVelocity()
        {
            rb.linearVelocity = Vector2.zero;
        }
    
        /// <summary>
        /// 设置速度 x y
        /// </summary>
        /// <param name="xVelocity"> x 速度 </param>
        /// <param name="yVelocity"> y 速度</param>
        public void SetVelocity(float xVelocity,float yVelocity)
        {
            rb.linearVelocity = new Vector2(xVelocity, yVelocity);
            FlipController(xVelocity);
        }
        
        public virtual void Flip()
        {
            FacingDirection *= -1;
            FacingRight = !FacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    
        public virtual void FlipController(float x)
        {
            if (x > 0 && !FacingRight)
            {
                Flip();
            }
            else if (x < 0 && FacingRight)
            {
                Flip();
            }
        }

        #endregion
        
    }
}