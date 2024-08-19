
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Person : MonoBehaviour
{
    [Header("Person Settings")]
    [SerializeField]
    private float _makeDecisionTime;
    [SerializeField]
    private float _speed;

    private float _decisionTimer;
    private Coroutine _changeStateRoutine;
    public Planet Planet { get; set; }

    private State _state;
    private Rigidbody2D _rb;

    [SerializeField]
    private GameObject SPLAT_PREFAB;

    // Start is called before the first frame update
    void Start()
    {
        _changeStateRoutine = StartCoroutine(ChangeStateRoutine());
        _rb = GetComponent<Rigidbody2D>();
    }

    private IEnumerator ChangeStateRoutine()
    {
        while (true)
        {
            _state = GetRandomState();
            yield return new WaitForSeconds(_makeDecisionTime);
        }
    }

    public void Freeze()
    {
        if (_rb == null) return;
        this._rb.bodyType = RigidbodyType2D.Static;
    }

    public void Unfreeze()
    {
        if (_rb == null) return;
        this._rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // Update is called once per frame
    void Update()
    {
        HandleState();
    }

    private void HandleState()
    {
        if (_state == State.Idle || (_rb != null && _rb.bodyType == RigidbodyType2D.Static))
        {
            return;
        }
        int direction = _state == State.WalkRight ? 1 : -1;
        if (Planet != null)
        {
            this.transform.up = (this.transform.position - Planet.transform.position).normalized;
        }
        // Keep original Y velocity
        Vector2 currentVelocityWorldSpace = _rb.velocity;
        Vector2 currentVelocityLocalSpace = transform.InverseTransformDirection(currentVelocityWorldSpace);

        // Take suggested X velocity
        Vector2 suggestedMovementWorldSpace = this.transform.right * _speed * direction;
        Vector2 suggestedMovementLocalSpace = transform.InverseTransformDirection(suggestedMovementWorldSpace);

        suggestedMovementLocalSpace = new Vector2(suggestedMovementLocalSpace.x, currentVelocityLocalSpace.y);
        Vector2 finalMovementWorldSpace = transform.TransformDirection(suggestedMovementLocalSpace);
        _rb.velocity = finalMovementWorldSpace;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Splatter")
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                return;
            }
            if (rb.velocity.magnitude < 2)
            {
                return;
            }
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (_changeStateRoutine != null)
        {
            StopCoroutine(_changeStateRoutine);
        }
        if (!this.gameObject.scene.isLoaded) return;
        var splat = Instantiate(SPLAT_PREFAB, transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), null);
        Destroy(splat.gameObject, 30f);
    }

    private enum State
    {
        Idle,
        WalkLeft,
        WalkRight,
    }

    private State GetRandomState()
    {
        return System.Enum.GetValues(typeof(State))
                   .OfType<State>()
                   .OrderBy(s => Random.value)
                   .First();
    }
}
