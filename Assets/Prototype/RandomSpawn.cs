using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.position += new Vector3(
            Random.Range(-5f, 5f),
            0,
            Random.Range(-5f, 5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
