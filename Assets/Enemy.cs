using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static List<Enemy> enemyList;
    public static List<Enemy> EnemyList
    {
        get
        {
            if (enemyList == null)
            {
                enemyList = new List<Enemy>();
            }
            return enemyList;
        }
    }

    public float MaxSpeed = 10f;
    public float Fiction = 0.1f;
    //public float Acc = 1f;

    private Vector2 velocity = new Vector2(0, 0);
    //public Vector2 moveDir = new Vector2(0, 0);
    
    public enum MoveDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        STOP,
    };

    void OnEnable()
    {
        if (!EnemyList.Contains(this))
        {
            EnemyList.Add(this);
        }
    }

    public void Move(MoveDirection md)
    {
        switch (md)
        {
            case MoveDirection.LEFT:
                //moveDir.x = -1;
                velocity = new Vector2(-MaxSpeed, velocity.y);
                break;
            case MoveDirection.RIGHT:
                //moveDir.x = 1;
                velocity = new Vector2(MaxSpeed, velocity.y);
                break;
            case MoveDirection.UP:
                //moveDir.y = 1;
                velocity = new Vector2(velocity.x, MaxSpeed);
                break;
            case MoveDirection.DOWN:
                // moveDir.y = -1;
                velocity = new Vector2(velocity.x, -MaxSpeed);
                break;
            case MoveDirection.STOP:
                velocity = new Vector2(0,0);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(velocity.ToString());
        if(velocity.magnitude > 0.01f)
        {
            Vector2 targetPos = new Vector2(transform.position.x + velocity.x * Time.deltaTime, transform.position.y + velocity.y * Time.deltaTime);
            transform.position = Vector2.Lerp(targetPos, transform.position, Time.deltaTime);
        }

        // apply fiction
        //velocity.x -= Mathf.Cos(Mathf.Atan(velocity.y / velocity.x)) * Fiction * Time.deltaTime;
        //velocity.y -= Mathf.Sin(Mathf.Atan(velocity.y / velocity.x)) * Fiction * Time.deltaTime;

    }
}
