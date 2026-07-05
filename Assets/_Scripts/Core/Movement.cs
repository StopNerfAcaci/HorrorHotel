using System;
using UnityEngine;

namespace Gameplay.CoreSystem
{
    public class Movement : CoreComponents
    {
        #region Components

        public CharacterController _controller;
        public bool CanSetVelocity { get; set; }
        public Vector3 CurrentVelocity { get; private set; }
        private Transform rootTransform;

        private Vector3 currentTargetRot;
        private Vector3 timeToReachTargetRot;
        private float dampedRotateVelocity;
        private float dampedRotateVelocityPassedTime;
        private float _verticalVelocity;

        #endregion

        #region NPC

        private Vector3 prevPosition;

        #endregion

        #region Set Velocity

        protected override void Awake()
        {
            base.Awake();
            _controller = GetComponentInParent<CharacterController>();

            rootTransform = GetRoot();
            CanSetVelocity = true;
            prevPosition = rootTransform.position;

            timeToReachTargetRot.y = .14f;
        }

        public override void LogicUpdate()
        {
            if (_controller == null) return;

            CurrentVelocity = _controller.velocity;
        }

        public void SetVelocityZero()
        {
            SetFinalVelocity(Vector3.zero);
        }

        public void SetVelocityXZ(Vector3 worldDirection, float speed)
        {
            Vector3 velocity = worldDirection.normalized * speed + new Vector3(0.0f, _verticalVelocity, 0.0f);
            SetFinalVelocity(velocity);
        }

        public void SetVelocityY(float value)
        {
            _verticalVelocity = value;
            if (_verticalVelocity < 0)
            {
                _verticalVelocity = -2;
            }

            Vector3 velocity = new Vector3(CurrentVelocity.x, value, CurrentVelocity.z);
            SetFinalVelocity(velocity);
        }

        private void SetFinalVelocity(Vector3 velocity)
        {
            CurrentVelocity = velocity;
            if (_controller == null)
            {
                Debug.LogError("Movement requires a CharacterController on this object or one of its parents.", this);
                return;
            }

            _controller.Move(velocity * Time.fixedDeltaTime);
        }

        private Transform GetRoot()
        {
            rootTransform = transform;
            while (rootTransform.parent != null)
            {
                rootTransform = rootTransform.parent;
            }

            return rootTransform;
        }

        #endregion
    }
}