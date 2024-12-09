using UnityEngine;

public class CameraMove : MonoBehaviour
{
    GameObject player;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void LateUpdate()
    {
        transform.position = player.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime);
    }
}
