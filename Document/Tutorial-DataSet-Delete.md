
### チュートリアル#03-02

データ削除メソッド Delete のテスト
==================================

テスト対象のメソッド
--------------------

ここでは、以下の Delete メソッドを対象にテストを行います。

```c#
    namespace TutorialCodeFirst
    {
        public class EmployeeStore
        {
            (...)
            
            public void Delete(DataTable parameter)
            {
                using (OracleConnection connection = GetConnection())
                {
                    OracleCommand deleteCommand = new OracleCommand("DELETE FROM EMPLOYEE WHERE ID = :ID");
                    deleteCommand.Connection = connection;
                    deleteCommand.Parameters.Add(new OracleParameter(":ID", parameter.Rows[0]["ID"]));
                    deleteCommand.ExecuteNonQuery();
                }
            }
            
            (...)
        }
    }
```

Delete メソッドは、引数として渡された従業員データのIDに合致する行をデータベーステーブル
(EMPLOYEE) から削除します。


テストケース
------------

以下のテストケースをテストします。

*   <b>指定した従業員データのIDをキーにしてデータベーステーブル (EMPLOYEE) 上のデータが削除される</b>


FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   `B.テストデータクリア条件` で `*` を指定して、一旦全データを消去する。
*   削除されるデータと削除されないデータをそれぞれ1件ずつデータベーステーブル (EMPLOYEE) に事前登録するために、
    `B.テストデータ` に記述する。
*   Delete メソッドの引数として渡すデータを `D.パラメタ` に記述する。
    Delete メソッドでは ID列以外は利用されないため、ID列のみ記述する。
*   Delete メソッドの実行後の EMPLOYEE テーブルのデータ状態を予想して `F.更新後データ` に記述する。
    削除されるデータ行のC列には、削除済みを表す 'D' を記述する。


![FixtureBook記述](./images/Tutorial-DataSet-Delete-01.png?raw=true)


テストメソッドの記述
--------------------

テストメソッドは以下の内容で記述します。

1.  Delete メソッドの引数として渡す DatTable を `fixtureBook.GetObject<DataTable>("EMPLOYEE")` で取得する。
    これにより、`D.パラメタ` の "EMPLOYEE" で記述した値で初期化された DataTable が取得できる。
2.  上記で取得した DataTable を引数にして、テスト対象メソッド (Delete) を呼び出す。
3.  `fixtureBook.ValidateStorage()` の呼び出しにより、データベーステーブル状態を検証する。
    これにより、`F.更新後データ`に記述した内容と、
    実際のデータベーステーブルの状態が同じかどうかのチェックが行われる。

```
    [TestMethod]
    public void Delete__指定した従業員データのIDをキーにしてデータベーステーブルEMPLOYEE上のデータが削除される()
    {
        // setup
        DataTable dataTable = fixtureBook.GetObject<DataTable>("EMPLOYEE");

        // when
        new EmployeeStore().Delete(dataTable);

        // then
        fixtureBook.ValidateStorage();
    }
```

*   今回のように `D.パラメタ` にひとつのテーブル定義しか記述されていない場合は、
    `fixtureBook.GetObject<DataTable>()` というふうに引数の "EMPLOYEE" 
    を省略してメソッド呼び出しを行うこともできます。



------------------------

*   [チュートリアル#03 - DataSet / DataTableを使った開発での利用例](./Tutorial-DataSet.md)
    *   [#03-01 データ登録メソッド Save のテスト](./Tutorial-DataSet-Save.md)
    *   #03-02 データ削除メソッド Delete のテスト
    *   [#03-03 データ取得メソッド GetAllEmployees のテスト](./Tutorial-DataSet-GetAllEmployees.md)
    *   [#03-04 データ検索メソッド GetEmployees のテスト](./Tutorial-DataSet-GetEmployees.md)
    *   [#03-05 例外発生のテスト](./Tutorial-DataSet-Exception.md)
