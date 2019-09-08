using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creature : MonoBehaviour
{
    // Public variables

    public float hungerLevel;
    public float thirstLevel;
    public float boredomLevel;


    // Private variabels

    private const float LEVEL_MAX = 100f;

    private NavMeshAgent m_agent;

    private Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
