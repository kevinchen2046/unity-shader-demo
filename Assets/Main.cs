using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private Vector3 rota;
    public GameObject target;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        rota = new Vector3(0.0f, 0.1f, 0.0f);
        speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = FindObjectOfType<Camera>();

        //camera.transform.LookAt(target.transform, rota);
        //camera.transform.Rotate(rota);
        camera.transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * speed);
    }
}
