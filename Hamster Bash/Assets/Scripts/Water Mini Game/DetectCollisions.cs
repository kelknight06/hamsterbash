using Unity.VisualScripting;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    public GameObject leftImage;
    public GameObject rightImage;
    public GameObject middleImage;
   
    private SpawnManager spawnManager;
    public float timeRate = 1f;
    public float speedIncrease = 10.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        //Destroy(gameObject);
        
        if (other.gameObject.CompareTag("left object"))
        {
            leftImage.SetActive(true);
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag("right object"))
        {
            rightImage.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("middle object"))
        {
            middleImage.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("bad"))
        {
            Destroy(other.gameObject);
            if (spawnManager != null)
            {
                spawnManager.IncreaseTimeRate(speedIncrease);
            }
            //timeRate += speedIncrease;
            //spawnManager.TimeLeft(timeRate);
        }
    }
 
}
