using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    private Tweener _planetRotationTween;
    private bool _isPlanetRotateTween;

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
        // don't move while switching planets
        if (_isPlanetRotateTween)
        {
            Debug.Log("is rotating");
            return;
        }

        this.transform.up = (this.transform.position - _planet.transform.position).normalized;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" && collision.gameObject != _planet && _playerManager.IsJumping && _canSwitchPlanet)
        {
            var originalUp = _planet.transform.position - transform.position;

            Debug.Log($"Collied with planet {collision.name}");
            var newPlanet = collision.gameObject;

            _planet.GetComponent<PointEffector2D>().enabled = false;
            newPlanet.GetComponent<PointEffector2D>().enabled = true;

            _planet = newPlanet;
            _canSwitchPlanet = false;

            Vector3 newUp = newPlanet.transform.position - transform.position;
            float angle = Vector2.SignedAngle(originalUp, newUp);

            _planetRotationTween = transform.DOLocalRotate(new Vector3(0, 0, transform.localEulerAngles.z + angle), 1f).SetEase(Ease.InOutFlash);
            _planetRotationTween.OnComplete(() =>
                {
                    _planetRotationTween.Kill();
                    _isPlanetRotateTween = false;
                }
            );
            _isPlanetRotateTween = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _canSwitchPlanet = true;
        }
    }
}