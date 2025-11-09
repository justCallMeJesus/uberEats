using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitArea : MonoBehaviour
{
    private List<PlayerInteraction> player = new List<PlayerInteraction>();
    private float timeToLeave = 1.5f;
    private float timePassed = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteraction player))
        {
            player.exitAreasInRange.Add(this);
            this.player.Add(player);
        }
        if(other.TryGetComponent(out Vehicle scooter))
        {
            if(scooter.interacter != null)
            {
                this.player.Add(player);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteraction player))
        {
            player.exitAreasInRange.Remove(this);
            this.player.Remove(player);
        }
        if (other.TryGetComponent(out Vehicle scooter))
        {
            if (scooter.interacter != null)
            {
                this.player.Remove(player);
            }
        }
    }

    private void Update()
    {
        if(player.Count > 0)
        {
            timePassed += Time.deltaTime;
            UIManager.Instance.SetLeavingBar((timePassed / timeToLeave) * 100);
            if (timePassed > timeToLeave)
            {
                GameManager.Instance.PlayerLeft();              
            }
        }
        else
        {
            timePassed = 0;
            UIManager.Instance.TurnOffLeavingBar();
        }
    }
}
