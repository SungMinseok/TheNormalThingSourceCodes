using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float xMargin = 1.5f;
    public float yMargin = 1.5f;

    public float xSmooth = 1.5f;
    public float ySmooth = 1.5f;

    public Vector2 maxXAndY;
    public Vector2 minXAndY;

    public Transform player;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;

        if (player == null)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다.");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
