using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeOpen : MonoBehaviour
{
    [SerializeField] private bool isIn;

    private void Awake()
    {
        isIn = false;
    }

    public bool GetIsIn()
    {
        return isIn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isIn = false;
        }
    }
}
