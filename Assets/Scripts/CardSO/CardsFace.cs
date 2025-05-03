using Unity.VisualScripting;
using UnityEngine;

public class CardsFace : MonoBehaviour
{
    public GameObject _target;
    public GameManager _gameManager;

    public float _rotationSpeed;
    public float _rotationAmount;

    Vector3 _rotation;
    Vector3 _movement;

    private float _randomRot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _randomRot = Random.Range(-_rotationAmount, _rotationAmount);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(transform.parent.gameObject);
            
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, _target.transform.position, Time.deltaTime * 25);

            if (!_target.GetComponent<Card>()._played)
            {
                Vector3 _pos = (transform.position - _target.transform.position);
                Vector3 _movementRotation;

                _movement = Vector3.Lerp(_movement, _pos, 25 * Time.deltaTime);

                _movementRotation = _movement;

                _rotation = Vector3.Lerp(_rotation, _movementRotation, _rotationSpeed * Time.deltaTime);

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(_movementRotation.x, -_rotationAmount, _rotationAmount));
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, _randomRot);
            }
        }       
    }
}
