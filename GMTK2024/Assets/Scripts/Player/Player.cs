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
    public PlayerStat PlayerStat { get; private set; }
    protected Planet _currPlanet;

    public bool IsEating { get; private set; }
    public UnityEvent JumpSquat { get; private set; }
    public UnityEvent JumpRelease { get; private set; }
    public bool IsJumping
    {
        get { return !_playerJump.IsGround && _playerJump.HasJumped; }
    }

    private void Awake()
    {
        JumpSquat = new UnityEvent();
        JumpRelease = new UnityEvent();
        PlayerStat = GetComponent<PlayerStat>();
        _inputHandler = GetComponent<PlayerInputController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerEating = GetComponent<PlayerEating>();
        _playerJump = GetComponent<PlayerJump>();
    }

    // Start is called before the first frame update
    void Start()
    {
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
