
### チュートリアル#02-03

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
            
            public List<Employee> GetAllEmployees()
            {
                using (MyAppDbContext context = new MyAppDbContext())
                {
                    return (from employee in context.Employees
                            orderby employee.Id
                            select employee).ToList();
                }
            }

            (...)
        }
    }
```


GetAllEmployees メソッドは、Employees テーブル上にある全てのデータをリスト形式 (`List<Employee>`) で取得します。


テストケース
------------

以下のテストケースをテストします。

*   <b>データベーステーブル (Employees) 上の全データが取得できる</b>


FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   `B.テストデータクリア条件` で `*` を指定して、一旦全データを消去する。
*   GetAllEmployees でデータ取得できるようにするために、`B.テストデータ` に何件かデータを記述する。
*   GetAllEmployees で取得できるはずのデータを予想結果として `E.取得データ`に記述する。


![FixtureBook記述](./images/Tutorial-CodeFirst-GetAllEmployees-01.png?raw=true)

*   E.取得データ は、GetAllEmployees メソッドの戻り値 `List<Employee>` を検証するための記述なので、
    テーブル名ではなくクラス名の `Employee` を指定します。


テストメソッドの記述
--------------------

次に単体テストクラスにテストメソッドを追加します。

メソッドの記述内容は以下のようになります。

1.  `fixtureBook.Setup()` を呼び出し、`B.テストデータクリア条件`でのデータ削除と
    `B.テストデータ`のデータ投入を行う。
2.  テスト対象メソッド `GetAllEmployees()` を呼び出して、戻り値 (`List<Employee> employees`) を取得する。
3.  `fixtureBook.Validate(employees)` を呼び出し、戻り値 employees の内容が正しいかどうかをチェックする。


```
    [TestMethod]
    public void GetAllEmployees__データベーステーブルEmployees上の全データが取得できる()
    {
        // setup
        fixtureBook.Setup();
        
        // when
        List<Employee> employees = new EmployeeStore().GetAllEmployees();
        
        // then
        fixtureBook.Validate(employees);
    }
```


