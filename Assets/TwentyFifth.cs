using UnityEngine;

public class TwentyFifth : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;
	[SerializeField] GameObject leftWing;
	[SerializeField] GameObject rightWing;
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

		float horizontal = Input.GetAxis("Horizontal") * 20;
		float vertical = Input.GetAxis("Vertical") * 20;
		leftWing.transform.localRotation = Quaternion.Euler(horizontal + vertical, 0, 0);
        rightWing.transform.localRotation = Quaternion.Euler(-horizontal + vertical, 0, 0);
    }

    void LeftAttach()
    {
        Quaternion direction = Quaternion.LookRotation(left.transform.forward);
        GameObject thisBullet = Instantiate(bullet, left.transform.position, direction);

        Invoke("RightAttach", 0.1f);
    }

	void RightAttach()
	{
		Instantiate(bullet, right.transform.position, transform.rotation);
		Invoke("LeftAttach", 0.1f);
	}
}
