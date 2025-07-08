using UnityEngine;

public class Score : MonoBehaviour
{

    public Transform Player;


    // Update is called once per frame
    void Update()
    {
        Debug.Log(Player.position.z);
    }
}
