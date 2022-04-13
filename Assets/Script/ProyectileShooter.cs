using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileShooter : MonoBehaviour
{
    public Vector3 rotation;
    Quaternion quat;
    public GameObject Proyectile;
    public float CD,timer;
    private void Start()
    {
        timer = CD;
    }
    void Update()
    {
        if (timer <= CD)
            timer += Time.deltaTime;
        quat = Quaternion.Euler(rotation);
        Detectar();
    }
    void Detectar ()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out RaycastHit hit, 10000000))
        {
            if (hit.transform.CompareTag("Player"))
            {
                
                if (timer >= CD)
                { 
                    Instantiate(Proyectile, transform.position, quat, transform);
                    timer = 0;
                }
            }
        }
    }
}
