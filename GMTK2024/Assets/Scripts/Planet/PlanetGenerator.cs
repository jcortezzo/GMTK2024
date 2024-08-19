using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject GROUND_TILE_PREFAB;
    [SerializeField]
    private GameObject CORE_TILE_PREFAB;
    [SerializeField]
    private GameObject DECORATION_PREFAB;
    [SerializeField]
    private GameObject PERSON_PREFAB;

    [Header("Planet Settings")]
    [SerializeField]
    private int NUM_SPOKES;
    [SerializeField]
    private int NUM_LAYERS;
    [SerializeField]
    private float DECORATION_DENSITY;
    [SerializeField]
    private float POPULATION_DENSITY;

    [field: SerializeField]
    public float Radius { get; private set; }
    [field: SerializeField]
    public float CoreRatio { get; private set; }

    private IDictionary<int, ISet<GameObject>> _layers;

    public ISet<Person> People { get; private set; }

    void Awake()
    {
        _layers = new Dictionary<int, ISet<GameObject>>();
        for (int i = 0; i < NUM_LAYERS; i++)
        {
            _layers[i] = new HashSet<GameObject>();
        }
        People = new HashSet<Person>();
    }

    private int CORE_LAYERS
    {
        get
        {
            return (int)Mathf.Floor(NUM_LAYERS * CoreRatio);
        }
    }

    private Stack<GameObject>[] stackList;
    private float DEGREES_IN_PLANET = 360f;

    [field: SerializeField]
    public bool GeneratePlanetOnStart { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        if (GeneratePlanetOnStart) GeneratePlanet(Radius, NUM_SPOKES, NUM_LAYERS);
    }

    public void GeneratePlanet(float radius, int numSpokes, int numLayers)
    {
        stackList = new Stack<GameObject>[numSpokes];
        for (int i = 0; i < stackList.Length; i++)
        {
            stackList[i] = new Stack<GameObject>();
        }
        for (int layer = 0; layer < numLayers; layer++)
        {
            bool isCoreLayer = layer < CORE_LAYERS;

            float currRadius = (radius / numLayers) * layer;
            for (int spoke = 0; spoke < numSpokes; spoke++)
            {
                float theta = (DEGREES_IN_PLANET / numSpokes) * spoke;
                theta *= Mathf.Deg2Rad;
                float x = Mathf.Cos(theta) * currRadius;
                float y = Mathf.Sin(theta) * currRadius;

                Vector2 worldPos = new Vector2(x, y);
                Vector2 position = worldPos + (Vector2)transform.position;
                Vector3 upDirection = (position - new Vector2(transform.position.x, transform.position.y)).normalized; // Assuming transform.position is the center of the planet
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, upDirection);
                GameObject groundTile = Instantiate(!isCoreLayer ? GROUND_TILE_PREFAB : CORE_TILE_PREFAB, position, rotation, this.transform);
                stackList[spoke].Push(groundTile);
                _layers[layer].Add(groundTile);
            }
        }

        // decorate / population
        var setLast = false;
        foreach (GameObject ground in _layers[NUM_LAYERS - 1])
        {
            setLast = Decorate(ground, setLast);
            AddPerson(ground);
        }
    }

    private bool Decorate(GameObject ground, bool setLast)
    {
        if (Random.value > DECORATION_DENSITY || setLast)
        {
            return false;
        }
        // rotation
        Vector3 upDirection = (ground.transform.position - transform.position).normalized; // Assuming transform.position is the center of the planet
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, upDirection);
        // position
        SpriteRenderer groundRenderer = ground.GetComponent<SpriteRenderer>();
        SpriteRenderer decorationRenderer = DECORATION_PREFAB.GetComponent<SpriteRenderer>();
        float buffer = 2f / groundRenderer.sprite.pixelsPerUnit;
        Vector3 groundTopPosition = ground.transform.position + ground.transform.up * (groundRenderer.bounds.extents.y + decorationRenderer.bounds.extents.y - buffer);
        var decoration = Instantiate(DECORATION_PREFAB, groundTopPosition, rotation, null);
        decoration.transform.parent = ground.transform;
        return true;
    }

    private void AddPerson(GameObject ground)
    {
        if (Random.value > POPULATION_DENSITY)
        {
            return;
        }
        // rotation
        Vector3 upDirection = (ground.transform.position - transform.position).normalized; // Assuming transform.position is the center of the planet
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, upDirection);
        // position
        SpriteRenderer groundRenderer = ground.GetComponent<SpriteRenderer>();
        SpriteRenderer decorationRenderer = DECORATION_PREFAB.GetComponent<SpriteRenderer>();
        float buffer = 2f / groundRenderer.sprite.pixelsPerUnit;
        Vector3 groundTopPosition = ground.transform.position + ground.transform.up * (groundRenderer.bounds.extents.y + decorationRenderer.bounds.extents.y - buffer);
        Person person = Instantiate(PERSON_PREFAB, groundTopPosition, rotation, null).GetComponent<Person>();
        person.Planet = GetComponent<Planet>();
        People.Add(person);
    }

    // Update is called once per frame
    void Update()
    {

    }
    // y = sqrt(r^2 - x^2)
    // y = sin(theta)*r
    // x = cos(theta)*r
}