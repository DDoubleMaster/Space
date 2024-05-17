using UnityEngine;

public class TwentyFifth : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;
    Transform player;

	private void Start()
	{
		player = GameObject.Find("Player").transform;
	}

	void Update()
    {
		if (Input.GetAxis("Attach") != 0 && !IsInvoking("LeftAttach") && !IsInvoking("RightAttach"))
			Invoke("LeftAttach", 1);
		else if (Input.GetAxis("Attach") == 0)
			CancelInvoke();
	}

    void LeftAttach()
    {
        Quaternion direction = Quaternion.LookRotation(left.transform.forward);
        GameObject thisBullet = Instantiate(bullet, left.transform.position, direction);

        Invoke("RightAttach", 0.3f);
    }

	void RightAttach()
	{
		Instantiate(bullet, right.transform.position, transform.rotation);
		Invoke("LeftAttach", 0.3f);
	}
}
