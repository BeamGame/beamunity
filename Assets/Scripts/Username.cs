using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Username : MonoBehaviour
{


    protected VisualElement root;
    protected TextField txtName;
    protected Label lblError;
    protected Button btnCreate;

    void Start()
    {


        root = GetComponent<UIDocument>().rootVisualElement;
        btnCreate = root.Q<Button>("btnCreate");
        txtName = root.Q<TextField>("txtName");
        
        lblError = root.Q<Label>("lblError");
        btnCreate.clicked += BtnCreate_clicked;
        //btnCreate.visible = false;
        //txtName.visible = false;

        lblError.text = string.Empty;

        
    }

    private void BtnBack_clicked()
    {
        SceneManager.LoadScene("Login");
    }

    private async void BtnCreate_clicked()
    {

        try
        {
            PlayerName reg = new PlayerName() {  Name = txtName.text };
            var player = await ConnectionService.UpdateUsername(reg);
            lblError.text = "Login created";
            Debug.Log($"Player {player.Name}");
            GameContext.Instance.Name = player.Name;
            SceneManager.LoadScene("Login");
        }
        catch (System.Exception ex)
        {
            lblError.text = ex.Message;
        }
    }



    void Update()
    {


    }
}
