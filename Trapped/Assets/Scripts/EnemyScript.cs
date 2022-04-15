using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform Player;
    public int moveSpeed = 4;
    public int killDist = 2;
    bool isChasing = true;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        isChasing = true;
    }

    // Update is called once per frame
    void Update()
    {
 
         if (Vector3.Distance(transform.position, Player.position) >= killDist && isChasing == true)
         {

            navMeshAgent.destination = Player.position;
 
 
 
             if (Vector3.Distance(transform.position, Player.position) <= killDist)
             {
                isChasing = false;
             }
             else
            {
                isChasing = true;
            }
 
         }
    }
}
