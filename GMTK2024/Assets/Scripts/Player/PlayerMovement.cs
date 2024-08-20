using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{

    // TODO: Set planetGenerator based on distance from/force on player
    [SerializeField]
    private GameObject _currPlanet;
    public GameObject CurrPlanet
    {
        get { return _currPlanet; }
    }

    private Rigidbody2D _rb;

    private Animator _animator;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject laserPrefab;

    [Header("Player Settings")]
    [SerializeField]
    private float speed;

    [Header("Animation Settings")]
    [SerializeField]
    private float animationSpeed;

    private Player _playerManager;
    private bool _canSwitchPlanet;

    private Tweener _planetRotationTween;
    private bool _isPlanetRotateTween;
    private Coroutine _playerDeath;
    private float deathTimer = 5f;
    [SerializeField]
    private float TIME_TILL_DEATH = 5F;
    
    [SerializeField] private PostProcessVolume pVolume;
    private DepthOfField depthOfFieldEffect;
    private float normalDOF = 10;
    private float minDOF = 0.1f;

    bool isDeadSoundPlayed;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        // Default playback speed is 1, which is too fast for the idle animation
        _animator.speed = animationSpeed;
        _playerManager = GetComponent<Player>();

        pVolume.profile.TryGetSettings(out depthOfFieldEffect);
        depthOfFieldEffect.focusDistance.value = normalDOF;

    }
    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void Update()
    {
        if (_currPlanet == null)
        {
            var currentFD = depthOfFieldEffect.focusDistance;
            var deathTimePercentage = deathTimer / TIME_TILL_DEATH;
            //Debug.Log(deathTimePercentage);
            var newFD = normalDOF * deathTimePercentage;
            newFD = Mathf.Max(minDOF, newFD);
            depthOfFieldEffect.focusDistance.value = newFD;

            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                Debug.Log("Death");
                if (!isDeadSoundPlayed) Jukebox.Instance.PlaySFX("Death");
                isDeadSoundPlayed = true;
                _playerManager.PlayerStat.IsDead = true;
                _playerManager.IsDead = true;
                _rb.bodyType = RigidbodyType2D.Static;
            }
        }
        else
        {
            depthOfFieldEffect.focusDistance.value = normalDOF;
            deathTimer = TIME_TILL_DEATH;
        }
    }

    public void HandleMovement(float delta, Vector2 movement)
    {
        if (_currPlanet == null)
        {
            movement = Vector2.zero;
        }

        // don't move while switching planets
        if (_isPlanetRotateTween)
        {
            return;
        }

        if (_currPlanet != null)
        {
            this.transform.up = (this.transform.position - _currPlanet.transform.position).normalized;
        }

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet" && collision.gameObject != _currPlanet && _playerManager.IsJumping && _canSwitchPlanet)
        {

            var originalUp = _currPlanet != null ? _currPlanet.transform.position - transform.position :
                                                    transform.up;

            Debug.Log($"Collied with planet {collision.name}");
            var newPlanet = collision.gameObject;

            if (_currPlanet != null)
            {
                _currPlanet.GetComponent<PointEffector2D>().enabled = false;
                _currPlanet.GetComponent<Planet>().FreezePeople();
            }
            newPlanet.GetComponent<PointEffector2D>().enabled = true;
            newPlanet.GetComponent<Planet>().UnfreezePeople();

            _currPlanet = newPlanet;
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
            Jukebox.Instance.PlaySFX("Swap");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Planet" || collision.gameObject != _currPlanet)
        {
            return;
        }

        _currPlanet.GetComponent<Planet>().FreezePeople();
        _currPlanet = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _canSwitchPlanet = true;
            collision.gameObject.GetComponentInParent<Planet>().PlayerOnPlanetEvent.Invoke();
            _currPlanet?.GetComponent<Planet>()?.UnfreezePeople();
        }
    }
}