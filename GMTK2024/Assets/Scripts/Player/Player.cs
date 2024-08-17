using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputController _inputHandler;
    private PlayerMovement _playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputController>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        if (_inputHandler == null) return;
        _playerMovement?.HandleMovement(delta, _inputHandler.MovementInput);
    }
}
