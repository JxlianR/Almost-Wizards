using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementControllerLeft : MonoBehaviour
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
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(PlayerController.aimPositionOne.x, PlayerController.aimPositionOne.y, PlayerController.aimPositionOne.z), speed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0.3f, 1) * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == combiningElement1Tag || other.tag == combiningArea1Tag) // Checks if the tag of the GameObject the element is colliding with equals the string for the first element this element can be combined with
        {
            Destroy(gameObject);

            if (GameObject.FindGameObjectsWithTag(combinedElement1Tag).Length == 0) // Checks if there already is a gameObject with the tag of the first combined element
            {
                Instantiate(combinedElement1, DetectGroundHeight(other.transform.position.x, other.transform.position.z), combinedElement1.transform.rotation); // Spawns the first combined element
            }
        }
        else if (other.tag == combiningElement2Tag || other.tag == combiningArea2Tag) // Checks if the tag of the GameObject the element is colliding with equals the string for the second element this element can be combined with
        {
            Destroy(gameObject);

            if (GameObject.FindGameObjectsWithTag(combinedElement2Tag).Length == 0) // Checks if there already is a gameObject with the tag of the second combined element
            {
                Instantiate(combinedElement2, DetectGroundHeight(other.transform.position.x, other.transform.position.z), combinedElement2.transform.rotation); // Spawns the second combined element
            }
        }
        else if (other.tag == "Enemy") // Checks if the element collides with an enemy or the ground
        {
            Destroy(gameObject);

            if (GameObject.FindGameObjectsWithTag(elementTag).Length <= 1)
            {
                Instantiate(elementArea, DetectGroundHeight(other.transform.position.x, other.transform.position.z) - new Vector3(0, 2.15f, 0), elementArea.transform.rotation); // Spawns the element area of this element
            }
        }
        else if (other.tag == "Ground")
        {
            Destroy(gameObject);

            if (GameObject.FindGameObjectsWithTag(elementTag).Length <= 1)
            {
                Instantiate(elementArea, DetectGroundHeight(PlayerController.aimPositionOne.x, PlayerController.aimPositionOne.z) - new Vector3(0, 0.08f, 0), elementArea.transform.rotation); // Spawns the element area of this element
            }
        }
        else if (other.tag == "Platform")
        {
            Destroy(gameObject);

            if (GameObject.FindGameObjectsWithTag(elementTag).Length <= 1)
            {
                Instantiate(elementArea, DetectGroundHeight(PlayerController.aimPositionOne.x, PlayerController.aimPositionOne.z) - new Vector3(0, 0.15f, 0), elementArea.transform.rotation); // Spawns the element area of this element
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector3 DetectGroundHeight(float x, float y)
    {
        RaycastHit hit;
        Vector3 origin = new Vector3(x, 100, y);
        Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity);
        Debug.Log("Terrain location found at" + hit.point);
        return hit.point;
    }
}