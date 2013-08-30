
### チュートリアル#03-06

テストメソッドの簡略化
======================

ここまでの例を見てもわかるようにテストメソッドの記述は、

1.  GetObject / GetList を呼び出してテスト対象メソッド引数に渡すデータを作成する。
2.  テスト対象処理を呼び出す。
3.  テスト対象処理が戻り値を返すならば、Validate メソッドを呼び出す。
4.  テスト対象処理がデータベースを更新するならば、ValidateStorage メソッドを呼び出す。

という決まりきったものになりがちです。

そこで FixtureBook クラスには、上記のような決まりきった処理を1行で書けるようにするためのメソッドが用意されています。

*   `Expect` (戻り値の検証が必要ない場合に使う)
*   `ExpectReturn` (戻り値を検証したい場合に使う)
*   `ExpectThrown` (例外発生を検証したい場合に使う)


テストメソッド記述例
--------------------

Expect / ExpectReturn / ExpectThrown メソッドを使用する場合には、
テスト対象の処理をラムダ式で記述します。

これまでのテストメソッドを書き直すと以下のようになります。


    [TestClass]
    public class EmployeeStoreTest
    {
        [TestMethod]
        public void Save__データベーステーブルEMPLOYEEに従業員データを新規追加できる()
        {
            FixtureBook.Expect((DataSet dataSet) => new EmployeeStore().Save(dataSet));
        }

        [TestMethod]
        public void Delete__指定した従業員データのIDをキーにしてデータベーステーブルEMPLOYEE上のデータが削除される()
        {
            FixtureBook.Expect((DataTable dataTable) => new EmployeeStore().Delete(dataTable));
        }

        [TestMethod]
        public void GetAllEmployees__データベーステーブルEMPLOYEE上の全データが取得できる()
        {
            FixtureBook.ExpectReturn(() => new EmployeeStore().GetAllEmployees());
        }

        [TestMethod]
        public void GetEmployees__引数の退職フラグが1の場合データベーステーブルEMPLOYEE上の退職者のみが取得できる()
        {
            FixtureBook.ExpectReturn((DataTable parameter) => new EmployeeStore().GetEmployees(parameter));
        }

        [TestMethod]
        public void GetEmployees__引数の退職フラグが0の場合データベーステーブルEMPLOYEE上の未退職者のみが取得できる()
        {
            FixtureBook.ExpectReturn((DataTable parameter) => new EmployeeStore().GetEmployees(parameter));
        }

        [TestMethod]
        [Fixture("Delete", @"指定した従業員データのIDが null ならば ""Invalid ID"" というメッセージを持つ ApplicationException が発生する")]
        public void Delete__指定した従業員データのIDがnullならばInvalid_IDというメッセージを持つApplicationExceptionが発生する()
        {
            FixtureBook.ExpectThrown<DataTable, ApplicationException>(dataTable => new EmployeeStore().Delete(dataTable));
        }
    }



ポイント
--------

*   ラムダ式での引数が複数ある場合、`D.パラメタ` で定義した順番で引数を記述してください。
*   DataSet型の引数がある場合、`D.パラメタ` にある全てのテーブル定義が格納されます。
*   ExpectReturn での検証は、`E.取得データ` に記述されている一番最初のテーブル定義を利用して行われます。
*   ただし、DataSet型の戻り値を ExpectReturn で検証する場合は、全てのテーブル定義を利用して行われます。


------------------------

*   [チュートリアル#03 - DataSet / DataTableを使った開発での利用例](./Tutorial-DataSet.md)
    *   [#03-01 データ登録メソッド Save のテスト](./Tutorial-DataSet-Save.md)
    *   [#03-02 データ削除メソッド Delete のテスト](./Tutorial-DataSet-Delete.md)
    *   [#03-03 データ取得メソッド GetAllEmployees のテスト](./Tutorial-DataSet-GetAllEmployees.md)
    *   [#03-04 データ検索メソッド GetEmployees のテスト](./Tutorial-DataSet-GetEmployees.md)
    *   [#03-05 例外発生のテスト](./Tutorial-DataSet-Exception.md)
    *   #03-06 テストメソッドの簡略化
