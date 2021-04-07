from sqlite3 import connect


class SQLite:
    def __init__(self, database):
        self.conection = connect(
            database=database)
        self.cursor = self.conection.cursor()

    def NoQuery(self, sql):
        self.cursor.execute(sql)

    def Query(self, sql):
        self.cursor.execute(sql)
        return self.cursor.fetchall()

    def ListTabels(self):
        self.cursor.execute("SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';")
        return [i[0] for i in self.cursor.fetchall()]

    def ListColumns(self, table):
        self.cursor.execute(f"PRAGMA table_info({table})")
        return [i[1] for i in self.cursor.fetchall()]
