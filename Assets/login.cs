using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Button btn;

    private void Start()
    {
        btn.onClick.AddListener(OnLoginButtonClicked);
    }

    public void OnLoginButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;
        print($"{username} {password}");

        if (username == "user" && password == "user")
        {
            SceneManager.LoadScene("chooseMeal");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
