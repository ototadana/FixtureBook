
### チュートリアル#02-06

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
        ...

        [TestMethod]
        public void Save__データベーステーブルEmployeesに存在しないIdの従業員データを渡した場合には新規追加される()
        {
            FixtureBook.Expect((List<Employee> employees) => new EmployeeStore().Save(employees));
        }

        [TestMethod]
        public void Save__データベーステーブルEmployeesに存在するIdの従業員データを渡した場合には既存データが更新される()
        {
            FixtureBook.Expect((List<Employee> employees) => new EmployeeStore().Save(employees));
        }

        [TestMethod]
        public void Delete__指定した従業員データのIdをキーにしてデータベーステーブルEmployees上のデータが削除される()
        {
            FixtureBook.Expect((List<Employee> employees) => new EmployeeStore().Delete(employees));
        }

        [TestMethod]
        public void GetAllEmployees__データベーステーブルEmployees上の全データが取得できる()
        {
            FixtureBook.ExpectReturn(() => new EmployeeStore().GetAllEmployees());
        }

        [TestMethod]
        public void GetEmployees__引数の退職フラグがtrueの場合データベーステーブルEmployees上の退職者のみが取得できる()
        {
            FixtureBook.ExpectReturn((Employee parameter) => new EmployeeStore().GetEmployees(parameter));
        }

        [TestMethod]
        public void GetEmployees__引数の退職フラグがfalseの場合データベーステーブルEmployees上の未退職者のみが取得できる()
        {
            FixtureBook.ExpectReturn((Employee parameter) => new EmployeeStore().GetEmployees(parameter));
        }

        [TestMethod]
        [Fixture("Delete", @"指定した従業員データのIdが 0 ならば ""Invalid Id"" というメッセージを持つ ApplicationException が発生する")]
        public void Delete__指定した従業員データのIdが0ならばInvalid_Idというメッセージを持つApplicationExceptionが発生する()
        {
            FixtureBook.ExpectThrown<List<Employee>, ApplicationException>(employees => new EmployeeStore().Delete(employees));
        }
    }



ポイント
--------

*   ラムダ式での引数が複数ある場合、`D.パラメタ` で定義した順番で引数を記述してください。
*   ExpectReturn での検証は、`E.取得データ` に記述されている一番最初のテーブル定義を利用して行われます。


------------------------

*   [チュートリアル#02 - Entity Framework コードファースト開発での利用例](./Tutorial-CodeFirst.md)
    *   [#02-01 データ登録メソッド Save のテスト](./Tutorial-CodeFirst-Save.md)
    *   [#02-02 データ削除メソッド Delete のテスト](./Tutorial-CodeFirst-Delete.md)
    *   [#02-03 データ取得メソッド GetAllEmployees のテスト](./Tutorial-CodeFirst-GetAllEmployees.md)
    *   [#02-04 データ検索メソッド GetEmployees のテスト](./Tutorial-CodeFirst-GetEmployees.md)
    *   [#02-05 例外発生のテスト](./Tutorial-CodeFirst-Exception.md)
    *   #02-06 テストメソッドの簡略化
