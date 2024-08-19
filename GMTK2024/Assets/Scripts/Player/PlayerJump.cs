using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Player _playerManager;
    private Rigidbody2D _rb;

    private bool _jumpSquat;
    private float _jumpSquatTimer;

    [field: SerializeField] public float MaxJumpForce; // 20
    [field: SerializeField] public float MinJumpForce; // 5

    [SerializeField] private float _maxJumpTime;

    [field: SerializeField]
    public bool IsGround
    {
        get { return _isGroundTimer > 0; }
    }

    [field: SerializeField]
    private float _coyoteTime;
    private float _isGroundTimer;

    public bool HasJumped { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_jumpSquat)
        {
            _jumpSquatTimer += Time.deltaTime;
        }
    }

    public void JumpSquat()
    {
        if (!IsGround && !_jumpSquat && HasJumped) return;
        _jumpSquat = true;
    }

    public void JumpRelease()
    {
        if (_jumpSquatTimer <= 0) return;

        var jumpRatio = Mathf.Min(1f, _jumpSquatTimer / _maxJumpTime);
        var jumpForce = (MaxJumpForce - MinJumpForce) * jumpRatio + MinJumpForce;
        //Debug.Log($"Jump force {jumpForce}, jump ratio: {jumpRatio}");
        var jumpVec = _playerManager.transform.up;
        _rb.AddForce(jumpVec * jumpForce, ForceMode2D.Impulse);

        _jumpSquat = false;
        _jumpSquatTimer = 0;

        _isGroundTimer = 0;
        HasJumped = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Core")
        {
            _isGroundTimer = _coyoteTime;
            HasJumped = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Core")
        {
            StartCoroutine(StartGroundTimer());
        }
    }

    private IEnumerator StartGroundTimer()
    {
        while (_isGroundTimer > 0)
        {
            _isGroundTimer -= Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
