using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    public Vector3 _speed = new Vector3(0, 100, 0);
    public float _fadingTime = 0.5f;
    public float _beforeFading = 1f;

    RectTransform _textTransform;
    TextMeshProUGUI _textMeshPro;

    private float _timePassed1 = 0f;
    private float _timePassed2 = 0f;
    private Color _initColor;

    private void Awake()
    {
        _textTransform = GetComponent<RectTransform>();
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        
    }

    private void Start()
    {
        _initColor = _textMeshPro.color;
    }

    private void Update()
    {
        _textTransform.position += _speed * Time.deltaTime;

        _timePassed1 += Time.deltaTime;

        if (_timePassed1 > _beforeFading)
        {
            _timePassed2 += Time.deltaTime;
            if (_timePassed2 < _fadingTime)
            {
                float _fade = _initColor.a * (1 - (_timePassed2 / _fadingTime));
                _textMeshPro.color = new Color(_initColor.r, _initColor.g, _initColor.b, _fade);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
    }
}
