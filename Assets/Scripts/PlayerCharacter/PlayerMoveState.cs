using Audio;
using Unity.VisualScripting;

namespace PlayerCharacter
{
    public class PlayerMoveState:PlayerState
    {
        public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            AudioManager.Instance.PlayEffectAudio("walk_audio", true);
        }

        public override void Update()
        {
            base.Update();
            Player.SetVelocity(XInput * Player.GetPlayerData().moveSpeed, Rb.linearVelocity.y);

            if (XInput == 0 && YInput == 0)
            {
                StateMachine.ChangeState(Player.IdleState);
            }
        }
        public override void Exit()
        {
            base.Exit();
            AudioManager.Instance.StopEffectAudio();
        }
    }
}