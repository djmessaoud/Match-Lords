using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var privacyAgreed = PlayerPrefs.GetInt("privacyAccepted", 0);
        if (privacyAgreed == 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else SceneManager.LoadScene("PrivacyPolicy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
