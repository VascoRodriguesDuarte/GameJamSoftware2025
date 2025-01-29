using System.Collections.Generic;
using UnityEngine;

public class Tunnel : MonoBehaviour
{

    [SerializeField]
    Transform tunnelPrefab;

    [SerializeField]
    Movement player;

    [SerializeField]
    TerrainManager TM;

    [SerializeField, Range(100,1000)]
    private int maxPieces = 300;
    private Transform[] pieces;

    private int i = 0;


    [SerializeField, Range(5,50)]
    public int TicksPerSecond = 20;
    private float dur;

    private Vector3 Scale;

    private float _t = 0f;
    void Update() {

        _t += Time.deltaTime;
        while(_t >= dur)
        {
            _t -= dur;

            if (CanDig()) {
                createPiece();
            }
        }

    }

    private bool CanDig() {
        return (GameTerrain.GetMajorType(TM.CurrentTerrain) == GameTerrain.MajorType.Land) && GameTerrain.IsDiggable(TM.CurrentTerrain);
    }

    private void createPiece() {
        Transform piece = getOrInstantiatePiece();

        addPieceToList(piece);

        var p = player.getTransform();
        if (!passthrough) {
            piece.localScale = Scale;
        }
        piece.position = p.position - p.up*0.70f;
        piece.rotation = p.rotation;
        piece.SetParent(transform, true);
    }

    private void addPieceToList(Transform piece) {
        pieces[i] = piece;
        i++;    
        if (i >= maxPieces) {
            i = 0;
        }
    }

    private bool passthrough = false;

    private Transform getOrInstantiatePiece() {
        var p = pieces[i];
        if (p == null) {
            return Instantiate(tunnelPrefab);
        } else {
            if (!passthrough) {
                passthrough = true;
            }
            return p;
        }
    }

    private void Awake() {
        pieces = new Transform[maxPieces];
        dur = 1f / TicksPerSecond;
        Scale = player.getTransform().localScale;
    }

}
