
### チュートリアル#03-03

データ取得メソッド GetAllEmployees のテスト
===========================================

テスト対象のメソッド
--------------------

ここでは、以下の GetAllEmployees メソッドを対象にテストを行います。

```c#
    namespace TutorialCodeFirst
    {
        public class EmployeeStore
        {
            (...)
            
            public DataTable GetAllEmployees()
            {
                using (OracleConnection connection = GetConnection())
                {
                    OracleCommand selectCommand = new OracleCommand("SELECT * FROM EMPLOYEE");
                    selectCommand.Connection = connection;
                    OracleDataAdapter adapter = new OracleDataAdapter(selectCommand);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }

            (...)
        }
    }
```

GetAllEmployees メソッドは、EMPLOYEE テーブル上にある全てのデータを DataTable として取得します。


テストケース
------------

以下のテストケースをテストします。

*   <b>データベーステーブル (EMPLOYEE) 上の全データが取得できる</b>


FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   `B.テストデータクリア条件` で `*` を指定して、一旦全データを消去する。
*   GetAllEmployees でデータ取得できるようにするために、`B.テストデータ` に何件かデータを記述する。
*   GetAllEmployees で取得できるはずのデータを予想結果として `E.取得データ` に記述する。


![FixtureBook記述](./images/Tutorial-DataSet-GetAllEmployees-01.png?raw=true)


テストメソッドの記述
--------------------

次に単体テストクラスにテストメソッドを追加します。

メソッドの記述内容は以下のようになります。

1.  `fixtureBook.Setup()` を呼び出し、`B.テストデータクリア条件` でのデータ削除と
    `B.テストデータ` のデータ投入を行う。
2.  テスト対象メソッド `GetAllEmployees()` を呼び出して、戻り値 (`DataTable employees`) を取得する。
3.  `fixtureBook.Validate(employees)` を呼び出し、戻り値 employees の内容が正しいかどうかをチェックする。

&nbsp;

    [TestMethod]
    public void GetAllEmployees__データベーステーブルEMPLOYEE上の全データが取得できる()
    {
        // setup
        fixtureBook.Setup();

        // when
        DataTable employees = new EmployeeStore().GetAllEmployees();

        // then
        fixtureBook.Validate(employees, "EMPLOYEE");
    }


ここでは、`fixtureBook.Validate(employees, "EMPLOYEE")` というふうに、
`E.取得データ` の中の "EMPLOYEE" という名前のテーブル定義と比較チェックすることを明示的に指定していますが、
今回のように `E.取得データ` にひとつしかテーブル定義がない場合は、
`fixtureBook.Validate(employees)` というふうに "EMPLOYEE" を省略することが可能です。

また、GetAllEmployees() メソッドの

```c#
    DataTable dataTable = new DataTable();
    adapter.Fill(dataTable);
```

の部分を

```c#
    DataTable dataTable = new DataTable("EMPLOYEE");
    adapter.Fill(dataTable);
```

というふうに変更して、DataTable にテーブル名を設定すれば、
`E.取得データ` に複数のテーブル定義があっても、
`fixtureBook.Validate(employees)` と省略することが可能です
（DataTable に設定されている名前で `E.取得データ` のテーブル定義を検索します）。


------------------------

*   [チュートリアル#03 - DataSet / DataTableを使った開発での利用例](./Tutorial-DataSet.md)
    *   [#03-01 データ登録メソッド Save のテスト](./Tutorial-DataSet-Save.md)
    *   [#03-02 データ削除メソッド Delete のテスト](./Tutorial-DataSet-Delete.md)
    *   #03-03 データ取得メソッド GetAllEmployees のテスト
    *   [#03-04 データ検索メソッド GetEmployees のテスト](./Tutorial-DataSet-GetEmployees.md)
    *   [#03-05 例外発生のテスト](./Tutorial-DataSet-Exception.md)
