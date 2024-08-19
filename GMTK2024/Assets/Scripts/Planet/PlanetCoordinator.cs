using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCoordinator : MonoBehaviour
{
    [field:SerializeField] public List<Planet> PlanetList { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i < PlanetList.Count; i++)
        {
            var planet = PlanetList[i];
            var effector = planet.GetComponent<Effector2D>();
            if (effector != null)
            {
                effector.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
