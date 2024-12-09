using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BotController : Agent
{
    [SerializeField] float speed = 5;
    [SerializeField] float manageAbility = 60;
    [SerializeField] float health = 50;
    float currentHealth;
    Vector3 botRotation;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.rotation.eulerAngles);
        sensor.AddObservation(transform.position);
    }

    public override void OnEpisodeBegin()
    {
        currentHealth = health;
        botRotation = Vector3.zero;
        transform.position = Vector3.zero;
    }

    public void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        botRotation.x = actions.ContinuousActions[0];
        botRotation.z = actions.ContinuousActions[1];

        transform.Rotate(botRotation, manageAbility * Time.deltaTime, Space.Self);

        SetReward(0.001f);

        if (currentHealth <= 0)
            EndEpisode();
    }

    private void OnTriggerEnter(Collider other)
    {
        float damage = other.transform.localScale.x;
        SetReward(-0.1f * damage);
        currentHealth -= damage;
    }
}
