using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerCharacter
{
    public class PlayerState
    {
        protected PlayerStateMachine StateMachine;
        protected Player Player;
        protected Rigidbody2D Rb;
        protected PlayerInput PlayerInput;

        protected float XInput;
        protected float YInput;
        private string _animBoolName;

        protected float StateTimer;
        protected bool TriggerCalled;

        public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
        {
            this.Player = player;
            this.StateMachine = stateMachine;
            this._animBoolName = animBoolName;
        
            PlayerInput = player.GetComponent<PlayerInput>();
        }
    
        public virtual void Enter()
        {
            Player.anim.SetBool(_animBoolName, true);
            Rb = Player.rb;
            TriggerCalled = false;
        }

        public virtual void Update()
        {
            StateTimer -= Time.deltaTime;
        
            // 使用新输入系统
            if (PlayerInput != null)
            {
                // 假设你在输入动作中有名为 "move" 的动作
                Vector2 moveInput = PlayerInput.actions["move"].ReadValue<Vector2>();
                XInput = moveInput.x;
                YInput = moveInput.y;
                // Debug.Log(xInput + " " + yInput);
            }
            else
            {
                Debug.LogError("PlayerInput component is missing on the Player GameObject.");
            }
        
            Player.anim.SetFloat("yVelocity", Rb.linearVelocity.y);
        }
    
        public virtual void Exit()
        {
            Player.anim.SetBool(_animBoolName, false);
        }
    
        public virtual void AnimationFinishTrigger()
        {
            TriggerCalled = true;
        }
        
    }
}