using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> generators;

    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transform GetClosestPlanetToPlayer()
    {
        float distance = float.MaxValue;
        GameObject closestGo = null;
        foreach (GameObject go in generators)
        {
            var del = Vector2.Distance(go.transform.position, player.transform.position);
            if (del < distance)
            {
                closestGo = go;
                distance = del;
            }
        }

        return closestGo.transform;
    }
}