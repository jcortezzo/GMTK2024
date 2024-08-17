using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    PlayerControls _inputActions;

    public Vector2 MovementInput { get; private set; }

    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerControls();
            #region Movement
            _inputActions.Player.Movement.performed += ctx => { MovementInput = ctx.ReadValue<Vector2>(); };
            #endregion
        }
        _inputActions.Enable();
    }
}
