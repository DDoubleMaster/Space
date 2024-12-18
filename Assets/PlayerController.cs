using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // SpaceShip Parameter
    [SerializeField] float speed;
    [SerializeField] public float manageAbility;
    [SerializeField] int shieldStrenght;
    [SerializeField] float shieldRegeniration;

    TaskManager taskManager;

    // Current Shield
    float currentShield;

    // Components
    Rigidbody _rBody;

    // UI
    ProgressBar UI_shieldStrength;

    private void Start()
    {
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();

        //
        VisualElement root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;
        UI_shieldStrength = root.Q<ProgressBar>("ShieldStrength");

        //Get Components
        _rBody = GetComponent<Rigidbody>();

        // Set current Shield to max Shield strenght
        currentShield = shieldStrenght;
	}

    // Update is called once per frame
    private void Update()
    {
        #region UI
        int percent = (int)(currentShield / shieldStrenght * 100);
		UI_shieldStrength.title = percent + "%";
        UI_shieldStrength.value = percent;
		#endregion

		#region Movement
		// Movement to Forward
		transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        transform.Rotate(new Vector3(-Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")), manageAbility * Time.deltaTime);
        // return velocity to normal 
        _rBody.linearVelocity = Vector3.MoveTowards(_rBody.linearVelocity, Vector3.zero, Time.deltaTime / 10);
        _rBody.angularVelocity = Vector3.MoveTowards(_rBody.angularVelocity, Vector3.zero, Time.deltaTime / 10);
        #endregion

        // Shield Regeniration
        currentShield = Mathf.MoveTowards(currentShield, shieldStrenght, shieldRegeniration * Time.deltaTime);

        if (currentShield <= 0)
            taskManager.ExitToMain();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // damage to Shield
        currentShield -= collision.transform.localScale.x * 2;
    }
}
