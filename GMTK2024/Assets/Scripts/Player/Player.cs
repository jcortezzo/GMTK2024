using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private PlayerInputController _inputHandler;
    public PlayerMovement _playerMovement;
    private PlayerEating _playerEating;

    protected Planet _currPlanet;

    public bool IsEating { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerEating = GetComponent<PlayerEating>();
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
