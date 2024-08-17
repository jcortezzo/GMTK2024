using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private CircleCollider2D cc2d;
    private PlanetGenerator planetGenerator;

    // Start is called before the first frame update
    void Start()
    {
        cc2d = GetComponent<CircleCollider2D>();
        planetGenerator = GetComponent<PlanetGenerator>();
        cc2d.radius = planetGenerator.Radius * 2;
    }
}