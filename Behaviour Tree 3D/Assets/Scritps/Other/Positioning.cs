using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PositionData
{
    Agent _agent;
    float _distance;

    public PositionData(Agent agent, float distance)
    {
        this._agent = agent;
        this._distance = distance;
    }

    public Agent Agent { get => _agent; set => _agent = value; }
    public float Distance { get => _distance; set => _distance = value; }
}

[RequireComponent(typeof(Leaderboard))]
public class Positioning : MonoBehaviour
{
    List<PositionData> _positions = new List<PositionData>();
    const float _updateTime = 0.5f;
    float _timeElapsed = 0.0f;

    private void Start()
    {
        
    }

    void Update()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed > _updateTime)
        {
            _timeElapsed -= _updateTime;
            UpdatePositions();
        }

        if(_positions[_positions.Count - 1].Agent.AIComponent.CheckIfFinished() && GetComponent<MainScript>().RaceInProgress)
        {
            GetComponent<MainScript>().RaceInProgress = false;
            GetComponent<Results>().Summarize();
        }
    }

    static int SortByDistance(PositionData p1, PositionData p2)
    {
        return p1.Distance.CompareTo(p2.Distance);
    }

    public void CreatePositions(List<Agent> agents_in)
    {
        for (int i = 0; i < agents_in.Count; i++)
        {
            _positions.Add(new PositionData(agents_in[i], 0));
        }
    }

    public List<Agent> GetPositions()
    {
        List<Agent> agents = new List<Agent>();

        for (int i = 0; i < _positions.Count; i++)
        {
            agents.Add(_positions[i].Agent);
        }

        return agents;
    }

    public Agent GetAgentAtPosition(int position_in)
    {
        return _positions[position_in - 1].Agent;
    }

    void UpdatePositions()
    {
        for (int i = 0; i < _positions.Count; i++)
        {
            PositionData pd = _positions[i];

            if (!pd.Agent.AIComponent.CheckIfFinished())
                pd.Distance = _positions[i].Agent.AIComponent.CalculateRemainingDistance();
            else
                pd.Distance = (-10.0f + pd.Agent.Position);

            _positions[i] = pd;
        }

        _positions.Sort(SortByDistance);

        for (int i = 0; i < _positions.Count; i++)
        {
            _positions[i].Agent.Position = i + 1;
        }

        GetComponent<Leaderboard>().UpdateLeaderboard();
    }
}
