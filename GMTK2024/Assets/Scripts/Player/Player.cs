using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private PlayerInputController _inputHandler;
    public PlayerMovement _playerMovement;
    private PlayerEating _playerEating;
    private PlayerJump _playerJump;

    protected Planet _currPlanet;

    public bool IsEating { get; private set; }
    public UnityEvent JumpSquat { get; private set; }
    public UnityEvent JumpRelease { get; private set; }
    public bool IsJumping {
        get { return !_playerJump.IsGround; }
    }

    private void Awake()
    {
        JumpSquat = new UnityEvent();
        JumpRelease = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerEating = GetComponent<PlayerEating>();
        _playerJump = GetComponent<PlayerJump>();

        JumpSquat.AddListener(_playerJump.JumpSquat);
        JumpRelease.AddListener(_playerJump.JumpRelease);
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        if (_inputHandler == null) return;
        _playerMovement?.HandleMovement(delta, _inputHandler.MovementInput);

        IsEating = _inputHandler.EatFlag;
    }
}
