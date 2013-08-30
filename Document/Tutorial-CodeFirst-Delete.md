
### チュートリアル#02-02

 データ削除メソッド Delete のテスト
===================================

テスト対象のメソッド
--------------------

ここでは、以下の Delete メソッドを対象にテストを行います。


```c#
    namespace TutorialCodeFirst
    {
        public class EmployeeStore
        {
            (...)
            
            public void Delete(List<Employee> employees)
            {
                using (MyAppDbContext context = new MyAppDbContext())
                {
                    employees.ForEach(employee => context.Employees.Remove(employee));
                    context.SaveChanges();
                }
            }

            (...)
        }
    }
```


Delete メソッドは、引数として渡された従業員データ (Employee) のIdに合致する行をデータベーステーブル
(Employees) から削除します。


テストケース
------------

以下のテストケースをテストします。

*   <b>指定した従業員データのIdをキーにしてデータベーステーブル (Employees) 上のデータが削除される</b>


FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   `B.テストデータクリア条件` で `*` を指定して、一旦全データを消去する。
*   削除されるデータと削除されないデータをそれぞれ1件ずつデータベーステーブル (Employees) に事前登録するために、
    `B.テストデータ` に記述する。
*   Delete メソッドの引数として渡すデータを `D.パラメタ` に記述する。
    Delete メソッドでは Id以外のプロパティは利用されないため、Id列のみ記述する。
*   Delete メソッドの実行後の Employees テーブルのデータ状態を予想して`F.更新後データ`に記述する。
    削除されるデータ行のC列には、削除済みを表す `D` を記述する。


![FixtureBook記述](./images/Tutorial-CodeFirst-Delete-01.png?raw=true)


テストメソッドの記述
--------------------

次に単体テストクラスにテストメソッドを追加します。

テスト対象メソッドが変わっただけで、ほかは Save メソッドのテスト内容と同じです。


```
    [TestMethod]
    public void Delete__指定した従業員データのIdをキーにしてデータベーステーブルEmployees上のデータが削除される()
    {
        // setup
        List<Employee> employees = fixtureBook.GetList<Employee>();

        // when
        new EmployeeStore().Delete(employees);

        // then
        fixtureBook.ValidateStorage();
    }
```


------------------------

*   [チュートリアル#02 - Entity Framework コードファースト開発での利用例](./Tutorial-CodeFirst.md)
    *   [#02-01 データ登録メソッド Save のテスト](./Tutorial-CodeFirst-Save.md)
    *   #02-02 データ削除メソッド Delete のテスト
    *   [#02-03 データ取得メソッド GetAllEmployees のテスト](./Tutorial-CodeFirst-GetAllEmployees.md)
    *   [#02-04 データ検索メソッド GetEmployees のテスト](./Tutorial-CodeFirst-GetEmployees.md)
    *   [#02-05 例外発生のテスト](./Tutorial-CodeFirst-Exception.md)
    *   [#02-06 テストメソッドの簡略化](./Tutorial-CodeFirst-Expect.md)
