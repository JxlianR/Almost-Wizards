using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementAreaBehavior : MonoBehaviour
{
    public GameObject combinedElement1;
    public string combinedElement1Tag;

    public GameObject combinedElement2;
    public string combinedElement2Tag;

    public string cancellingElementTag; // Tag of the element that cancels this area
    public string cancellingAreaTag; // Tag of the area that cancels this area

    public string combiningElement1Tag; // Tag of the first element this element can be combined with
    public string combiningElement2Tag; // Tag of the second element this element can be combined with
    public string combiningArea1Tag; // Tag of the first area this area ca be combined with
    public string combiningArea2Tag; // Tag of the secon area this are ca be combined with

    public float duration; // Duration of the area, after this duration it gets destroyed

    public int Damage;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == combiningElement1Tag || other.tag == combiningArea1Tag) // Checks if the other gameobject is an element this element can combine with
        {
            StartCoroutine(Destroy());
            //Instantiate(combinedElement1, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation); // Spawns the first combined element
        }
        else if (other.tag == combiningElement2Tag || other.tag == combiningArea2Tag)
        {
            StartCoroutine(Destroy());
            //Instantiate(combinedElement2, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation); // Spawns the second combined element
        }
        else if (other.tag == cancellingElementTag || other.tag == cancellingAreaTag)
        {
            StartCoroutine(Destroy());
        }
    }
}
