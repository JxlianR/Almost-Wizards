using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControllerRight : MonoBehaviour
{
    public GameObject combinedElement1;
    public string combinedElement1Tag;

    public GameObject combinedElement2;
    public string combinedElement2Tag;

    public GameObject elementArea; // Spell area of this element

    public float speed;

    public static int damage = 1;

    public string elementTag;
    public string cancellingElementTag; // Tag of the element that cancels this element
    public string cancellingAreaTag; // Tag of the area that cancels this element
    public string combiningElement1Tag; // Tag of the first element this element can be combined with
    public string combiningElement2Tag; // Tag of the second element this element can be combined with
    public string combiningArea1Tag; // Tag of the first area this elemant ca be combined with
    public string combiningArea2Tag; // Tag of the secon area this elemant ca be combined with

    //public static bool elementAlive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(PlayerController.aimPositionTwo.x, PlayerController.aimPositionOne.y, PlayerController.aimPositionTwo.z), speed * Time.deltaTime);
        transform.Translate(new Vector3(0, -0.4f, -1) * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == combiningElement1Tag || other.tag == combiningArea1Tag) // Checks if the tag of the GameObject the element is colliding with equals the string for the first element this element can be combined with
        {
            Destroy(gameObject);
            // Spawns the first combined element at the position I get from DetectGroundHeigh minus the Vector3
            Instantiate(combinedElement1, DetectGroundHeight(other.transform.position.x, other.transform.position.z) - new Vector3(0, 2.44f, 0), combinedElement1.transform.rotation);
        }
        else if (other.tag == combiningElement2Tag || other.tag == combiningArea2Tag) // Checks if the tag of the GameObject the element is colliding with equals the string for the second element this element can be combined with
        {
            Destroy(gameObject);
            // Spawns the second combined element at the position I get from DetectGroundHeigh minus the Vector3
            Instantiate(combinedElement2, DetectGroundHeight(other.transform.position.x, other.transform.position.z) - new Vector3(0, 1.56f, 0), combinedElement2.transform.rotation);
        }
        else if (other.tag == "Enemy") // Checks if the element collides with an enemy or the ground
        {
            Destroy(gameObject);
            // Spawns the element area of this element at the position I get from DetectGroundHeigh minus the Vector3
            Instantiate(elementArea, DetectGroundHeight(other.transform.position.x, other.transform.position.z) - new Vector3(0, 1.27f, 0), elementArea.transform.rotation);
        }
        else if (other.tag == "Ground")
        {
            Destroy(gameObject);
            // Spawns the element area of this element at the position I get from DetectGroundHeigh minus the Vector3
            Instantiate(elementArea, transform.position - new Vector3(0, 0.28f, 0) /*DetectGroundHeight(PlayerController.aimPositionTwo.x, PlayerController.aimPositionTwo.z) - new Vector3(0, 0.8f, 0)*/, elementArea.transform.rotation);
        }
        else if (other.tag == "Platform")
        {
            Destroy(gameObject);
            // Spawns the element area of this element at the position I get from DetectGroundHeigh minus the Vector3
            Instantiate(elementArea, transform.position - new Vector3(0, 0.22f,0) /*DetectGroundHeight(PlayerController.aimPositionTwo.x, PlayerController.aimPositionTwo.z) - new Vector3(0, 0.15f, 0)*/, elementArea.transform.rotation);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector3 DetectGroundHeight(float x, float z)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 100, z); // setting a high number to the v value
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity); // Send the raycast
        return hit.point; // returning the position of the ground
    }
}
