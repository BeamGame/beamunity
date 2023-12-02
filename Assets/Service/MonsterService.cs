using System.Collections.Generic;
using System.Threading.Tasks;
using Debug = UnityEngine.Debug;

namespace Assets.Service
{
    public class MonsterService
    {
        private static string serviceUrl = "https://localhost:7062/api/monster";


        static MonsterService()
        {
            serviceUrl = "https://imxserver.azurewebsites.net/api/monster";

#if UNITY_EDITOR
            serviceUrl = "https://localhost:7062/api/monster";
#endif

        }

        public static async Task<List<TokenDto>> MintMonster(AddMonsterDto monsterInfo)
        {
            Debug.Log("MintMonster");
            var url = serviceUrl + "/MintMonster";
            return await UnityRequestClient.Post<List<TokenDto>>(url, monsterInfo);
        }

        public static async Task<bool> UpdateMonster(UpdateMonsterDto monsterInfo)
        {
            Debug.Log("UpdateMonster");
            var url = serviceUrl + "/UpdateMonster";
            return await UnityRequestClient.Post<bool>(url, monsterInfo);
        }

        public static async Task<List<TokenDto>> GetMonsters()
        {
            Debug.Log("GetMonsters");
            var url = serviceUrl + "/GetMonsters";
            return await UnityRequestClient.Get<List<TokenDto>>(url);
        }

        public static async Task<List<TokenDto>> TransferMonster(int tokenId, string playerName)
        {
            Debug.Log("TransferMonster");
            var url = serviceUrl + "/TransferMonsters";
            var monsterInfo = new TransferMonsterDto()
            {
                TokenId = tokenId,
                UserName = playerName
            };
            return await UnityRequestClient.Post<List<TokenDto>>(url, monsterInfo);
        }
    }
}
