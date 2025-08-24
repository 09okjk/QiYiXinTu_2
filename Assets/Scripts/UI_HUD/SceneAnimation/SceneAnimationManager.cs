using UnityEngine;

namespace UI_HUD.SceneAnimation
{
    public class SceneAnimationManager: MonoBehaviour
    {
        public static SceneAnimationManager Instance;
        public Animator animator;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void TriggerAnimator(bool isTrigger)
        {
            animator.enabled = isTrigger;
        }
        
        public void AnimationFinishedTrigger(string animationName)
        {
            // 触发动画完成事件
        }
    }
}