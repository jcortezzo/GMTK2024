using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Planet : MonoBehaviour
{
    public bool AllowNeighborPlanetGeneration { get; set; }
    public UnityEvent PlayerOnPlanetEvent;

    private CircleCollider2D _cc2d;
    private PlanetGenerator _planetGenerator;
    private ParticleSystem _particleSystem;
    private ParticleSystem _ringParticleSystem;
    private Color _planetColor;

    [SerializeField]
    private GameObject MINIMAP_ICON_PREFAB;
    [SerializeField]
    private GameObject MINIMAP_RING_PREFAB;

    private static Color[] _colors = new Color[]
    {
        HexToColor("362023"),
        HexToColor("700548"),
        HexToColor("7272ab"),
        HexToColor("7899d4")
    };

    // Start is called before the first frame update
    void Start()
    {
        _cc2d = GetComponent<CircleCollider2D>();
        _planetGenerator = GetComponent<PlanetGenerator>();
        _particleSystem = GetComponent<ParticleSystem>();
        Transform child = transform.GetChild(0);
        _ringParticleSystem = child.GetComponent<ParticleSystem>();

        _cc2d.radius = _planetGenerator.Radius * 2;
        PlayerOnPlanetEvent = new UnityEvent();
        PlayerOnPlanetEvent.AddListener(GenerateNeighborPlanets);

        InitParticleSystem();
        _particleSystem.Play();
        _ringParticleSystem.Play();

        SpawnMinimapCircle();
    }

    public void FreezePeople()
    {
        _planetGenerator.People?.ToList().ForEach(p => p?.Freeze());
    }

    public void UnfreezePeople()
    {
        _planetGenerator.People?.ToList().ForEach(p => p?.Unfreeze());
    }

    private void GenerateNeighborPlanets()
    {
        if (!AllowNeighborPlanetGeneration) return;
        // code for generate neighbor planets
        // generate 3 planets
        PlanetProcedureGeneration.Instance.GeneratePlanet(this);
        AllowNeighborPlanetGeneration = false;
    }

    private void InitParticleSystem()
    {
        var shape = _particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = _cc2d.radius;
        var main = _particleSystem.main;
        _planetColor = _colors.OrderBy(c => Random.value).First();
        main.startColor = _planetColor;

        var outerShape = _ringParticleSystem.shape;
        outerShape.shapeType = ParticleSystemShapeType.Circle;
        outerShape.radius = _cc2d.radius;
        var outerMain = _ringParticleSystem.main;
        outerMain.startColor = Color.white;
    }

    void SpawnMinimapCircle()
    {
        GameObject minimapCircle = Instantiate(MINIMAP_ICON_PREFAB, transform.position, Quaternion.identity, null);
        minimapCircle.transform.localScale = Vector3.one * _planetGenerator.Radius * 2f;
        SpriteRenderer minimapRenderer = minimapCircle.GetComponent<SpriteRenderer>();
        if (minimapRenderer != null)
        {
            minimapRenderer.color = _planetColor;
        }

        GameObject minimapRing = Instantiate(MINIMAP_RING_PREFAB, transform.position, Quaternion.identity, null);
        minimapRing.transform.localScale = Vector3.one * _cc2d.radius * 2f;
        minimapRenderer = minimapRing.GetComponent<SpriteRenderer>();
        if (minimapRenderer != null)
        {
            minimapRenderer.color = _planetColor;
        }
    }

    static Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
        {
            return color;
        }
        Debug.LogError("Invalid hex color: " + hex);
        return Color.white;
    }
}