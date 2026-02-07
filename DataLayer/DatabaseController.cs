using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using SqlERDiagrammDrawer.SQLBuisnessObj;

namespace SqlERDiagrammDrawer.DataLayer
{
    internal class DatabaseController
    {



        string ConnectionString { get; set; }

        string QueryDump { get; set; }

        public SQLBuisnessObj.Keys[] Keys { get; set; }
        public DatabaseController()
        {



        }
        public Task<List<SQLBuisnessObj.Keys>> GetKeys(string DB, List<SQLEntity> entities)
        {
            return Task.Run(() =>
            {
                List<SQLBuisnessObj.Keys> FKs = new List<SQLBuisnessObj.Keys>();
                string queryKeys = @$"
            SELECT 
                TABLE_NAME ,
                COLUMN_NAME ,
                CONSTRAINT_NAME ,
                REFERENCED_TABLE_NAME ,
                REFERENCED_COLUMN_NAME
            FROM 
                INFORMATION_SCHEMA.KEY_COLUMN_USAGE
            WHERE 
                TABLE_SCHEMA = '{DB}'
                AND REFERENCED_TABLE_NAME IS NOT NULL;";

                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand(queryKeys, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableName = reader["TABLE_NAME"].ToString();
                            string columnName = reader["COLUMN_NAME"].ToString();
                            string referencedTable = reader["REFERENCED_TABLE_NAME"].ToString();
                            string referencedColumn = reader["REFERENCED_COLUMN_NAME"].ToString();
                            SQLBuisnessObj.Keys keys = new SQLBuisnessObj.Keys();
                            SQLEntity primary = entities.First((x) => x.NameEntity == referencedTable);
                            SQLEntity foreing = entities.First((x) => x.NameEntity == tableName);

                            keys.ForeingTableName = foreing.NameEntity;
                            keys.PrimaryTableName = primary.NameEntity;

                            keys.PrimaryKey = primary.Fields.First((x) => x.Primary);
                            keys.ForeingKey = foreing.Fields.First((x) => x.Name == columnName);


                            FKs.Add(keys);
                            Debug.Write(tableName + " " + columnName + " " + referencedColumn + " " + referencedTable);
                        }
                    }

                    connection.Close();
                    return FKs;
                }

            });
        }
        public Task<List<SQLEntity>> GetEntities(string DB)
        {


            // Query SQL
            return Task.Run(() =>
            {
                string queryEntity = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{DB}';";
                string result = "";
                List<SQLEntity> entities = new List<SQLEntity>();

                // Connessione al database
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        using (MySqlCommand cmd = new MySqlCommand(queryEntity, conn))
                        {
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string name = (string)reader["TABLE_NAME"];
                                    Debug.WriteLine(name);
                                    SQLEntity entity = new SQLEntity();
                                    entity.NameEntity = name;

                                    entities.Add(entity);

                                }
                            }
                        }
                        foreach (SQLEntity entity in entities)
                        {


                            string tableName = entity.NameEntity;

                            string queryFields = $@"
                           SELECT
                         COLUMN_NAME,
                         DATA_TYPE,
                        IS_NULLABLE,
                         COLUMN_KEY,
                         EXTRA
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_SCHEMA = '{DB}'
                         AND TABLE_NAME = @tableName;";




                            using (MySqlCommand cmd = new MySqlCommand(queryFields, conn))
                            {
                                cmd.Parameters.AddWithValue("@tableName", tableName);

                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string nameAttribute = reader.GetString("COLUMN_NAME");
                                        Debug.WriteLine(nameAttribute);
                                        string type = reader.GetString("DATA_TYPE");

                                        string isNullableStr = reader.GetString("IS_NULLABLE");
                                        string columnKey = reader.GetString("COLUMN_KEY");
                                        string extra = reader.GetString("EXTRA");

                                        bool isNullable = isNullableStr == "YES";
                                        bool isPrimaryKey = columnKey == "PRI";
                                        bool isAutoIncrement = extra.Contains("auto_increment");

                                        int? typeKey = MyValuesDictionary.EntityVariableType
                                                      .First(x => x.Value.Contains(type)).Key;
                                        if (typeKey != null)
                                        {
                                            entity.Fields.Add(new SQLField
                                            {
                                                Name = nameAttribute,
                                                VariableTyp = (int)typeKey,
                                                Nullable = isNullable,
                                                Primary = isPrimaryKey,
                                                AutoIncremental = isAutoIncrement
                                            });
                                        }

                                    }
                                }
                            }

                        }
                        conn.Close();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }



                return entities;
            });


        }

        public Task<bool> TryConnection(string Server, string Db, string User, string Password)
        {

            return Task.Run(() =>
            {
                ConnectionString = $"Server={Server};Database={Db};User={User};Password={Password};";
                using (MySqlConnection SqlConn = new MySqlConnection(ConnectionString))
                {
                    try
                    {


                        SqlConn.Open();
                        MessageBox.Show("Connection successful!");
                        SqlConn.Close();
                        return true;



                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"Error: {ex.Message}");
                        return false;
                    }


                }


            });
        }

        public void CreateKey(SQLBuisnessObj.Keys[] keys, RichTextBox p, FormForwardEng ctrl)
        {


            bool error = false;
            SQLCommandBuilder commandCreator = new SQLCommandBuilder();
            using (MySqlConnection MySqlConn = new MySqlConnection(ConnectionString))
            {

                for (int i = 0; i < keys.Length; i++)
                {
                    string command = commandCreator.CreateKey(keys[i]);


                    p.Invoke(new Action(() =>
                    {
                        p.AppendText(command + Environment.NewLine);

                    }));





                    MySqlCommand sqlCommand = new MySqlCommand(command, MySqlConn);

                    try
                    {
                        sqlCommand.ExecuteNonQueryAsync();
                        p.Invoke(new Action(() =>
                        {
                            p.AppendText(command);
                            p.AppendText(
                                "Key creata con successo: " +
                                keys[i].PrimaryTableName + "/" +
                                keys[i].ForeingTableName +
                                Environment.NewLine
                            );
                        }));
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        p.Invoke(new Action(() =>
                        {
                            p.AppendText($"Error: {ex.Message}");

                        }));


                    }




                }
                if (!error)
                {
                    ctrl.ok++; ; ;


                }
            }

        }






        public Task CreateTables(SQLEntity[] entities, SQLBuisnessObj.Keys[] keys, RichTextBox p, FormForwardEng ctrl)
        {

            return Task.Run(async () =>
            {
                bool error = false;
                SQLCommandBuilder commandCreator = new SQLCommandBuilder();
                using (MySqlConnection SqlConnection = new MySqlConnection(ConnectionString))
                {
                    SqlConnection.Open();
                    for (int i = 0; i < entities.Length; i++)
                    {
                        string drop = commandCreator.DropTable(entities[i]);
                        string command = commandCreator.CreateEntitySqlCommand(entities[i]);

                        if (p.InvokeRequired)
                        {
                            p.Invoke(new Action(() =>
                            {
                                p.AppendText(drop + Environment.NewLine);
                                p.AppendText(command + Environment.NewLine);

                            }));






                        }
                        MySqlCommand dropCommand = new MySqlCommand(drop, SqlConnection);

                        MySqlCommand sqlCommand = new MySqlCommand(command, SqlConnection);

                        try
                        {
                            dropCommand.ExecuteNonQueryAsync();
                            sqlCommand.ExecuteNonQueryAsync();
                            p.Invoke(new Action(() =>
                            {
                                p.AppendText("Tabella creata con successo: " + entities[i].NameEntity + Environment.NewLine);
                            }));





                        }
                        catch (Exception ex)
                        {
                            error = true;
                            p.Invoke(new Action(() =>
                            {
                                p.AppendText($"Error: {ex.Message}");

                            }));


                        }


                    }

                    if (!error)
                    {
                        ctrl.Invoke(new Action(() => { ctrl.ok++; ; }));
                        CreateKey(keys, p, ctrl);


                    }
                    SqlConnection.Close();
                }
            });


        }


    }
}
