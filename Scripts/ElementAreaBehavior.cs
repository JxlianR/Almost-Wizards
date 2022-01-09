using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementAreaBehavior : MonoBehaviour
{
    public GameObject combinedElement1;
    public string combinedElement1Tag;

    public GameObject combinedElement2;
    public string combinedElement2Tag;

    //public int damage;

    public float duration; // Duration of the area, after this duration it gets destroyed

    public string combiningElement1Tag; // Tag of the first element this element can be combined with
    public string combiningElement2Tag; // Tag of the second element this element can be combined with

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyElement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destroying the area after the duration
    IEnumerator DestroyElement()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Enemy" && collision.collider.tag != "Player")
        {
            if (collision.gameObject.tag == combiningElement1Tag)
            {
                if (GameObject.FindGameObjectsWithTag(combinedElement1Tag).Length == 0)
                {
                    Instantiate(combinedElement1, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
                    Destroy(gameObject);
                }
            }
            else if (collision.gameObject.tag == combiningElement2Tag)
            {
                if (GameObject.FindGameObjectsWithTag(combinedElement2Tag).Length == 0)
                {
                    Instantiate(combinedElement2, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation);
                    Destroy(gameObject);
                }
            }

            //Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == combiningElement1Tag) // Checks if the other gameobject is an element this element can combine with
        {
            StartCoroutine(Destroy());

            if (GameObject.FindGameObjectsWithTag(combinedElement1Tag).Length == 0) // Checks if there already is a gameObject with the tag of the first combined element
            {
                Instantiate(combinedElement1, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation); // Spawns the first combined element
            }
        }
        else if (other.tag == combiningElement2Tag)
        {
            StartCoroutine(Destroy());

            if (GameObject.FindGameObjectsWithTag(combinedElement2Tag).Length == 0) // Checks if there already is a gameObject with the tag of the second combined element
            {
                Instantiate(combinedElement2, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation); // Spawns the second combined element
            }
        }

        //Destroy(gameObject);
    }
}
