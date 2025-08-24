namespace UI_HUD.SceneAnimation
{
    public class SceneAnimationController
    {
        public void AnimationFinished(string animationName)
        {
            SceneAnimationManager.Instance.AnimationFinishedTrigger(animationName);
        }
    }
}