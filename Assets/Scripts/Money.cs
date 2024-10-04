using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int moneyAmount;
    // Start is called before the first frame update
    void Start()
    {
        moneyAmount = Random.Range(3,6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
