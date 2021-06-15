using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReboot : MonoBehaviour
{
    KeyCode restartKey = KeyCode.R;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(restartKey))
        {
            RebootScene();
        }
    }

    static public void RebootScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
