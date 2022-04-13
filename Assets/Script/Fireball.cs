using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public int direccion;
    public float speed;   
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }
    
    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition += new Vector3(direccion * speed * Time.deltaTime, 0, 0);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
