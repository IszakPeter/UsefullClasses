from mysql import connector


class MySqlConnector:
    def __init__(self, options={"host": "localhost", "port":'3306', "user": "root", "password": "", "database": ""}):
        self.conector = connector.connect(
            host=options['host'],
            port=options['port'],
            user=options['user'],
            password=options['password'],
            database=options['database']
        )
        self.cursor = self.conector.cursor()

    def NoQuery(self, sql):
        self.cursor.execute(sql)

    def Query(self, sql):
        self.cursor.execute(sql)
        return self.cursor.fetchall()

    def ListTabels(self):
        self.cursor.execute('SHOW TABLES;')
        return [i[0] for i in self.cursor.fetchall()]

    def ListDatabases(self):
        self.cursor.execute('SHOW DATABASES;')
        return [i[0] for i in self.cursor.fetchall()]

    def SetDatabase(self,db):
        self.cursor.execute(f'Use `{db}`;')

    def GetColumns(self, x):
        sql = f"""SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'{x}' """
        try:
            self.cursor.execute(sql)
            return self.cursor.fetchall()
        except Exception as e:
            print(e)
            return e