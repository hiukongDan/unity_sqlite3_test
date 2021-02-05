using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Data.SQLite;

public class DebugUtility : MonoBehaviour
{
    public void LoadNextScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }

    public void MoveEnemy(int moveDir)
    {
        
        foreach(var enemy in Enemy.EnemyList)
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.Move((Enemy.MoveDirection)moveDir);
            }
        }
    }

    private SQLiteConnection getConn()
    {
        string db_path = "Data Source=" + Application.dataPath + "/" + "data.db";

        return new SQLiteConnection(db_path);
    }
    public void ClearDB()
    {
        SQLiteConnection conn = null;
        try
        {
            conn = getConn();
            conn.Open();
            string cmdtext = "DELETE FROM ";
            string[] tables = { "Entity", "EntityInitial", "Item" };
            for (int i = 0; i < tables.Length; i++)
            {
                using (var cmd = new SQLiteCommand(cmdtext + tables[i], conn))
                {
                    cmd.ExecuteNonQuery();
 /*                   foreach(Enemy enemy in Enemy.EnemyList)
                    {
                        enemy.gameObject.GetComponent<Tester>().SendMessage("DebugReset");
                    }*/
                }
            }

        }
        catch (SQLiteException ex)
        {
            Debug.Log("Error Operation SQLite, brobably due to connection, Error Code: " + ex.ErrorCode);
        }
        finally
        {
            conn?.Dispose();
        }

    }

    public void OnDestroy()
    {
        Enemy.EnemyList.Clear();
    }

}
