using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public class InputManager : MonoBehaviour
    {

        // W is +1y, S is -1y, A is -1x, D is +1x
        private Vector2 movement;
        private Vector2 aim;
        private bool jump;
        private bool sprint;
        private bool crouch;
        private bool shoot;
        private bool slide;
        private bool aimDownSight;

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    public void OnMovement(InputValue value)
    {
        MovementInput(value.Get<Vector2>());
    }

    public void OnAim(InputValue value)
    {
        AimtInput(value.Get<Vector2>());
    }
    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    public void OnCrouch(InputValue value)
    {
        CrouchInput(value.isPressed);
    }
    public void OnSlide(InputValue value)
    {
        SlideInput(value.isPressed);
    }
    public void OnAimDownSight(InputValue value)
    {
        AimDownSightInput(value.isPressed);
    }

        private void MovementInput(Vector2 value)
        {
            movement = value;
        }

        private void JumpInput(bool value)
        {
            jump = value;
        }
        private void AimtInput(Vector2 value)
        {
            aim = value;
        }
        private void ShootInput(bool value)
        {
            shoot = value;
        }
        private void SprintInput(bool value)
        {
            sprint = value;
        }
        private void CrouchInput(bool value)
        {
            crouch = value;
        }
        private void SlideInput(bool value)
        {
            slide = value;
        }
        private void AimDownSightInput(bool value)
        {
            aimDownSight = value;
        }


        public Vector2 Movement
        {
            get { return movement; }
        }
        public Vector2 Aim
        {
            get { return aim; }
        }
        public bool Jump
        {
            get { return jump; }
        }
        public bool Sprint
        {
            get { return sprint; }
        }
        public bool Crouch
        {
            get { return crouch; }
        }
        public bool Slide
        {
            get { return slide; }
        }
        public bool Shoot
        {
            get { return shoot; }
        }
        public bool AimDownSight
        {
            get { return aimDownSight; }
        }
}