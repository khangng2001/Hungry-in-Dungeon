using Unity.Burst.CompilerServices;
using UnityEngine;

public class ChangeToNewScene : MonoBehaviour
{
    [Header("Scene's Name To Go")]
    [SerializeField] private string goToSceneName;

    private bool check;

    private void Update()
    {
        if (check)
        {
            //Player reaches the door
            GameManager.instance.ChangeScene(goToSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            check = true;
        }
        else
        {
            check = false;
        }
    }
}
