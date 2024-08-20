using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    PlayerControls _inputActions;

    private Player _playerManager;
    public PostGameUI _postGame;

    public Vector2 MovementInput { get; private set; }

    public bool EatFlag { get; private set; }
    private void Awake()
    {
        _playerManager = GetComponent<Player>();
    }

    void Start()
    {
        
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
            _inputActions.Player.Eat.performed += ctx => { EatFlag = true; };
            _inputActions.Player.Eat.canceled += ctx => { EatFlag = false; };
            #endregion

            #region Jump
            _inputActions.Player.Jump.performed += ctx => { _playerManager.JumpSquat.Invoke(); };
            _inputActions.Player.Jump.canceled += ctx => { _playerManager.JumpRelease.Invoke(); };
            #endregion

            #region Restart
            _inputActions.Player.Restart.performed += ctx => { _postGame.RestartLevel(); };
            #endregion
        }
        _inputActions.Enable();
    }
}
