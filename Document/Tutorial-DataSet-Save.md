
### チュートリアル#03-01

データ登録メソッド Save のテスト
================================

テスト対象のメソッド
--------------------

ここでは、以下の Save メソッドを対象にテストを行います。

```c#
    namespace TutorialDataSet
    {
        public class EmployeeStore
        {
            public void Save(DataSet dataSet)
            {
                DataTable table = dataSet.Tables["EMPLOYEE"];
                using (OracleConnection connection = GetConnection())
                {
                    OracleCommand insertCommand = new OracleCommand(
                        "INSERT INTO EMPLOYEE(ID, NAME, AGE, RETIRE, LAST_UPDATED)" + 
                        "VALUES(:ID, :NAME, :AGE, :RETIRE, :LAST_UPDATED)");
                    insertCommand.Connection = connection;
                    foreach(DataRow row in table.Rows) 
                    {
                        insertCommand.Parameters.Clear();
                        insertCommand.Parameters.Add(new OracleParameter(":ID", row["ID"]));
                        insertCommand.Parameters.Add(new OracleParameter(":NAME", row["NAME"]));
                        insertCommand.Parameters.Add(new OracleParameter(":AGE", row["AGE"]));
                        insertCommand.Parameters.Add(new OracleParameter(":RETIRE", row["RETIRE"]));
                        insertCommand.Parameters.Add(new OracleParameter(":LAST_UPDATED", DateTime.Now));
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }

            (...)
        }
    }
```

Save メソッドは、引数として渡された従業員データをデータベーステーブル (EMPLOYEE) に INSERT で登録します。


テストケース
------------

以下のケースについてテストします。

*   <b>データベーステーブルEMPLOYEEに従業員データを新規追加できる</b>


テストプログラムの処理
----------------------

上記のテストケースをテストするために、テストプログラムでは以下のことを行う必要があります。

1.  事前にデータベーステーブル (EMPLOYEE) 内のデータを削除しておく
    (「Saveメソッドの実行によりデータが登録された」ということを確認するため）。
2.  Saveメソッドの引数として渡す従業員データの DataSet を作成する。
3.  Saveメソッドを呼び出す。
4.  Saveメソッドの呼び出しの結果、データベーステーブル (EMPLOYEE) にデータ登録されたことを確認する。


上記の処理の 3 以外はいずれもテストデータを扱う処理となります。
FixtureBook には、これらのデータを記述します。



FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   データベーステーブル (EMPLOYEE) の行削除条件を `B.テストデータクリア条件` に記述する。
*   Save メソッドの引数として渡すデータを `D.パラメタ` に記述する。
*   Save メソッドの実行後の EMPLOYEE テーブルのデータ状態を予想して`F.更新後データ`に記述する。

![FixtureBook記述](./images/Tutorial-DataSet-Save-01.png?raw=true)


### B.テストデータクリア条件

ここでは、EMPLOYEE テーブルの行データをどういう条件で削除するかを指定します。

![B.テストデータクリア条件](./images/Tutorial-DataSet-Save-02.png?raw=true)

この例では、主キー項目である ID 列に `*` を指定することですべての行を削除するように指定しています。


### D.パラメタ

Save メソッドの引数 DataSet に格納するデータを `D.パラメタ` に記述します。

![D.パラメタ](./images/Tutorial-DataSet-Save-03.png?raw=true)

LAST_UPDATE列に関しては Save メソッドの中で自動的に現在時刻を設定するため、
ここでは記述していません。


### F.更新後データ

`F.更新後データ`には、Save メソッド実行後の予想結果 (EMPLOYEE テーブルのあるべき状態) を記述します。

![F.更新後データ](./images/Tutorial-DataSet-Save-04.png?raw=true)

説明の先頭に \* が付いている項目 (この例では `*ID`) は検索条件指定列を意味します。
FixtureBook がデータベースの状態が正しいかどうかのチェックを行う際には、
まず、この列に指定された値を条件にしてデータベースを検索し、
次に、取得したデータ行の各列値について、Excel セル上の値と等しいかどうかを比較チェックします。

LAST_UPDATED列の値として指定されている `${TODAY}` は「本日の日時であればOK」を意味します。
LAST_UPDATED列に関してはSaveメソッドにより現在日時が自動設定されるため、正確な時間を予想することが難しいからです。


テストメソッドの記述
--------------------

次に単体テストクラスにテストメソッドを追加します。

テストメソッドの名前は、`[シート名]__[テストケース記述]` の形式で指定してください。

    [TestMethod]
    public void Save__データベーステーブルEMPLOYEEに従業員データを新規追加できる()

テストケース記述にメソッド名として利用できない文字 (空白や記号等) が含まれている場合は、
以下のように `[Fixture]` 属性を使用してテストケースを指定してください。

    [TestMethod]
    [Fixture("Save", "データベーステーブル (EMPLOYEE) に従業員データを新規追加できる")]
    public void Save__データベーステーブルEMPLOYEEに従業員データを新規追加できる()

テストメソッドは以下の内容で記述します。

1.  Save メソッドの引数として渡すデータを `fixtureBook.GetObject<DataSet>("EMPLOYEE")` で取得する。
    これにより、`D.パラメタ` の "EMPLOYEE" で記述した値で初期化された DataTable をもつ DataSet が取得できる。
2.  上記で取得した DataSet を引数にして、テスト対象メソッド (Save) を呼び出す。
3.  `fixtureBook.ValidateStorage()` の呼び出しにより、データベーステーブル状態を検証する。
    これにより、`F.更新後データ`に記述した内容と実際のデータベーステーブルの状態が同じかどうかのチェックが行われる。

```
    [TestMethod]
    public void Save__データベーステーブルEMPLOYEEに従業員データを新規追加できる()
    {
        // setup
        DataSet dataSet = fixtureBook.GetObject<DataSet>("EMPLOYEE");

        // when
        new EmployeeStore().Save(dataSet);

        // then
        fixtureBook.ValidateStorage();
    }
```

なお、`B.テストデータクリア条件` に記述された条件でのデータベーステーブルからの行削除は、
上記のテストプログラムでは明示的に行っていませんが、
`fixtureBook.GetObject<DataSet>("EMPLOYEE")` の呼び出しの中で自動的に実行されます。

今回の例では GetObject メソッドの引数として "EMPLOYEE" というテーブル定義名を指定していますが、
テーブル定義名を明示的に指定せずに `fixtureBook.GetObject<DataSet>()` とすることもできます。

この場合、`D.パラメタ` に記述されている全てのテーブル定義が DataSet に格納されます。
ただし、今回の例では "EMPLOYEE" しかテーブル記述がないため、
結局 `fixtureBook.GetObject<DataSet>("EMPLOYEE")` も `fixtureBook.GetObject<DataSet>()` も
全く同じ結果になります。

また、`D.パラメタ` に複数のテーブルが定義されており、その中のいくつかをピックアップして
DataSet の中に格納したい場合には、`fixtureBook.GetObject<DataSet>("EMPLOYEE", "COMPANY")` 
というふうに複数の引数を指定することも可能です。


テストの実行
------------

テスト実行の方法は、通常の単体テスト実行と同じです。
エラーが出た場合はエラーメッセージを確認して、テスト対象メソッドの内容やテストデータ記述の修正を行ってください。


![エラーメッセージ例](./images/Tutorial-DataSet-Save-05.png?raw=true)


------------------------

*   [チュートリアル#03 - DataSet / DataTableを使った開発での利用例](./Tutorial-DataSet.md)
    *   #03-01 データ登録メソッド Save のテスト
    *   [#03-02 データ削除メソッド Delete のテスト](./Tutorial-DataSet-Delete.md)
    *   [#03-03 データ取得メソッド GetAllEmployees のテスト](./Tutorial-DataSet-GetAllEmployees.md)
    *   [#03-04 データ検索メソッド GetEmployees のテスト](./Tutorial-DataSet-GetEmployees.md)
    *   [#03-05 例外発生のテスト](./Tutorial-DataSet-Exception.md)
    *   [#03-06 テストメソッドの簡略化](./Tutorial-DataSet-Expect.md)
