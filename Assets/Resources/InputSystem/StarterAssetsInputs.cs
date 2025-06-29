using UnityEditor.SceneManagement;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool shoot;
		public event System.Action OnEscPressed;
		public event System.Action OnTabPressed;
		public event System.Action UIOnEscPressed;
		public event System.Action OnPPressed;
		public event System.Action OnVPressed;
		public event System.Action OnRPressed;

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
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}
		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}
		public void OnEsc(InputValue value)
		{
			if (value.isPressed && OnEscPressed != null)
			{
				OnEscPressed.Invoke();
			}
		}
		public void OnTab(InputValue Value)
		{
			if (Value.isPressed)
			{
				OnTabPressed.Invoke();
			}
		}
		public void OnCancel(InputValue Value)
		{
			if (Value.isPressed)
			{
				UIOnEscPressed.Invoke();
			}
		}

		public void OnP(InputValue Value)
		{
			if (Value.isPressed)
			{
				OnPPressed.Invoke();
			}
		}
		public void OnV(InputValue Value)
		{
			if (Value.isPressed)
			{
				OnVPressed.Invoke();
			}
		}
		public void OnR(InputValue Value)
		{
			if (Value.isPressed)
			{
				OnRPressed.Invoke();
			}
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
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
		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}
		public void EscInput(bool newEscState)
		{
			shoot = newEscState;
		}
	}
}