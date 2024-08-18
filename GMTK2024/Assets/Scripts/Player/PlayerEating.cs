using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEating : MonoBehaviour
{
    private GameObject _eatCollider;

    private Coroutine _eatRoutine;

    [SerializeField]
    private float _eatFrequencyS;

    [SerializeField]
    private Transform _eatBoxPosition;

    [SerializeField]
    private GameObject EAT_BOX_PREFAB;

    void Start()
    {
        StopEating();
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
            _eatCollider.transform.position = _eatBoxPosition.position;
            _eatCollider.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            _eatCollider.SetActive(false);
            yield return new WaitForSeconds(_eatFrequencyS);
        }
    }

    public void StopEating()
    {
        if (_eatCollider != null) Destroy(_eatCollider);
        if (_eatRoutine != null) StopCoroutine(_eatRoutine);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
