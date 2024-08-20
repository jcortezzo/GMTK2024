using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;

public class PlayerEating : MonoBehaviour
{
    private GameObject _eatCollider;

    private Coroutine _eatRoutine;

    private Player _playerManager;
    private TextSpawner textBubble;

    [SerializeField]
    private float _eatFrequencyS;

    [SerializeField]
    private Transform _eatBoxPosition;

    [SerializeField]
    private GameObject EAT_BOX_PREFAB;

    [SerializeField]
    private float BONK_THRESHOLD;

    void Start()
    {
        _playerManager = GetComponent<Player>();
        textBubble = GetComponent<TextSpawner>();
        _eatRoutine = StartCoroutine(EatRoutine());

    }

    public void StartEating()
    {
        StopEating();
        _eatRoutine = StartCoroutine(EatRoutine());
    }

    private IEnumerator EatRoutine()
    {
        _eatCollider = Instantiate(EAT_BOX_PREFAB, _eatBoxPosition.position, Quaternion.identity);
        var eatBox = _eatCollider.GetComponent<EatBox>();
        eatBox.EatGroundEvent.AddListener(_playerManager.PlayerStat.EatGround);

        //_eatCollider.GetComponent<EatBox>()?.EatGroundEvent.AddListener(_playerManager.PlayerStat.EatGround);
        while (true)
        {
            var playerAte = false;
            if (_playerManager.IsEating)
            {
                textBubble.SpawnRandomText();
                _eatCollider.SetActive(true);
                _eatCollider.transform.position = _eatBoxPosition.position;
                _eatCollider.transform.localScale = _playerManager.transform.localScale;
                playerAte = true;
                yield return new WaitForSeconds(0.1f);
            }
            _eatCollider.SetActive(false);
            if (playerAte) yield return new WaitForSeconds(_eatFrequencyS);
            yield return null;
        }
    }

    public void StopEating()
    {
        if (_eatCollider != null) Destroy(_eatCollider);
        if (_eatRoutine != null) StopCoroutine(_eatRoutine);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Core") return;

        if (collision.gameObject.tag == "Ground")
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                var dot = Vector2.Dot(point.normal, this.transform.up);
                if (dot <= BONK_THRESHOLD)
                {
                    _playerManager.PlayerStat.EatGround();
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
