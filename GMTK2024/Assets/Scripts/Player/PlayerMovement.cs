using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // TODO: Set planetGenerator based on distance from/force on player
    [SerializeField]
    private GameObject _planet;

    private Rigidbody2D _rb;

    private Animator _animator;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject laserPrefab;

    [Header("Player Settings")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float laserDuration;
    [SerializeField]
    private float laserLength;

    [Header("Animation Settings")]
    [SerializeField]
    private float animationSpeed;

    private Player _playerManager;
    private bool _canSwitchPlanet;
    // public Vector2 CoreToPlayer
    // {
    //     get
    //     {
    //         var planet = planetController.GetClosestPlanetToPlayer();
    //         return (this.transform.position - planet.transform.position).normalized;
    //     }
    // }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        // Default playback speed is 1, which is too fast for the idle animation
        _animator.speed = animationSpeed;
        _playerManager = GetComponent<Player>();
    }

    public void HandleMovement(float delta, Vector2 movement)
    {
        this.transform.up = (this.transform.position - _planet.transform.position).normalized;
        // this.transform.up = (planetGenerator.transform.position * this.transform.up.magnitude).normalized;

        // Keep original Y velocity
        Vector2 currentVelocityWorldSpace = _rb.velocity;
        Vector2 currentVelocityLocalSpace = transform.InverseTransformDirection(currentVelocityWorldSpace);

        // Take suggested X velocity
        Vector2 suggestedMovementWorldSpace = this.transform.right * speed * movement.x;
        Vector2 suggestedMovementLocalSpace = transform.InverseTransformDirection(suggestedMovementWorldSpace);

        suggestedMovementLocalSpace = new Vector2(suggestedMovementLocalSpace.x, currentVelocityLocalSpace.y);
        Vector2 finalMovementWorldSpace = transform.TransformDirection(suggestedMovementLocalSpace);
        _rb.velocity = finalMovementWorldSpace;
    }

    // private void Jump()
    // {
    //     _rb.AddForce(CoreToPlayer * jumpForce, ForceMode2D.Impulse);
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Planet" && collision.gameObject != _planet && _playerManager.IsJumping && _canSwitchPlanet)
        {
            Debug.Log($"Collied with planet {collision.name}");
            var newPlanet = collision.gameObject;
            _planet = newPlanet;
            _canSwitchPlanet = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _canSwitchPlanet = true;
        }   
    }

}