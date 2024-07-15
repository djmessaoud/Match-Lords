using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PolicySceneManager : MonoBehaviour
{
    public GameObject policyPanel;
    public GameObject acceptPopup;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   public void policyAccepted()
    {
        PlayerPrefs.SetInt("policyAccepted", 1);
        SceneManager.LoadScene("MainMenu");
    }
    public void viewPolicy()
    {
        acceptPopup.SetActive(false);
        policyPanel.SetActive(true);
    }
    public void unviewPolicy()
    {
        policyPanel.SetActive(false);
        acceptPopup.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
