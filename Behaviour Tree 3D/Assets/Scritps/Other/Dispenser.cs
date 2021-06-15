using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            BaseAI baseAI = other.gameObject.GetComponent<BaseAI>();

            if (baseAI != null)
            {
                baseAI.GetAbility().IsAvailable = true;
            }
            else
            {
                Debug.LogError("FinishLine.cs: BaseAI Component is missing");
            }
        }
    }
}
