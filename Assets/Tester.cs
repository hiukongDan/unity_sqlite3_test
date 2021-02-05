using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Data.SQLite;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update

    public string GOname = "tester";
    public int hp = 100;
    public int attack = 10;
    public bool isDead = false;

    //public bool isDebugReset = false;
    
    void Start()
    {
        string db_path = "Data Source=" + Application.dataPath + "/" + "data.db";
        var conn = new SQLiteConnection(db_path);

        conn.Open();
        int id = GetInstanceID();

        string cmdtext = "SELECT entityid, name, hp, attack, isdead, position " +
                "FROM Entity " +
                "WHERE entityid = " + id;

        using (var cmd = new SQLiteCommand(cmdtext, conn))
        {
            var reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Debug.Log("has row");
                reader.Read();
                GOname = reader.GetString(1);
                hp = reader.GetInt32(2);
                attack = reader.GetInt32(3);
                isDead = reader.GetBoolean(4);
                transform.position = PositionFromString(reader.GetString(5));
            }
            else
            {
                // set cmd after closing reader
                reader.Close();
                // INSERT
                cmd.CommandText = "INSERT INTO Entity (entityid, name, hp, attack, isdead, position) " +
                    string.Format("VALUES({0},\"{1}\",{2},{3},{4},\"{5}\")", id, GOname, hp, attack, isDead ? 1 : 0, transform.position.ToString());
                //Debug.Log(cmd.CommandText);
                int res = cmd.ExecuteNonQuery();
/*                if (res != 0)
                {
                    Debug.Log("Insert " + GOname + " successfully!");
                }
*/

                cmd.CommandText = "Insert INTO EntityInitial (entityid, name, hp, attack, position) " +
                    string.Format("VALUES({0},\"{1}\",{2},{3},\"{4}\")", id, GOname, hp, attack, transform.position.ToString());
                cmd.ExecuteNonQuery();
            }
        }
        conn.Dispose();

    }

    private Vector3 PositionFromString(string pos)
    {
        string[] item = pos.Substring(1, pos.Length - 2).Split(',');
        return new Vector3(float.Parse(item[0]), float.Parse(item[1]), float.Parse(item[2]));
    }

    void OnDestroy()
    {
        // saving
        string db_path = "Data Source=" + Application.dataPath + "/" + "data.db";
        var conn = new SQLiteConnection(db_path);

        conn.Open();
        var cmd = new SQLiteCommand(conn);

        int id = GetInstanceID();
        
        cmd.CommandText = "UPDATE Entity " +
            string.Format("SET name=\"{0}\",hp={1},attack={2},isdead={3},position=\"{4}\" ", GOname, hp, attack, (isDead?1:0), transform.position.ToString()) +
            "WHERE entityid = " + id;

        int res = cmd.ExecuteNonQuery();
        if (res != 0)
        {
            Debug.Log("Update " + GOname + " successfully!");
        }
        conn.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
