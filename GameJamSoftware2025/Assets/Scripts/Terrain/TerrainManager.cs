using System;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{

    [SerializeField] private Movement player;
    [SerializeField] private GameTerrain.MinorType StartingTerrain = GameTerrain.MinorType.Dentin;

    public GameTerrain.MinorType CurrentTerrain {get;private set;}

[SerializeField]
    private List<GameTerrain.MinorType> list;

    private int getOrder(GameTerrain.MinorType type) {
        int idx = 0;
        foreach (var item in list)
        {
            if (item == type) {
                break;
            }
            idx++;
        }
        return idx;
    }
    private bool onTop(GameTerrain.MinorType typea, GameTerrain.MinorType typeb) {
        //Debug.Log("typea: " + typea.ToString() + " - " + getOrder(typea) + "typeb: " + typeb.ToString() + " - " + getOrder(typeb));
        return getOrder(typea) < getOrder(typeb);
    }

    private void ExecuteChangeTerrain(GameTerrain.MinorType entryType, Dictionary<String, Vector2> additional) {
        Debug.Log("Changing Terrain to: " + entryType.ToString());
        if (InTunnel == 0) {

            player.ChangeTerrainMovement(entryType, additional);
        }
        //Debug.Log(entryType.ToString());
        CurrentTerrain = entryType;
    }

private int InTunnel = 0;
    private void EnterTunnel() {
        Debug.Log("Entering Tunnel");
        InTunnel += 1;
        if (InTunnel != 0) {
            player.ChangeTerrainMovement(GameTerrain.MinorType.Air, null);
        }
    }

    private void ExitTunnel() {
        Debug.Log("Exiting Tunnel");
        InTunnel -= 1;
        if (InTunnel == 0) {
            player.ChangeTerrainMovement(CurrentTerrain, null);
        }
    }
    private GameTerrain.MinorType lastEntry;
    public void ChangeTerrain(GameTerrain.MinorType type, bool entry, bool isTunnel, Dictionary<String, Vector2> additional) {
        if (entry) {
            //Debug.Log(CurrentTerrain.ToString());
            //Debug.Log(lastEntry.ToString());
            if (isTunnel) {
                EnterTunnel();
                return;
            } 
            Debug.Log("entered "+type.ToString());
            if (GameTerrain.GetMajorType(type) != GameTerrain.MajorType.Solid) {
                lastEntry = type;
            }
            if (CurrentTerrain == type) {
                return;
            } else {
                if (onTop(type, CurrentTerrain)) {
                    ExecuteChangeTerrain(type, additional);
                } else {
                    return;
                }
            }
        } else {
            if (isTunnel) {
                ExitTunnel();
                return;
            } 
            Debug.Log("left "+type.ToString());
            if (CurrentTerrain == type) { 
                if (lastEntry == type) {
                    return;
                } else {
                    ExecuteChangeTerrain(lastEntry, additional);
                }
            } else {
                return;
            }
        }
    }

    public void ForceChangeTerrain(GameTerrain.MinorType type, Dictionary<String, Vector2> additional) {
        ExecuteChangeTerrain(type, additional);
    }

    

    private void Awake() {
        CurrentTerrain = StartingTerrain;
        lastEntry = StartingTerrain;
    }

}
