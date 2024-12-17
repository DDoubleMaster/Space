using UnityEngine;

public class Bullet : MonoBehaviour
{
	private void Start()
	{
		Debug.DrawLine(transform.position, transform.position + (-transform.forward * 10), Color.red, 1000);
		Invoke("Destroy", 1.5f);
	}

	void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.forward, 100 * Time.deltaTime);
    }

	private void Destroy()
	{
		Destroy(gameObject);
	}
}
