using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddlePoint : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    float distPlayers;
    Vector3 dirVec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distPlayers = Vector3.Distance(player1.position, player2.position);

        dirVec = player1.position - player2.position;
        dirVec = dirVec.normalized;
        transform.position = (dirVec * (distPlayers/2)) + player2.position;


        //transform.position.x = player1.position.x + (player2.position.x - player1.position.x) / 2;
        //transform.position.z = player1.position.z + (player2.position.z - player1.position.z) / 2;
    }
}
