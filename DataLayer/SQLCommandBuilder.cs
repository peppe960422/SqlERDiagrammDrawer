using SqlERDiagrammDrawer.SQLBuisnessObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlERDiagrammDrawer.DataLayer
{
    internal class SQLCommandBuilder
    {
        public string DropTable(SQLEntity e)
        {
            return $"DROP TABLE IF EXISTS `{e.NameEntity}`;";

        }

        public string CreateEntitySqlCommand(SQLEntity Entity)
        {


            string commandCreate = $"CREATE TABLE `{Entity.NameEntity}` (";
            string primaryKey = string.Empty;
            string fields = string.Empty;
            for (int i = 0; i < Entity.Fields.Count; i++)
            {


                string field = $"`{Entity.Fields[i].Name}` {MyValuesDictionary.EntityVariableType[Entity.Fields[i].VariableTyp]}";


                if (Entity.Fields[i].Primary)
                {
                    field += " NOT NULL ";
                    primaryKey = $" PRIMARY KEY(`{Entity.Fields[i].Name}`) ) ";

                    if (Entity.Fields[i].AutoIncremental)
                    {
                        field += "auto_increment";
                        primaryKey += "AUTO_INCREMENT = 1";
                    }


                }
                else
                {
                    if (Entity.Fields[i].Nullable)
                    {

                        field += " default NULL ";

                    }

                }

                field += ",";



                fields += field;


            }

            primaryKey += ";";

            return commandCreate + fields + primaryKey;
        }

        public string CreateKey(SQLBuisnessObj.Keys Key)
        {

            return $"ALTER TABLE {Key.ForeingTableName}" +
             $" ADD CONSTRAINT {Key.ForeingKey.Name}" +
              $" FOREIGN KEY({Key.PrimaryKey.Name}) REFERENCES {Key.PrimaryTableName}({Key.PrimaryKey.Name});";





        }


    }
}

