using System.Collections.Generic;
using UnityEngine;

public class Tunnel : MonoBehaviour
{

    [SerializeField]
    Transform tunnelPrefab;

    [SerializeField]
    Movement player;

    [SerializeField, Range(100,500)]
    private int maxPieces = 300;
    private Transform[] pieces;

    private int i = 0;


    [SerializeField, Range(5,30)]
    public int TicksPerSecond = 20;
    private float _t = 0f;
    void Update() {

        float dur = 1f / TicksPerSecond;
        _t += Time.deltaTime;
        while(_t >= dur)
        {
            _t -= dur;
            if (player.GetCurrentTerrain() == Terrain.Type.Land) {
                createPiece();
            }
        }

    }

    private bool delete = false;
    private void createPiece() {
        Transform piece = Instantiate(tunnelPrefab);

        addPieceToList(piece);

        var p = player.getTransform();
        piece.localScale = p.localScale;
        piece.position = p.position - p.up*0.75f;
        piece.rotation = p.rotation;
        piece.SetParent(transform,true);
    }

    private void addPieceToList(Transform piece) {
    
        if (delete) {
            Destroy(pieces[i].gameObject);
        } 
        pieces[i] = piece;
        i++;
        if (i >= maxPieces) {
            i = 0;
            delete = true;
        }
    }

    private void Awake() {
        pieces = new Transform[maxPieces];
    }

}
