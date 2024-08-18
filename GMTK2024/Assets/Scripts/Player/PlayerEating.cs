using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEating : MonoBehaviour
{
    private GameObject _eatCollider;

    private Coroutine _eatRoutine;

    private Player _playerManager;

    [SerializeField]
    private float _eatFrequencyS;

    [SerializeField]
    private Transform _eatBoxPosition;

    [SerializeField]
    private GameObject EAT_BOX_PREFAB;

    void Start()
    {
        _playerManager = GetComponent<Player>();
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
        while (true)
        {
            var playerAte = false;
            if (_playerManager.IsEating)
            {
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


}
