using System.Collections;
using System.Collections.Generic;
using Assets.Service;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Trade : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset pokemonTrade;

    [SerializeField]
    VisualTreeAsset UserTrade;

    protected VisualElement root;
    protected VisualElement vePokemon;
    protected VisualElement veUser;
    protected Button btnTrade;
    protected Button btnClose;
    protected DropdownField ddPokemon;
    protected DropdownField ddUser;

    private List<TokenDto> token;
    private List<string> name;

    // Start is called before the first frame update
    async void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        btnTrade = root.Q<Button>("btnTrade");
        btnClose = root.Q<Button>("btnClose");
        ddPokemon = root.Q<DropdownField>("ddPokemon");
        ddUser = root.Q<DropdownField>("ddUser");

        btnClose.clicked += BtnClose_clicked;
        btnTrade.clicked += BtnTrade_clicked;

        //var monsters = await MonsterService.GetMonsters();
        var players = await RegisterService.GetUsers();

        ddUser.choices = players;
    }

    private void BtnTrade_clicked()
    {
        ///throw new System.NotImplementedException();
    }

    private void BtnClose_clicked()
    {
        SceneManager.UnloadScene("Trade");
    }


    
    // Update is called once per frame
    void Update()
    {

    }
}
