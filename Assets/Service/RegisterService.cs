using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class RegisterService
{
    private static string serviceUrl = "https://localhost:7171/api/register";


    static RegisterService()
    {
        serviceUrl = "https://beam-server.azurewebsites.net/api/register";

#if UNITY_EDITOR
        serviceUrl = "https://localhost:7171/api/register";
#endif

        //GameContext.Instance.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6IjB4NjcwY0FjZjQ4QjY4NWVCMUFGOGRjNzNDNThBQWJkMzBhQTM1OTU4RSIsImp0aSI6ImIzMzJlMzMyLTAxMWQtNGJiZi1iMGFjLTU3MmJjMWU2YmM4MCIsImV4cCI6MTY3NTk0NTE3NX0.1HGytwdV0ZWu5Jyv7XudX5qg31EYWIncmSeQPhYas7o";

    }

    public static async Task<PlayerDto> CreatePlayer(string name)
    {
        Debug.Log("CreatePlayer");
        var url = serviceUrl + "/CreatePlayer";
        var data = new PlayerName()
        {
            Name = name
        };
        return await UnityRequestClient.Post<PlayerDto>(url, data);
    }

    public static async Task<PlayerDto> GetPlayer()
    {
        Debug.Log("GetPlayer");
        var url = serviceUrl + "/GetPlayer";
        return await UnityRequestClient.Get<PlayerDto>(url);
    }

    public static async Task<List<string>> GetUsers()
    {
        Debug.Log("GetPlayer");
        var url = serviceUrl + "/GetUsers";
        return await UnityRequestClient.Get<List<string>>(url);
    }
}
