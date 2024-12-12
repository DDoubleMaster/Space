using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;
	Camera mainCamera;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.Find("Player");
        mainCamera = Camera.main;
	}

	// Update is called once per frame
	void LateUpdate()
    {
        transform.position = player.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime);
        mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, new Vector3(0, 2.5f, -8), Time.deltaTime);
    }
}
