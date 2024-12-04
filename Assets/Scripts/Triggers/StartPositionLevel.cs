using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartPositionLevel : MonoBehaviour
{
   


    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = Vector3.zero;   

        }
    }


}
