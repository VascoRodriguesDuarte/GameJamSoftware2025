using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class VisualTunnel : MonoBehaviour
{

    [SerializeField]
    Transform tunnelPrefab;

    [SerializeField]
    Movement player;

    [SerializeField]
    TerrainManager TM;

    [SerializeField, Range(50,300)]
    private int maxPieces = 50;

    [SerializeField]
    private bool lingering = true;
    private Transform[] pieces;

    private int i = 0;

    [SerializeField, Range(5,200)]
    public int TicksPerSecond = 50;

    private GameObject visualTunnel;
    private float dur;

    private Vector3 Scale;

    private float _t = 0f;
    void Update() {

        _t += Time.deltaTime;
        while(_t >= dur)
        {
            _t -= dur;

            if (CanDig() && CanCreate()) {
                createPiece();
            }
        }

    }

    private bool CanDig() {
        return GameTerrain.GetMajorType(TM.CurrentTerrain) == GameTerrain.MajorType.Land;
    }

    private bool CanCreate() {
        if (!lingering) {
            return true;
        } else {
            if (player.GetCurentSpeed() != 0) {
                visualTunnel.SetActive(true);
                return true;
            } else {
                visualTunnel.SetActive(false);
                var idx = 0;
                foreach (Transform y in pieces) {
                    if (pieces[idx] != null) {
                        Destroy(pieces[idx].gameObject);
                    }
                    pieces[idx] = null;
                    idx++;
                }
                passthrough = false;
                return false;
            }
        }
    }

    private float lastSpeed;
    private void createPiece() {
        Transform piece = getOrInstantiatePiece();

        addPieceToList(piece);

        var speed = player.GetCurentSpeed();
        float animSpeed = player.GetCurentSpeed()/player.getCurrentMovement().defaultSpeed;
        if (lastSpeed != speed) {
            foreach (Transform y in pieces) {
                var animator = y.GetChild(0).GetComponent<Animator>();
                animator.speed=animSpeed;
            }
        } else {
            var animator = piece.GetChild(0).GetComponent<Animator>();
            animator.speed=animSpeed;
        }
        lastSpeed = speed;

        var p = player.getTransform();
        player.GetCurentSpeed();
        if (!passthrough) {
            piece.localScale = Scale;
        }        
        piece.position = p.position + p.up*0.80f;
        piece.rotation = p.rotation;
        piece.SetParent(visualTunnel.transform, true);
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
            var animator = p.GetChild(0).GetComponent<Animator>();
            animator.ResetTrigger("again");
            animator.SetTrigger("again");
            return p;
        }
    }

    private void Awake() {
        pieces = new Transform[maxPieces];
        dur = 1f / TicksPerSecond;
        Scale = player.getTransform().localScale;
        lastSpeed = player.GetCurentSpeed();
    }

    private void Start() {
        visualTunnel = new GameObject("VisualTunnel");
        visualTunnel.transform.SetParent(transform, true);
    }

}
