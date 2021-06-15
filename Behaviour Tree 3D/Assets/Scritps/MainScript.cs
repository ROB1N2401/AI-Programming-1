using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Positioning))]
public class MainScript : MonoBehaviour
{
    public bool RaceInProgress = true;

    List<Agent> agents = null;

    void Awake()
    {
        agents = new List<Agent>();
    }

    void Start()
    {
        this.gameObject.AddComponent<SceneReboot>();
        this.gameObject.AddComponent<Results>();
        this.gameObject.AddComponent<Positioning>();
        GameObject finishLine = GameObject.FindGameObjectWithTag("Finish");
        finishLine.transform.GetChild(0).gameObject.AddComponent<FinishLine>();

        GameObject dispensersParent = GameObject.Find("Dispensers");
        for(int i = 0; i < dispensersParent.transform.childCount; i++)
        {
            dispensersParent.transform.GetChild(i).gameObject.AddComponent<Dispenser>();
        }
 
        CreateAgents();
        GetComponent<Positioning>().CreatePositions(agents);
    }

    void Update()
    {

    }

    void CreateAgents()
    {
        agents.Add(new Agent(Team.BLUE, Type.LEADER));
        agents.Add(new Agent(Team.BLUE, Type.INTERCEPTOR));
        agents.Add(new Agent(Team.BLUE, Type.DEFENDER));
        agents.Add(new Agent(Team.RED, Type.LEADER));
        agents.Add(new Agent(Team.RED, Type.INTERCEPTOR));
        agents.Add(new Agent(Team.RED, Type.DEFENDER));
        agents.Add(new Agent(Team.GREEN, Type.LEADER));
        agents.Add(new Agent(Team.GREEN, Type.INTERCEPTOR));
        agents.Add(new Agent(Team.GREEN, Type.DEFENDER));
    }
}


