using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private bool FollowRotation;

    private Player player;
    private Vector3 PlayerPos
    {
        get
        {
            return new Vector3(
                player.transform.position.x,
                player.transform.position.y,
                transform.position.z
           );
        }
    }

    private Vector3 PlayerPosLocal
    {
        get
        {
            return player.transform.InverseTransformPoint(PlayerPos);
        }
    }

    private Quaternion PlayerRotation
    {
        get
        {
            return player.transform.rotation;
        }
    }

    [SerializeField]
    float offset;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void LateUpdate()
    {
        Vector3 localPos = new Vector3(PlayerPosLocal.x, PlayerPosLocal.y - offset, PlayerPosLocal.z);
        Vector3 worldPos = player.transform.TransformPoint(localPos);
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);
        if (FollowRotation)
        {
            var playerRotationEurler = PlayerRotation.eulerAngles;
            var playerRotation = Quaternion.Euler(0, 0, playerRotationEurler.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, playerRotation, 0.1f);
        }
    }
}
