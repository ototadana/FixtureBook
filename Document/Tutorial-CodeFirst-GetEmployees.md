
### チュートリアル#02-04

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
            
            public List<Employee> GetEmployees(Employee parameter)
            {
                using (MyAppDbContext context = new MyAppDbContext())
                {
                    return (from employee in context.Employees
                            where employee.Retire == parameter.Retire
                            orderby employee.Id
                            select employee).ToList();
                }
            }
        }
    }
```

GetEmployees メソッドは、parameter 引数の条件 (退職フラグ Retire の true / false) に従って、
Employees テーブル内のデータを抽出し、リスト形式 (`List<Employee> `) で返します。


テストケース
------------

以下の2ケースをテストします。

*   <b>引数の退職フラグがtrueの場合、データベーステーブル (Employees) 上の退職者のみが取得できる</b>
*   <b>引数の退職フラグがfalseの場合、データベーステーブル (Employees) 上の未退職者のみが取得できる</b>


FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   `B.テストデータクリア条件` で `*` を指定して、一旦全データを消去する。
*   `B.テストデータ` に退職者と未退職者双方のデータを登録する。
*   GetEmployees メソッドの引数として渡すデータを `D.パラメタ` に記述する。
*   GetEmployees でデータ取得できるはずのデータを予想結果として `E.取得データ`に記述する。


![FixtureBook記述1](./images/Tutorial-CodeFirst-GetEmployees-01.png?raw=true)

![FixtureBook記述2](./images/Tutorial-CodeFirst-GetEmployees-02.png?raw=true)



テストメソッドの記述
--------------------

テストメソッドの記述内容は以下のようになります。

1.  `fixtureBook.GetObject<Employee>()` を呼び出し、`D.パラメタ` に定義された内容で初期化された 
    Employee オブジェクトを取得する。
2.  取得した Employee オブジェクトを引数にしてテスト対象メソッド `GetEmployees()` を呼び出して、
    戻り値 (`List<Employee> employees`) を取得する。
3.  `fixtureBook.Validate(employees)` を呼び出し、戻り値 employees の内容が正しいかどうかをチェックする。

```
    [TestMethod]
    public void GetEmployees__引数の退職フラグがtrueの場合データベーステーブルEmployees上の退職者のみが取得できる()
    {
        TestGetEmployees();
    }

    [TestMethod]
    public void GetEmployees__引数の退職フラグがfalseの場合データベーステーブルEmployees上の未退職者のみが取得できる()
    {
        TestGetEmployees();
    }

    private void TestGetEmployees()
    {
        // setup
        Employee parameter = fixtureBook.GetObject<Employee>();

        // when
        List<Employee> employees = new EmployeeStore().GetEmployees(parameter);

        // then
        fixtureBook.Validate(employees);
    }
```

