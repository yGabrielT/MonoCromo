using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
        public float scroll;
        public bool jump;
		public bool interact;
        public bool aim;
        public bool slow;
        public bool sprint;
        public bool shoot;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;


#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnScroll(InputValue value)
		{
			ScrollInput(value.Get<float>());
		}


		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }

        public void OnSlow(InputValue value)
        {
            SlowInput(value.isPressed);
        }

		public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

        public void ScrollInput(float newScrollDirection)
        {
            scroll = newScrollDirection;
        }

        public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }
        public void SlowInput(bool newSlowState)
        {
            slow = newSlowState;
        }

        public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}