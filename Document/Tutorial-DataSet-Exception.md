
### チュートリアル#03-05

例外発生のテスト
================

ここでは、メソッドの中で例外が発生することを想定したテストの例を紹介します。


テスト対象のメソッド
--------------------

[Delete メソッド](./Tutorial-DataSet-Delete.md) を以下のように修正し、
parameter の ID が DBNull の場合に例外が発生するようにしました。

```c#
    namespace TutorialCodeFirst
    {
        public class EmployeeStore
        {
            (...)
            
            public void Delete(DataTable parameter)
            {
                if (parameter.Rows[0]["ID"] == DBNull.Value)
                {
                    throw new ApplicationException("Invalid ID");
                }

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


テストケース
------------

以下のテストケースをテストします。

*   <b>指定した従業員データのIDが null ならば "Invalid ID" というメッセージを持つ ApplicationException が発生する</b>


FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   Delete メソッドの引数として渡すデータを `D.パラメタ` に記述する。
    ID列には何も入力しない。
    ただし、全列に値が入力されていない場合、 Excel ファイルの読み込み時に「行そのものが存在しない」と解釈されてしまうため、
    左端の列に `C` と入力しておく（データが存在することを示す印です）。
*   キャッチした例外の内容をチェックするために `E.取得データ` の記述を行う。
    ここでは、例外メッセージの内容が想定通りかどうかをチェックしている。


![FixtureBook記述](./images/Tutorial-DataSet-Exception-01.png?raw=true)


テストメソッドの記述
--------------------

Validate メソッドの引数として、例外を発生させるテストコードをラムダ式で記述します。

    [TestMethod]
    [Fixture("Delete", @"指定した従業員データのIDが null ならば ""Invalid ID"" というメッセージを持つ ApplicationException が発生する")]
    public void Delete__指定した従業員データのIDがnullならばInvalid_IDというメッセージを持つApplicationExceptionが発生する()
    {
        // setup
        DataTable dataTable = fixtureBook.GetObject<DataTable>("EMPLOYEE");

        // expect
        fixtureBook.Validate<ApplicationException>(() => new EmployeeStore().Delete(dataTable));
    }
