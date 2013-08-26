
### チュートリアル #02

Entity Framework コードファースト開発での利用例
===============================================

ここでは、Entity Framework を使って「コード・ファースト」のスタイルで開発を行っているプロジェクトでの利用例を説明します。


テスト対象のアプリケーション
----------------------------

このチュートリアルでは、以下のような構成のアプリケーションを対象にテストを行います。

### エンティティ

```c#
    using System;

    namespace TutorialCodeFirst
    {
        public class Employee
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public bool Retire { get; set; }
            public DateTime LastUpdated { get; set; }
        }
    }
```

このコードを使うと、以下のようなデータベーステーブルが Entity Framework によって作成されます。

*   データベーステーブル名は、"Employees" (Employee の複数形) になる。
*   Id は自動採番で値設定される。


### DbContext

```c#
    using System.Data.Entity;

    namespace TutorialCodeFirst
    {
        public class MyAppDbContext : DbContext
        {
            public DbSet<Employee> Employees { get; set; }
        }
    }
```


### データベース操作を行うクラス（テスト対象のクラス）

```c#
    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace TutorialCodeFirst
    {
        public class EmployeeStore
        {
            public void Save(List<Employee> employees)
            {
                using (MyAppDbContext context = new MyAppDbContext())
                {
                    employees.ForEach(employee => employee.LastUpdated = DateTime.Now);
                    context.Employees.AddOrUpdate(employees.ToArray());
                    context.SaveChanges();
                }
            }
            
            public void Delete(List<Employee> employees)
            {
                using (MyAppDbContext context = new MyAppDbContext())
                {
                    employees.ForEach(employee => context.Employees.Remove(employee));
                    context.SaveChanges();
                }
            }
            
            public List<Employee> GetAllEmployees()
            {
                using (MyAppDbContext context = new MyAppDbContext())
                {
                    return (from employee in context.Employees
                            orderby employee.Id 
                            select employee).ToList();
                }
            }
            
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

このクラスでは以下の処理を行います。

*   Save は引数に指定された従業員データの保存を行う。
*   Delete  は引数に指定された従業員データの削除を行う。
*   GetAllEmployees はデータベーステーブル上に登録されている従業員データを全て取得する。
*   GetEmployees では、引数に指定されている従業員情報の Retire プロパティの値を条件にして従業員データを抽出する。



事前準備
--------

まず、コード・ファーストで開発を行う場合に必要となる、単体テストプロジェクトの設定を説明します。

コード・ファーストで開発を行う場合、
[チュートリアル#01 セットアップ](./Tutorial-Setup.md) で行った設定に加えて、
単体テストプロジェクトに以下のような設定が必要になります。

1.  Entity Framework をインストールする。
2.  App.config のデータベース接続設定の名前を変更する。
3.  DbMigrationsConfiguration のサブクラスを作成する。
4.  単体テストクラスの初期化メソッドでデータベース作成および変更反映を行う。


### Entity Framework のインストール

NuGet 等を使って単体テストプロジェクトにも Entity Framework をインストールしてください。


### App.config のデータベース接続設定の名前変更

Entity Framework を利用する場合、デフォルトでは、
データベース接続設定の名前（name 属性の値）は DbContext クラスの名前と同じにする必要があります。
今回の DbContext クラスは MyAppDbContext という名前で作成したため、以下のような定義に変更します。

```xml
    <?xml version="1.0" encoding="utf-8"?>
    <configuration>
      <connectionStrings>
        <clear/>
        <add name="MyAppDbContext" providerName="System.Data.SqlClient" connectionString="Data Source=(LocalDB)\v11.0;integrated security=True;Initial Catalog=TutorialCodeFirstTest"/>
      </connectionStrings>
    </configuration>
```


### DbMigrationsConfiguration のサブクラスを作成する

単体テストプロジェクトの中に
DbMigrationsConfiguration のサブクラスを以下のような形で作成します。

```c#
    using System.Data.Entity.Migrations;
    using TutorialCodeFirst;

    namespace TutorialCodeFirstTest
    {
        class MyDbConfiguration : DbMigrationsConfiguration<MyAppDbContext>
        {
            public MyDbConfiguration()
            {
                AutomaticMigrationsEnabled = true;
                AutomaticMigrationDataLossAllowed = true;
            }
        }
    }
```

このクラスは、次項で説明する処理で利用します。


### 単体テストクラスの初期化メソッドでデータベース作成および変更反映を行う

テストが実行される前には、データベースが存在し、かつ、データベーステーブルがエンティティ定義（この例では Employee.cs）
と同期がとれている必要があります。
そこで、単体テストクラスの中に、この処理を記述します。

具体的には、`[ClassInitialize]` 属性指定したメソッドを作成し、以下のふたつの処理を記述します。

*   `Database.SetInitializer` を呼び出して自動反映用のイニシャライザを登録する。
*   ダミーのデータベース操作を行い、テスト前に確実にデータベース及びデータベーステーブルが作成されるようにする。

下記では、InitializeDatabase の中にこの処理を記述しています。

```c#
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.Entity;
    using TutorialCodeFirst;

    namespace TutorialCodeFirstTest
    {
        [TestClass]
        public class EmployeeStoreTest
        {
            [ClassInitialize]
            public static void InitializeDatabase(TestContext context)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyAppDbContext, MyDbConfiguration>());
                new MyAppDbContext().Employees.Find(0); // ダミーのデータベース操作
            }
        }
    }
```


FixtureBook の作成
------------------

FixtureBook (テストデータを記述する Excel ファイル) を作成します。

ファイルは `.xlsx` 形式で作成してください。
ファイルの作成場所は単体テストクラスのあるフォルダ、ファイル名は「単体テストクラス名.xlsx」とすると、
単体テストクラスでのファイルパス指定が省略できるため、おすすめです。
例えば、今回の場合は EmployeeStoreTest.xlsx になります。

![今回作成したファイル](./images/Tutorial-CodeFirst-01.png?raw=true)



単体テストクラスで FixtureBook を利用可能にする
-----------------------------------------------

作成した FixtureBook を利用可能にするためには、以下のように単体テストクラスを修正します。

1.  `using XPFriend.Fixture;` を追加する。
2.  `private FixtureBook fixtureBook = new FixtureBook();` といった形で fixtureBook フィールドを追加する。

```c#
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Data.Entity;
    using TutorialCodeFirst;
    using XPFriend.Fixture;

    namespace TutorialCodeFirstTest
    {
        [TestClass]
        public class EmployeeStoreTest
        {
            private FixtureBook fixtureBook = new FixtureBook();

            [ClassInitialize]
            public static void InitializeDatabase(TestContext context)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyAppDbContext, MyDbConfiguration>());
                new MyAppDbContext().Employees.Find(0); // ダミーのデータベース操作
            }
        }
    }
```


実際のテストの例
----------------

これで Entity Framework を使ってコード・ファーストのスタイルで開発を行っているプロジェクトでの利用準備ができました。
実際のテスト方法については以下のドキュメントに記載されていますので、そちらでどうぞ。

*   [#02-01 データ登録メソッド Save のテスト](./Tutorial-CodeFirst-Save.md)
*   [#02-02 データ削除メソッド Delete のテスト](./Tutorial-CodeFirst-Delete.md)
*   [#02-03 データ取得メソッド GetAllEmployees のテスト](./Tutorial-CodeFirst-GetAllEmployees.md)
*   [#02-04 データ検索メソッド GetEmployees のテスト](./Tutorial-CodeFirst-GetEmployees.md)
*   [#02-05 例外発生のテスト](./Tutorial-CodeFirst-Exception.md)

