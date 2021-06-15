using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Leaderboard : MonoBehaviour
{
    TextMeshProUGUI _tmPro;
    List<Agent> _agents;

    void Awake()
    {
        _tmPro = GameObject.Find("Leaderboard").GetComponent<TextMeshProUGUI>();
        _agents = new List<Agent>();
    }

    public void UpdateLeaderboard()
    {
        _agents = GetComponent<Positioning>().GetPositions();
        _tmPro.text = "";

        for(int i = 0; i < _agents.Count; i++)
        {
            _tmPro.color = new Color(255, 255, 0);
            _tmPro.text += ((i + 1) + ". ");
            _tmPro.text += _agents[i].GameObjectHolder.name;
            _tmPro.text += "\n";
        }
    }
}
