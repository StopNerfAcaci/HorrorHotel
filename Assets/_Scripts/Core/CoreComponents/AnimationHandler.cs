using UnityEngine;

namespace Gameplay.CoreSystem
{
    public class AnimationHandler : CoreComponents
    {
        private const float crossFadeDuration = 0.15f;
        [SerializeField] private Animator _animator;

        public void SmoothChangeAnim(string animName)
        {
            _animator.enabled = true;
            _animator.CrossFade(animName, crossFadeDuration);
        }

        public void StopAnim()
        {
            _animator.Play("Idle");
            _animator.Update(0);
            _animator.enabled = false;
        }
    }
}

