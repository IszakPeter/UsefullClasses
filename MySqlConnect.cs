     public class MySqlConnect
     {
         private MySqlConnection connection ;

         public MySqlConnect(string server, string database,string user,string password) {
             try
             {
                 this.connection = new MySqlConnection($"server={server};database={database};uid={user};Pwd={password};character set=utf8");
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
                     using var cmd = new MySqlCommand(sql, this.connection);
                     return cmd.ExecuteNonQuery();
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
             Console.WriteLine(sql.Pastel("#FF6404"));
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
         
     }
