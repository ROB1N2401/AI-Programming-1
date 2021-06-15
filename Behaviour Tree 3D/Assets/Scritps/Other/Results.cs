using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct ResultData
{
    int _points;
    Team _team;

    public ResultData(int points, Team team)
    {
        _points = points;
        _team = team;
    }

    public int Points { get => _points; set => _points = value; }
    public Team Team { get => _team; set => _team = value; }
}

public class Results : MonoBehaviour
{
    GameObject _gameObject;
    List<TextMeshProUGUI> _standings;

    List<ResultData> _results = new List<ResultData>();

    // Start is called before the first frame update
    void Start()
    {
        _results.Add(new ResultData(0, Team.RED));
        _results.Add(new ResultData(0, Team.GREEN));
        _results.Add(new ResultData(0, Team.BLUE));
        _gameObject = GameObject.Find("Results");
        _gameObject.SetActive(false);
        _standings = new List<TextMeshProUGUI>();
        
        for(int i = 0; i < _gameObject.transform.childCount; i++)
        {
            _standings.Add(_gameObject.transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>());
        }
    }

    public void CountIn(Agent agent_in)
    {
        ResultData rd;
        switch (agent_in.AgentTeam)
        {
            case Team.RED:
                rd = _results[0];
                rd.Points += GetPointsFromPosition(agent_in.Position) * agent_in.PointsMultiplier;
                _results[0] = rd;
                break;
            case Team.GREEN:
                rd = _results[1];
                rd.Points += GetPointsFromPosition(agent_in.Position) * agent_in.PointsMultiplier;
                _results[1] = rd;
                break;
            case Team.BLUE:
                rd = _results[2];
                rd.Points += GetPointsFromPosition(agent_in.Position) * agent_in.PointsMultiplier;
                _results[2] = rd;
                break;
            default:
                break;
        }
    }

    static int SortByScore(ResultData p1, ResultData p2)
    {
        return p1.Points.CompareTo(p2.Points);
    }

    public void Summarize()
    {
        _gameObject.SetActive(true);
        _results.Sort(SortByScore);

        for (int i = 0; i < _results.Count; i++)
        {
            switch (_results[i].Team)
            {
                case Team.RED:
                    _standings[i].color = new Color(255, 0, 0);
                    _standings[i].text += ("RED (" + _results[i].Points + ")");
                    break;
                case Team.GREEN:
                    _standings[i].color = new Color(0, 255, 0);
                    _standings[i].text += ("GREEN (" + _results[i].Points + ")");
                    break;
                case Team.BLUE:
                    _standings[i].color = new Color(0, 0, 255);
                    _standings[i].text += ("BLUE (" + _results[i].Points + ")");
                    break;
                default:
                    break;
            }
        }
    }

    private int GetPointsFromPosition(int position_in)
    {
        int pts = 0;
        switch (position_in)
        {
            case 1:
                pts = 12;
                break;
            case 2:
                pts = 10;
                break;
            case 3:
                pts = 8;
                break;
            case 4:
                pts = 6;
                break;
            case 5:
                pts = 5;
                break;
            case 6:
                pts = 4;
                break;
            case 7:
                pts = 3;
                break;
            case 8:
                pts = 2;
                break;
            case 9:
                pts = 1;
                break;
            default:
                Debug.LogError("Failed to assign points to the agent");
                break;
        }

        return pts;
    }
}
