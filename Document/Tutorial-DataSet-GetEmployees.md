
### チュートリアル#03-04

データ検索メソッド GetEmployees のテスト
========================================

テスト対象のメソッド
--------------------

ここでは、以下の GetEmployees メソッドを対象にテストを行います。

```c#
    namespace TutorialCodeFirst
    {
        public class EmployeeStore
        {
            (...)
            
            public DataSet GetEmployees(DataTable parameter)
            {
                using (OracleConnection connection = GetConnection())
                {
                    OracleCommand selectCommand = new OracleCommand("SELECT * FROM EMPLOYEE where RETIRE = :RETIRE");
                    selectCommand.Connection = connection;
                    selectCommand.Parameters.Add(new OracleParameter(":RETIRE", parameter.Rows[0]["RETIRE"]));
                    OracleDataAdapter adapter = new OracleDataAdapter(selectCommand);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    return dataSet;
                }
            }
        }
    }
```

GetEmployees メソッドは、parameter 引数の条件 (退職フラグ RETIRE の 1/0) に従って、
EMPLOYEE テーブル内のデータを抽出し、DataSet として返します。


テストケース
------------

以下の2ケースをテストします。

*   <b>引数の退職フラグが1の場合、データベーステーブル (EMPLOYEE) 上の退職者のみが取得できる</b>
*   <b>引数の退職フラグが0の場合、データベーステーブル (EMPLOYEE) 上の未退職者のみが取得できる</b>


FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   `B.テストデータクリア条件` で `*` を指定して、一旦全データを消去する。
*   `B.テストデータ` に退職者と未退職者双方のデータを登録する。
*   GetEmployees メソッドの引数として渡すデータを `D.パラメタ` に記述する。
*   GetEmployees でデータ取得できるはずのデータを予想結果として `E.取得データ` に記述する。


![FixtureBook記述1](./images/Tutorial-DataSet-GetEmployees-01.png?raw=true)

![FixtureBook記述2](./images/Tutorial-DataSet-GetEmployees-02.png?raw=true)



テストメソッドの記述
--------------------

テストメソッドの記述内容は以下のようになります。

1.  `fixtureBook.GetObject<DataTable>()` を呼び出し、`D.パラメタ` に定義された内容で初期化された 
    従業員データを DataTable としてを取得する。
2.  取得した DataTable を引数にしてテスト対象メソッド `GetEmployees()` を呼び出して、
    戻り値 (`DataSet employees`) を取得する。
3.  `fixtureBook.Validate(employees)` を呼び出し、戻り値 employees の内容が正しいかどうかをチェックする。

```
    [TestMethod]
    public void GetEmployees__引数の退職フラグが1の場合データベーステーブルEMPLOYEE上の退職者のみが取得できる()
    {
        TestGetEmployees();
    }

    [TestMethod]
    public void GetEmployees__引数の退職フラグが0の場合データベーステーブルEMPLOYEE上の未退職者のみが取得できる()
    {
        TestGetEmployees();
    }

    private void TestGetEmployees()
    {
        // setup
        DataTable parameter = fixtureBook.GetObject<DataTable>();

        // when
        DataSet employees = new EmployeeStore().GetEmployees(parameter);

        // then
        fixtureBook.Validate(employees, "EMPLOYEE");
    }
```
