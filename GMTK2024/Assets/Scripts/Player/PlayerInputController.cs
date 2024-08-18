using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    PlayerControls _inputActions;

    private Player _playerManager;

    public Vector2 MovementInput { get; private set; }

    void Start()
    {
        _playerManager = GetComponent<Player>();
        MovementInput = Vector2.zero;
    }

    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new PlayerControls();
            #region Movement
            _inputActions.Player.Movement.performed += ctx => { MovementInput = ctx.ReadValue<Vector2>(); };
            _inputActions.Player.Movement.canceled += ctx => { MovementInput = Vector2.zero; };
            #endregion

            #region Eating
            _inputActions.Player.Eat.performed += ctx => { _playerManager.EatEvent.Invoke(); };
            _inputActions.Player.Eat.canceled += ctx => { _playerManager.StopEatEvent.Invoke(); };
            #endregion
        }
        _inputActions.Enable();
    }
}
