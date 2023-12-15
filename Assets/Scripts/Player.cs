using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPosition;
    private float lerpingSmoothness = 8f;

    private void Update()
    {
        if (photonView.IsMine)
        {
            // Adicione o código para o movimento local do jogador
            MoveLocally();
        }
        else
        {
            // Sincronize a posição do jogador para os outros jogadores
            SmoothMove();
        }
    }

    void MoveLocally()
    {
        // Adicione o código para o movimento local do jogador
        // Por exemplo, usando Input.GetAxis para movimentação
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal, 0f, vertical) * Time.deltaTime * 15);
    }

    private void SmoothMove()
    {
        // Lerp para suavizar o movimento sincronizado
        transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * lerpingSmoothness);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Envia a posição para os outros jogadores
            stream.SendNext(transform.position);
        }
        else
        {
            // Recebe a posição dos outros jogadores
            networkPosition = (Vector3)stream.ReceiveNext();
        }
    }

}
