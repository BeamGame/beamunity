using Assets.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("HaveStarter", 5f, 60f);
    }

    async void HaveStarter()
    {
        var questList = QuestList.GetQuestList();
        var ms = await MonsterService.HaveStarter();
        if (ms)
        {
            questList.Complete("Meet Professor");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
