using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    // Public variables

    public List<SM_State> states;


    // Private variables

    private Creature m_creature;

    // Start is called before the first frame update
    void Start()
    {
        m_creature = GetComponent<Creature>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
