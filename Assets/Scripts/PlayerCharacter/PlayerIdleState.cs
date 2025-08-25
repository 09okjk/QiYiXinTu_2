namespace PlayerCharacter
{
    public class PlayerIdleState:PlayerState
    {
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }
    }
}