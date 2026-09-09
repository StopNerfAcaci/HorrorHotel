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

        }

        public override void LogicUpdate()
        {
            if (_controller == null) return;

            ApplyGravity();
            CurrentVelocity = new Vector3( _controller.velocity.x, _verticalVelocity, _controller.velocity.z );
        }
        private void ApplyGravity()
        {
            if (_controller.isGrounded)
            {
                _verticalVelocity = -2f;
            }
            else
            {
                _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            }
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