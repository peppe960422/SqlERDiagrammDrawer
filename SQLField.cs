using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlERDiagrammDrawer
{
    public class SQLField
    {
        public string Name { get; set; }

        public int VariableTyp { get; set; }

        public bool AutoIncremental { get; set; }

        public bool Nullable { get; set; }

        public bool Primary { get; set; }

        public bool Foreing { get; set; }

        public override string ToString()
        {
            string Name = this.Name;

            string typ = MyValuesDictionary.EntityVariableType[VariableTyp];

            string isSpecial = " ";
            string isNullable = " ";
            ;

            if (this.Primary) { isSpecial = "PK"; }
            else if (this.Foreing) { isSpecial = "FK"; }
            if (this.Nullable) { isNullable = "Null"; }

            ;
            return $" {Name} | {typ} | {isSpecial} | {isNullable}";

        }



    }
    public static class MyValuesDictionary
    {
        static readonly public Dictionary<int, string> EntityVariableType = new Dictionary<int, string>
{
    { 0, "varchar(50)" },
    { 1, "smallint" },
    { 2, "int" },
    { 3, "bigint" },
    { 4, "bit" },
    { 5, "decimal(18,2)" },
    { 6, "float" },
    { 7, "real" },
    { 8, "date" },
    { 9, "datetime" },
    {10, "datetime2" },
    {11, "time" },
    {12, "char" },
    {13, "varchar(max)" },
    {14, "nvarchar(50)" },
    {15, "nvarchar(max)" },
    {16, "text" },
    {17, "uniqueidentifier" },
    {18, "binary(50)" },
    {19, "varbinary(max)" },
    {20,"enum" },
    {21,"decimal" },
    {22, "tinytext" }
};
    }

    public class SQLEntity
    {
        public string NameEntity { get; set; }

        public List<SQLField> Fields { get; set; } = new List<SQLField>();



    }
    public class Keys
    {
        public string PrimaryTableName { get; set; }
        public SQLField PrimaryKey { get; set; }
        public string ForeingTableName { get; set; }

        public SQLField ForeingKey { get; set; }


    }

}
