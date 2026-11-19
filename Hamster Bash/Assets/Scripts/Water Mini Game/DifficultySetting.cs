using UnityEngine;
using UnityEngine.UI;

public class DifficultySetting : MonoBehaviour
{
    private Button button;
    private SpawnManager spawnManager;
    public int difficulty;
    void Start()
    {
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetDifficulty()
    {
        Debug.Log(button.gameObject.name + " was clicked");
        spawnManager.StartGame(difficulty);
       
    }
}
