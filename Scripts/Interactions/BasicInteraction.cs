using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInteraction : MonoBehaviour
{
    public enum DIRECTIONS { ANY, DIRECTION_DOWN, DIRECTION_UP, DIRECTION_RIGHT, DIRECTION_LEFT }

    public DIRECTIONS facingDirection;

    public virtual bool Interact(Vector2 playerFacing, Vector2 playerPos)
    {
        return false;
    }

    public virtual bool FacingObject(Vector2 playerFacing)
    {
        switch (facingDirection)
        {
            case DIRECTIONS.DIRECTION_DOWN:
                return playerFacing.y > 0;
            case DIRECTIONS.DIRECTION_UP:
                return playerFacing.y < 0;
            case DIRECTIONS.DIRECTION_LEFT:
                return playerFacing.x > 0;
            case DIRECTIONS.DIRECTION_RIGHT:
                return playerFacing.x < 0;
            case DIRECTIONS.ANY:
                return true;
            default: return false;
        }
    }

    public virtual bool FacingNPC(Vector2 playerFacing, Vector2 playerPos, Vector2 npcPos)
    {
        bool success = false;

        if (playerPos.y < npcPos.y) success = playerFacing.y > 0;
        else if (playerPos.y > npcPos.y) success = playerFacing.y < 0;

        if (!success)
        {
            if (playerPos.x < npcPos.x) success = playerFacing.x > 0;
            else if (playerPos.x > npcPos.x) success = playerFacing.x < 0;
        }

        return success;
    }
}
