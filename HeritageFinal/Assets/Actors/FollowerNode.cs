using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerNode : MonoBehaviour {

    private int direction;
    private Vector2 position;

    public void setNode(int dir, Vector2 pos)
    {
        direction = dir;
        position = pos;
    }
    public int getNode(Vector2 playerPos)
    {
        if (playerPos == position)
        {
            return direction;
        }
        return 0;
    }


}
