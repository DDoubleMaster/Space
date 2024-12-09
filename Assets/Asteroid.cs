using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int chance;
    [HideInInspector] public TaskManager taskManager;

    // Asteroid Parameters
    Vector3 direction;
    Vector3 resultScale;
    // Components
    Rigidbody _rBody;
    Renderer _render;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        _render = transform.GetChild(0).GetComponent<Renderer>();
        _rBody = GetComponent<Rigidbody>();

        // Set Parameters
        transform.localScale = Vector3.zero;
        resultScale = Vector3.one * Random.Range(1f, 5f);
        direction = Random.insideUnitSphere.normalized;
        _rBody.mass = Mathf.Pow(resultScale.x, resultScale.x) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        //  Asteroid Movement
        transform.localScale = Vector3.Lerp(transform.localScale, resultScale, 2f * Time.deltaTime);
        transform.Translate(direction * Time.deltaTime * (5 / transform.localScale.x));
        transform.Rotate(direction * (5 / transform.localScale.x));

        // Destroy Asteroid if he is on camera
        if (!_render.isVisible)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Change direction after collision
        direction = Vector3.Reflect(direction, collision.contacts[0].point.normalized);
        _rBody.AddForce(direction * (5 - resultScale.x + 1) * 10, ForceMode.Impulse);
    }

	private void OnTriggerEnter(Collider other)
	{
        taskManager.AddMoney(((100 - chance) / 4) * (int)transform.localScale.x);
		Destroy(gameObject);
	}
}
