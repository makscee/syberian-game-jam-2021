using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class RouteObject : MonoBehaviour
{
    public GameObject button;
    public FoodStorage from, to;
    public TextMeshProUGUI text;
    public Route route;
    [SerializeField] GameObject line;

    void Start()
    {
        route.onWorkersAmountChange += UpdateText;
        SetColors();
        UpdateText();
    }

    void Update()
    {
        var fromPos = from.transform.position;
        var toPos = to.transform.position;
        var vec = toPos - fromPos;
        transform.position = fromPos + vec / 2;
        
        var angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg - 90;
        line.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        line.transform.localScale = new Vector3(.1f, vec.magnitude - 1, 1);
    }

    SpriteRenderer _lineSr, _btnSr;
    void SetColors()
    {
        text.color = GlobalConfig.Instance.paletteBg;
        if (_lineSr == null || _btnSr == null)
        {
            _lineSr = line.GetComponent<SpriteRenderer>();
            _btnSr = button.GetComponent<SpriteRenderer>();
        }

        var color = GlobalConfig.Instance.GetColorByType(from.type);
        _lineSr.color = color.ChangeAlpha(_lineSr.color.a);
        _btnSr.color = color;
    }

    void UpdateText()
    {
        text.text = route.WorkersAmount.ToString();
    }

    void Destroy(Route r)
    {
        Destroy(gameObject);
    }

    public static void Create(Route r)
    {
        var routeObject = Instantiate(Prefabs.Instance.routeObject).GetComponent<RouteObject>();
        routeObject.route = r;
        routeObject.from = r.storagesOrder[r.storagesOrder.Length - 2];
        routeObject.to = r.storagesOrder[r.storagesOrder.Length - 1];
        r.onDestroy += routeObject.Destroy;
    }
}