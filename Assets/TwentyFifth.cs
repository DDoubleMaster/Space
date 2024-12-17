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
		if (Input.GetKey(KeyCode.Space) && !IsInvoking("LeftAttach") && !IsInvoking("RightAttach"))
			Invoke("LeftAttach", 1);
		else if (!Input.GetKey(KeyCode.Space)) CancelInvoke();

        float horizontal = Input.GetAxis("Horizontal") * 20;
		float vertical = Input.GetAxis("Vertical") * 20;

		Quaternion leftWingFinalRotate = Quaternion.Euler(horizontal + vertical, 0, 0);
		Quaternion rightWingFinalRotate = Quaternion.Euler(-horizontal + vertical, 0, 0);

		Quaternion leftWingCurrentRotate = leftWing.transform.localRotation;
		Quaternion rightWingCurrentRotate = rightWing.transform.localRotation;

		leftWing.transform.localRotation = Quaternion.Lerp(leftWingCurrentRotate, leftWingFinalRotate, Time.deltaTime * 5);
		rightWing.transform.localRotation = Quaternion.Lerp(rightWingCurrentRotate, rightWingFinalRotate, Time.deltaTime * 5);
    }

    void LeftAttach()
    {
        Quaternion direction = Quaternion.LookRotation(left.transform.forward);
        GameObject thisBullet = Instantiate(bullet, left.transform.position, direction);

        Invoke("RightAttach", 0.2f);
    }

	void RightAttach()
	{
		Instantiate(bullet, right.transform.position, transform.rotation);
		Invoke("LeftAttach", 0.2f);
	}
}
