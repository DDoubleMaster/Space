using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    GameObject player;

	[HideInInspector] public bool toMain = false;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.Find("Player");
	}

	// Update is called once per frame
	void LateUpdate()
    {
<<<<<<< Updated upstream
        transform.position = player.transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime);
=======
        if (toMain)
		{
            transform.Translate(transform.forward * Time.deltaTime * 100, Space.World);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, 2 * Time.deltaTime);
            Vector3 cameraPos = mainCamera.transform.localPosition;
            mainCamera.transform.localPosition = Vector3.Slerp(cameraPos, Vector3.zero, 5 * Time.deltaTime);

            if (transform.rotation == Quaternion.identity && !IsInvoking("LoadMainScene"))
                Invoke("LoadMainScene", 1.2f);
        }
        else
		{
            transform.position = player.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime);
            mainCamera.transform.localPosition = Vector3.Slerp(mainCamera.transform.localPosition, new Vector3(0, 2.5f, -8), Time.deltaTime);
        }
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene("Main Menu");
>>>>>>> Stashed changes
    }
}
