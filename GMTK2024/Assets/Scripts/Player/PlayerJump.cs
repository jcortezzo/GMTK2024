using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Player _playerManager;
    private Rigidbody2D _playerRB;

    private bool _jumpSquat;
    private float _jumpSquatTimer;

    [SerializeField]
    private float _maxJumpForce;
    [SerializeField]
    private float _minJumpForce;
    [SerializeField]
    private float _maxJumpTime;

    [field:SerializeField]
    public bool IsGround { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _playerManager = GetComponent<Player>();
        _playerRB = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(_jumpSquat)
        {
            _jumpSquatTimer += Time.deltaTime;
        }
    }

    public void JumpSquat()
    {
        if (!IsGround) return;
        _jumpSquat = true;
    }

    public void JumpRelease()
    {
        if(_jumpSquatTimer <= 0) return;

        var jumpRatio = Mathf.Min(1f, _jumpSquatTimer / _maxJumpTime);
        var jumpForce = (_maxJumpForce - _minJumpForce) * jumpRatio + _minJumpForce;
        Debug.Log($"Jump force {jumpForce}, jump ratio: {jumpRatio}");
        var jumpVec = _playerManager.transform.up;
        _playerRB.AddForce(jumpVec * jumpForce, ForceMode2D.Impulse);

        _jumpSquat = false;
        _jumpSquatTimer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            IsGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGround = false;
        }
    }
}
