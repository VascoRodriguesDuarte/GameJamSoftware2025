using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TerrainCollider : MonoBehaviour
{

    [SerializeField]
    private GameTerrain.MinorType type;

    [SerializeField]
    private bool IsTunnel = false;

    [SerializeField]
    private bool IsWall = false;

    private TerrainManager TM;

    private Dictionary<String, Vector2> dict = new Dictionary<String, Vector2>();

    ContactPoint2D[] contacts = new ContactPoint2D[2];
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("OnTriggerEnter2D call");
        if (other.gameObject.CompareTag("Player")) {
            TM.ChangeTerrain(type, true, IsTunnel, dict);
        }
        //other.GetComponent<Movement>().ChangeTerrainMovement(GameTerrain.MinorType.Air);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("OnCollisionEnter2D call");
        if (other.gameObject.CompareTag("Player")) {
            if (GameTerrain.GetMajorType(type) == GameTerrain.MajorType.Solid) {
                Vector2 normal = other.contacts[0].normal;
                dict["normal"] = normal;
                //Debug.Log("normal: " + normal.ToString());
            }
            TM.ChangeTerrain(type, true, IsTunnel, dict);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && GameTerrain.GetMajorType(type) == GameTerrain.MajorType.Air && !IsTunnel) {
            TM.ForceChangeTerrain(type, dict);
        } else if (other.gameObject.CompareTag("Player") && 
            GameTerrain.GetMajorType(type) == GameTerrain.MajorType.Land && 
            !IsTunnel && 
            GameTerrain.GetMajorType(TM.CurrentTerrain) != GameTerrain.MajorType.Land
            ) {
            TM.ForceChangeTerrain(type, dict);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            TM.ChangeTerrain(type, false, IsTunnel, dict);
        }
        //other.GetComponent<Movement>().ChangeTerrainMovement(GameTerrain.MinorType.Enamel);
    }

    private void Start() {
        if (!IsTunnel && !IsWall) {
            TM = transform.parent.GetComponent<TerrainManager>();
        } else {
            TM = transform.parent.parent.GetComponent<TerrainManager>();   
        }
    }
}
