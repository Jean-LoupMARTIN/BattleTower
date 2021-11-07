using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public float speed = 5;

    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift)) transform.position += Vector3.down * speed * Time.deltaTime / 2;
        if (Input.GetKey(KeyCode.Space))     transform.position += Vector3.up   * speed * Time.deltaTime / 2;
    }
}
