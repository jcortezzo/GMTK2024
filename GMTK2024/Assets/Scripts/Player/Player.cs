using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private PlayerInputController _inputHandler;
    public PlayerMovement PlayerMovement { get; private set; }
    private PlayerEating _playerEating;

    protected Planet _currPlanet;

    public UnityEvent EatEvent { get; private set; }
    public UnityEvent StopEatEvent { get; private set; }

    void Awake()
    {
        EatEvent = new UnityEvent();
        StopEatEvent = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputController>();
        PlayerMovement = GetComponent<PlayerMovement>();
        _playerEating = GetComponent<PlayerEating>();

        EatEvent.AddListener(_playerEating.StartEating);
        EatEvent.AddListener(() => Debug.Log("Start"));
        StopEatEvent.AddListener(_playerEating.StopEating);
        StopEatEvent.AddListener(() => Debug.Log("Stop"));
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        if (_inputHandler == null) return;
        PlayerMovement?.HandleMovement(delta, _inputHandler.MovementInput);
    }
}
