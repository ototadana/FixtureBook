
### チュートリアル#02-05

例外発生のテスト
================

ここでは、メソッドの中で例外が発生することを想定したテストの例を紹介します。


テスト対象のメソッド
--------------------

[Delete メソッド](./Tutorial-CodeFirst-Delete.md) を以下のように修正し、
従業員データの Id が 0 の場合に例外が発生するようにしました。


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
                    employees.ForEach(employee =>
                    {
                        if (employee.Id == 0)
                        {
                            throw new ApplicationException("Invalid Id");
                        }
                        context.Employees.Attach(employee);
                        context.Entry(employee).State = EntityState.Deleted;
                    });
                    context.SaveChanges();
                }
            }

            (...)
        }
    }
```



テストケース
------------

以下のテストケースをテストします。

*   <b>指定した従業員データのIdが 0 ならば "Invalid Id" というメッセージを持つ ApplicationException が発生する</b>


FixtureBook の記述
------------------

FixtureBook では以下の記述を行います。

*   Delete メソッドの引数として渡すデータを `D.パラメタ` に記述する。
    Id列には 0 を入力する。
*   キャッチした例外の内容をチェックするために `E.取得データ` の記述を行う。
    ここでは、例外メッセージの内容が想定通りかどうかをチェックしている。


![FixtureBook記述](./images/Tutorial-CodeFirst-Exception-01.png?raw=true)


テストメソッドの記述
--------------------

Validate メソッドの引数として、例外を発生させるテストコードをラムダ式で記述します。

    [TestMethod]
    [Fixture("Delete", @"指定した従業員データのIdが 0 ならば ""Invalid Id"" というメッセージを持つ ApplicationException が発生する")]
    public void Delete__指定した従業員データのIdが0ならばInvalid_Idというメッセージを持つApplicationExceptionが発生する()
    {
        // setup
        List<Employee> employees = fixtureBook.GetList<Employee>();
        
        // expect
        fixtureBook.Validate<ApplicationException>(() => new EmployeeStore().Delete(employees));
    }


