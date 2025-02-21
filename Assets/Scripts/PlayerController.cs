using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;  // constant speed moving to the right
    public float verticalSpeed = 5f;  // vertical speed
    public float restartDelay = 3f;
    public GameObject destroyEffect;
    public GameObject ghostPrefab;


    private static List<Vector2> recordedPositions = new List<Vector2>();

    private bool isRecording = true;
    private bool isReplaying = false;
    private static bool ghostExists = false;

    private bool hasKey = false;

    void Start()
    {
        if (recordedPositions.Count > 0 && ghostExists)  // 2nd round with the ghost
        {
            Debug.Log("Enters second round");
            //PlayerPrefs.SetInt("round", 2);  // round set

            transform.position = new Vector3(-8, 0, 0); // change the position a little to avoid collision
            StartGhostReplay();
        }
        else  // 1st round only the player, record movement
        {
            PlayerPrefs.SetInt("round", 1);  // round set
            StartCoroutine(RecordMovement());  
        }
    }

    void Update()
    {

        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += Vector3.up * verticalInput * verticalSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ability"))  // reach the powerup
        {
            Debug.Log("Ability Acquired - Triggering Ghost Replay and Restarting");
            ghostExists = true;
            isReplaying = false;
            isRecording = false;

            PlayerPrefs.SetInt("round", 2);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // scene reload
        }
        else  // collide and crashes
        {
            Debug.Log("Collision happens with player");
            recordedPositions.Clear(); // clear the previous data

            PlayerPrefs.SetInt("round", 1);
            ghostExists = false;

            Destroy(gameObject); // destroy the player
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // restart
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Key"))
        {
            hasKey = true;
            GameManager.Instance.AddKey();
            Destroy(other.gameObject); // destroy the key after collected
            Debug.Log("Key has collected");
        }
        else if (other.CompareTag("Door") && hasKey) // reach the gate
        {
            Debug.Log("Success!");
            //GameManager.Instance.LevelComplete(); // ���� GameManager ����ͨ��
            PlayerPrefs.SetInt("round", 1);
            GameManager.Instance.PlayerWins(); // ����ͨ�غ���
        }
    
    }

    IEnumerator RecordMovement()
    {
        recordedPositions.Clear();  // clear previous data
        //recordedJumps.Clear();

        while (isRecording)
        {
            recordedPositions.Add(transform.position);
            //recordedJumps.Add(Input.GetKey(KeyCode.Space));
            yield return new WaitForFixedUpdate();
        }
    }

    public void StartGhostReplay()
    {
        if (!isReplaying)
        {
            StartCoroutine(ReplayGhost());
        }
    }

    IEnumerator ReplayGhost()
    {
        isReplaying = true;
        GameObject ghost = Instantiate(ghostPrefab, recordedPositions[0], Quaternion.identity);
        ghost.transform.Rotate(0, 0, -90);
        Rigidbody2D rb = ghost.GetComponent<Rigidbody2D>();
        //rb.gravityScale = 0;

        int index = 0;

        while (index < recordedPositions.Count)
        {
            ghost.transform.position = recordedPositions[index];
            //if (recordedJumps[index])
            //{
                // Optional: Play jump animation or effect
            //}
            index++;
            yield return new WaitForFixedUpdate();
        }

        // finish replaying
        isReplaying = false;
        ghostExists = false;
    }
}