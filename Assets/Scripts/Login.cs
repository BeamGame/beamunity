using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class Login : MonoBehaviour
{
    //public RawImage image;

    // Texture for encoding 
    private Texture2D encoded;
    private string LastResult;
    private bool shouldEncodeNow;

    protected VisualElement root;
    protected Button btnConnect;
    protected Button btnRegister;
    protected TextField txtEmail;
    protected TextField txtPassword;



    void Start()
    {
        encoded = new Texture2D(512, 512);

        root = GetComponent<UIDocument>().rootVisualElement;
        btnConnect = root.Q<Button>("btnConnect");
        btnRegister = root.Q<Button>("btnRegister");
        txtEmail = root.Q<TextField>("txtEmail");
        txtPassword = root.Q<TextField>("txtPassword");

        btnConnect.clicked += BtnConnect_clicked;
        btnRegister.clicked += BtnRegister_clicked;

    }

    private void BtnRegister_clicked()
    {
        throw new System.NotImplementedException();
    }

    private async void BtnConnect_clicked()
    {
        Debug.Log("connected account " + txtEmail.text);

        LoginVM loginVM = new LoginVM() { Email = txtEmail.text, Password = txtPassword.text };
        // get auth infos
        var token = await ConnectionService.GetTokenAsync(loginVM);      
        GameContext.Instance.Token = token;
        Debug.Log("Token " + token);
        SceneManager.LoadScene("Register");
        Debug.Log("Scene loaded");
    }

    void Update()
    {


    }

}
