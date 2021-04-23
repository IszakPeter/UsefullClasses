public class MySqlConnect
    {
        private MySqlConnection connection;

        public MySqlConnect(string server, string database, string user, string password)
        {
            try
            {
                connection = new MySqlConnection($"server={server};database={database};uid={user};Pwd={password};character set=utf8");
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public int NoQuery(string sql)
        {
            Console.WriteLine(sql);
            try
            {
                using (var cmd = new MySqlCommand(sql, this.connection))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public List<string> Query(string sql, bool names = false)
        {
            Console.WriteLine(sql);
            try
            {
                using (var cmd = new MySqlCommand(sql, this.connection))
                {
                    var l = new List<string>();
                    var colNames = new List<string>();
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            var s = new List<string>();
                            for (var i = 0; i < result.FieldCount; i++)
                            {
                                var t = result.GetName(i);
                                s.Add(result[t].ToString());
                                if (!colNames.Contains(t) && names)
                                    colNames.Add(t);
                            }
                            l.Add(string.Join(";", s));
                        }
                    }
                    if (names) l.Insert(0, string.Join(";", colNames));
                    return l;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public List<List<string>> QueryList(string sql, bool names = false)
        {
            Console.WriteLine(sql);
            try
            {

                using (var cmd = new MySqlCommand(sql, this.connection))
                {
                    var l = new List<List<string>>();
                    var colNames = new List<string>();
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            var s = new List<string>();
                            for (var i = 0; i < result.FieldCount; i++)
                            {
                                var t = result.GetName(i);
                                s.Add(result[t].ToString());
                                if (!colNames.Contains(t) && names)
                                    colNames.Add(t);
                            }
                            l.Add(s);
                        }
                    }
                    if (names) l.Insert(0, colNames);
                    return l;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public DataTable FillDataTable(string sql)
        {
            Console.WriteLine(sql);
            try
            {
                using (var cmd = new MySqlCommand(sql, this.connection))
                {
                    using (var ad = new MySqlDataAdapter(cmd))
                    {
                        var table = new DataTable();
                        ad.Fill(table);
                        return table;
                    }
                }
            }
            catch (Exception e)
            {
                var t = new DataTable();
                t.Columns.Add("error");
                t.Rows.Add(e.ToString());
                return t;
            }
        }
        public string[] GetColumns(string table)
                            => Query($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{table}'").ToArray();
                            
        public string[] GetTables(string database) =>
            Query($@" select table_name
                    from information_schema.tables
                    where table_type = 'BASE TABLE'
    	                    and table_schema not in ('information_schema','mysql','performance_schema','sys')
   	                    AND table_schema='{database}'
                    order by table_name;
                    ").ToArray();

    }
